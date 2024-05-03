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
			var name = array[0];
			var password = array[1];
			var question = array[2];
			var answer = array[3];

			if (password.Length <= 1 || password.Length > 18)
			{
				SendPacket(new AccountChangePasswordFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The password length is incorrect")
				});
				return;
			}

			var account = SAccounts.Find(name);
			if (account == null)
			{
				SendPacket(new AccountChangePasswordFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The account does not exist")
				});
				return;
			}
			if (question != account.SecurityQuestion)
			{
				SendPacket(new AccountChangePasswordFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("Password issue is incorrect")
				});
				return;
			}
			if (answer != account.SecurityAnswer)
			{
				SendPacket(new AccountChangePasswordFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The secret answer is incorrect")
				});
				return;
			}
			account.Password = password;
			SAccounts.SaveAccount(account);
			SendPacket(new AccountChangePasswordSuccessPacket());
			SMain.AddLogMessage("Password change successful! Account: " + name);
		}
	}

	public void Process(AccountRegisterPacket P)
	{
		string[] array = Encoding.UTF8.GetString(P.RegistrationInformation).Split('/');
		if (array.Length == 5)
		{
			string name, password, question, answer, referrerCode;

			name = array[0];
			password = array[1];
			question = array[2];
			answer = array[3];
			referrerCode = array[4];

			if (name.Length <= 5 || name.Length > 12)
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The username length is incorrect")
				});
				return;
			}
			if (password.Length <= 5 || password.Length > 18)
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The password length is incorrect")
				});
				return;
			}
			if (question.Length <= 1 || question.Length > 18)
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The security question length is incorrect")
				});
				return;
			}
			if (answer.Length <= 1 || answer.Length > 18)
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The secret answer length is incorrect")
				});
				return;
			}
			if (!string.IsNullOrEmpty(referrerCode) && (referrerCode.Length > 4 || !SAccounts.ReferrerExists(referrerCode)))
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("If the promotion code is wrong, you can register directly without filling it in")
				});
				return;
			}
			if (!Regex.IsMatch(name, "^[a-zA-Z]+.*$"))
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The username is formatted incorrectly")
				});
				return;
			}
			if (!Regex.IsMatch(name, "^[a-zA-Z_][A-Za-z0-9_]*$"))
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The username is formatted incorrectly")
				});
				return;
			}
			if (SAccounts.AccountExists(name))
			{
				SendPacket(new AccountRegisterFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("The username already exists")
				});
				return;
			}
			SAccounts.AddAccount(new AccountInfo(name, password, question, answer, referrerCode));
			SendPacket(new AccountRegisterSuccessPacket());
			SMain.AddLogMessage("Account registration is successful! Account: " + name);
		}
	}

	public void Process(下载游戏文件 P)
	{
		SMain.AddLogMessage(P.ToString());
	}

	public void Process(AccountDownloadConfigFilePacket P)
	{
		if (P.DocumentIndex < SMain.PatchChunks)
		{
			SendPacket(new AccountSendConfigFilePacket
			{
				FileInformation = SMain.PatchData.Skip(P.DocumentIndex * 40960).Take(40960).ToArray()
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
			var name = array[0];
			var password = array[1];

			var account = SAccounts.Find(name);
			if (account == null || password != account.Password)
			{
				SendPacket(new AccountLogInFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("Wrong username or password")
				});
				return;
			}
			LoggedIn = true;
			AccountName = name;
			SendPacket(new AccountLogInSuccessPacket
			{
				ServerListInformation = Encoding.UTF8.GetBytes(SMain.PublicServerInfo)
			});
			SMain.AddLogMessage("Account login successful! Account: " + name);
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

	public void Process(AccountRequestUpdateInfoPacket P)
	{
		string[] array = Encoding.UTF8.GetString(P.UpdateInformation).Split('/');
		string value = array[1];

		if (!ulong.TryParse(value, out var number))
		{
			SendPacket(new AccountSendUpdateInfoFailPacket
			{
				HintCode = -1
			});
			return;
		}

		if (number != SMain.PatchChecksum)
		{
			SendPacket(new AccountSendUpdateInfoSuccessPacket
			{
				DocumentID = 1,
				DocumentCount = SMain.PatchChunks,
				DocumentChecksum = SMain.PatchChecksum
			});
		}
		else
		{
			SendPacket(new AccountSendUpdateInfoFailPacket
			{
				HintCode = 0
			});
		}
	}

	public void Process(AccountStartGamePacket P)
	{
		var array = Encoding.UTF8.GetString(P.LoginInformation).Split('/');
		if (LoggedIn && array.Length == 2)
		{
			var svname = array[1];
			var server = SMain.ServerList.Find(x => x.ServerName == svname);

			if (server == null)
			{
				SendPacket(new AccountStartGameFailPacket
				{
					ErrorMessage = Encoding.UTF8.GetBytes("No server found")
				});
				return;
			}

			string ticket = SAccounts.GenerateTicket();
			var account = SAccounts.Find(AccountName);
			if (account != null)
			{
				SEngine.SendTicketToServer(server.TicketAddress, ticket, AccountName, account.PromoCode, account.ReferrerCode);

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
