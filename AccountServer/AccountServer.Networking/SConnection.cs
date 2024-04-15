using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GamePackets;
using GamePackets.Client;
using GamePackets.Server;

namespace AccountServer.Networking;

public sealed class SConnection
{
	private byte[] _rawData = new byte[0];
    private readonly byte[] _rawBytes = new byte[8 * 1024];
    private ConcurrentQueue<GamePacket> ReceivedPackets = new ConcurrentQueue<GamePacket>();
	private ConcurrentQueue<GamePacket> SendPackets = new ConcurrentQueue<GamePacket>();

	public readonly DateTime ConnectionTime;
	public readonly TcpClient Connection;
	public bool IPAddressObtained;
	public string IPAddress;
	public bool LoggedIn;
	public string AccountName;

	public SConnection(TcpClient socket)
	{
		IPAddressObtained = false;
		Connection = socket;
		Connection.NoDelay = true;
		ConnectionTime = DateTime.Now;
		IPAddress = Connection.Client.RemoteEndPoint.ToString().Split(':')[0];
		LoggedIn = false;
		AccountName = string.Empty;

        BeginReceive();
	}

	public void Process()
	{
		try
		{
			ProcessReceivedPackets();
			SendAllPackets();
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage("Network processing error. error:" + ex.Message);
			Connection?.Client?.Close();
		}
	}

	public void SendPacket(GamePacket packet)
	{
		if (packet != null)
            SendPackets.Enqueue(packet);
	}

	private void ProcessReceivedPackets()
	{
		while (!ReceivedPackets.IsEmpty)
		{
			if (ReceivedPackets.TryDequeue(out var p))
			{
				if (!GamePacket.PacketProcessMethodTable.TryGetValue(p.PacketType, out var method))
				{
					Disconnect(new Exception("No packet handling found, disconnected. The type of packet: " + p.PacketType.FullName));
					break;
				}
				method.Invoke(this, new object[1] { p });
			}
		}
	}

	private void SendAllPackets()
	{
		List<byte> data = new List<byte>();
		while (!SendPackets.IsEmpty)
		{
			if (SendPackets.TryDequeue(out var packet))
				data.AddRange(packet.ReadPacket());
		}
		if (data.Count > 0)
			BeginSend(data);
	}

	private void BeginReceive()
	{
        if (Connection == null || !Connection.Connected) return;

        try
		{
			Connection.Client.BeginReceive(_rawBytes, 0, _rawBytes.Length, SocketFlags.None, ReceiveData, _rawBytes);
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage("Asynchronous receive error. error:" + ex.Message);
			Connection?.Client?.Close();
		}
	}

	private void ReceiveData(IAsyncResult result)
	{
		try
		{
			if (Connection == null || Connection.Client == null) return;

			var dataRead = Connection.Client.EndReceive(result);
			if (dataRead == 0)
			{
				Disconnect(new Exception("Disconnect normally."));
				return;
			}

			SEngine.TotalBytesReceived += dataRead;

			byte[] src = result.AsyncState as byte[];
			byte[] dst = new byte[_rawData.Length + dataRead];
			Buffer.BlockCopy(_rawData, 0, dst, 0, _rawData.Length);
			Buffer.BlockCopy(src, 0, dst, _rawData.Length, dataRead);
			_rawData = dst;

			while (true)
			{
				GamePacket packet = GamePacket.GetClientPacket(_rawData, out _rawData);
				if (packet == null)
					break;
				ReceivedPackets?.Enqueue(packet);
			}

			BeginReceive();
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage("Receive Completion Error. error: " + ex.Message);
			Connection?.Client?.Close();
		}
	}

	private void BeginSend(List<byte> data)
	{
		try
		{
			Connection?.Client?.BeginSend(data.ToArray(), 0, data.Count, SocketFlags.None, SendComplete, null);
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage("Asynchronous send error. error:" + ex.Message);
			Connection?.Client?.Close();
		}
	}

	private void SendComplete(IAsyncResult result)
	{
		try
		{
			var dataSent = Connection.Client.EndSend(result);

            SEngine.TotalBytesSent += dataSent;
            if (dataSent == 0)
			{
				SendPackets = new ConcurrentQueue<GamePacket>();
				Disconnect(new Exception("Sending Callback Error!"));
			}
		}
		catch (Exception ex)
		{
			Disconnect(ex);
		}
	}

	public void Disconnect(Exception e)
	{
		SMain.AddLogMessage("Disconnecting. Message: " + e.Message);
		Connection?.Client?.Close();
	}

	public void Process(保持网络连接 P)
	{
	}

	public void Process(AccountLogOutPacket P)
	{
		LoggedIn = false;
		AccountName = string.Empty;
		SendPacket(new AccountLogOutSuccessPacket
		{
			ErrorMessage = Encoding.UTF8.GetBytes("Successfully logged out")
		});
	}

	public void Process(AccountChangePasswordPacket P)
	{
		string[] array = Encoding.UTF8.GetString(P.AccountInformation).Split('/');
		if (array.Length == 4)
		{
			if (array[1].Length <= 1 || array[1].Length > 18)
			{
				SendPacket(new AccountChangePasswordFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The password length is incorrect")
				});
				return;
			}
			if (!SMain.Accounts.TryGetValue(array[0], out var value))
			{
				SendPacket(new AccountChangePasswordFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The account does not exist")
				});
				return;
			}
			if (array[2] != value.SecurityQuestion)
			{
				SendPacket(new AccountChangePasswordFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("Password issue is incorrect")
				});
				return;
			}
			if (array[3] != value.SecurityAnswer)
			{
				SendPacket(new AccountChangePasswordFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The secret answer is incorrect")
				});
				return;
			}
			value.Password = array[1];
			SMain.SaveAccount(value);
			SendPacket(new AccountChangePasswordSuccessPacket());
			SMain.AddLogMessage("Password change successful! Account: " + array[1]);
		}
	}

	public void Process(AccountRegisterPacket P)
	{
        string[] array = Encoding.UTF8.GetString(P.RegistrationInformation).Split('/');
		if (array.Length == 5)
		{
			if (array[0].Length <= 5 || array[0].Length > 12)
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The username length is incorrect")
				});
				return;
			}
			if (array[1].Length <= 5 || array[1].Length > 18)
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The password length is incorrect")
				});
				return;
			}
			if (array[2].Length <= 1 || array[2].Length > 18)
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The security issue length is incorrect")
				});
				return;
			}
			if (array[3].Length <= 1 || array[3].Length > 18)
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The secret answer length is incorrect")
				});
				return;
			}
			if (array[4].Length > 4 || (array[4] != string.Empty && !SMain.AccountRefferalCodes.ContainsKey(array[4])))
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("If the promotion code is wrong, you can register directly without filling it in")
				});
				return;
			}
			if (!Regex.IsMatch(array[0], "^[a-zA-Z]+.*$"))
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The username is formatted incorrectly")
				});
				return;
			}
			if (!Regex.IsMatch(array[0], "^[a-zA-Z_][A-Za-z0-9_]*$"))
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The username is formatted incorrectly")
				});
				return;
			}
			if (SMain.Accounts.ContainsKey(array[0]))
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The username already exists")
				});
				return;
			}
			SMain.AddAccount(new AccountInfo(array[0], array[1], array[2], array[3], array[4]));
			SendPacket(new AccountRegisterSuccessPacket());
			SMain.AddLogMessage("Account registration is successful! Account: " + array[2]);
			SMain.CreatedAccounts++;
		}
	}

	public void Process(下载游戏文件 P)
	{
		SMain.AddLogMessage(P.ToString());
	}

	public void Process(AccountDownloadConfigFilePacket P)
	{
		if (P.文件序号 < SMain.PatchChunks)
		{
			SendPacket(new AccountSendConfigFilePacket
			{
				FileInformation = SMain.PatchData.Skip(P.文件序号 * 40960).Take(40960).ToArray()
			});
		}
		else
		{
			SendPacket(new AccountConfigFileCompletePacket());
		}
	}

	public void Process(AccountLogInPacket P)
	{
		string[] array = Encoding.UTF8.GetString(P.LoginInformation).Split('/');
		if (array.Length == 2)
		{
			if (!SMain.Accounts.TryGetValue(array[0], out var account) || array[1] != account.Password)
			{
				SendPacket(new AccountLogInFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("Wrong username or password")
				});
				return;
			}
            LoggedIn = true;
			AccountName = array[0];
			SendPacket(new AccountLogInSuccessPacket
			{
				ServerListInformation = Encoding.UTF8.GetBytes(SMain.PublicServerInfo)
			});
			SMain.AddLogMessage("Account login successful! Account: " + array[0]);
		}
	}

	public void Process(AccountSMSVerificationRequestPacket P)
	{
		var number = P.MobilePhoneNumber;
		if (number == null || number.Length != 11 || !Regex.IsMatch(number, "^[1]+[3,4,5,7,8]+\\d{9}"))
			return;

		if (!SMain.PhoneCaptchaTime.ContainsKey(number))
			SMain.PhoneCaptchaTime.Add(number, DateTime.Now.AddMinutes(1.0));

		if (!(SMain.PhoneCaptchaTime[number] > DateTime.Now))
		{
			if (SMain.PhoneCaptcha.ContainsKey(number))
				SMain.PhoneCaptcha.Remove(number);

			string code = SMain.CreateVerificationCode();
			SMain.PhoneCaptcha.Add(number, code);
			AccountSendSMSVerificationCodePacket(number, code);
		}
	}

	public void AccountSendSMSVerificationCodePacket(string phone, string code)
	{
		Task.Run(delegate
		{
			var data = SMS.Send(phone, code);
            SendPacket(new AccountRegisterFailPacket
			{
				ErrorMessage = Encoding.UTF8.GetBytes(data)
			});
		});
	}

	public void Process(请求更新文件 P)
	{
		string[] array = Encoding.UTF8.GetString(P.UpdateInformation).Split('/');
		string value = array[1];

        if (!ulong.TryParse(value, out var number))
		{
            SendPacket(new 程序提示信息
            {
                HintCode = -1
            });
            return;
        }

		if (number != SMain.PatchChecksum)
		{
			SendPacket(new 开始更新文件
			{
				文件编号 = 1,
				文件数量 = SMain.PatchChunks,
				校验代码 = SMain.PatchChecksum
			});
		}
		else
		{
			SendPacket(new 程序提示信息
			{
				HintCode = 0
			});
		}
	}

	public void Process(AccountStartGamePacket P)
	{
		string[] array = Encoding.UTF8.GetString(P.LoginInformation).Split('/');
		if (LoggedIn && array.Length == 2)
		{
			string svname = array[1];
			if (!SMain.ServerTable.TryGetValue(svname, out var server))
			{
				SendPacket(new AccountStartGameFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("No server found")
				});
				return;
			}

			string ticket = AccountInfo.GenerateTicket();
			if (SMain.Accounts.TryGetValue(AccountName, out var account))
			{
				SEngine.SendTicketToServer(server.InternalAddress, ticket, AccountName, account.PromoCode, account.ReferrerCode);

				SendPacket(new AccountStartGameSuccessPacket
				{
					Ticket = Encoding.UTF8.GetBytes(ticket)
				});

				SMain.AddLogMessage("Ticket has been generated! Account: " + AccountName + " - " + ticket);
			}
		}
		else
		{
			SendPacket(new AccountStartGameFailPacket
			{
				ErrorMessage = Encoding.UTF8.GetBytes("Not logged in yet!")
			});
		}
	}
}
