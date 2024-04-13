using System;
using System.Collections.Generic;

namespace AccountServer;

public sealed class AccountInfo
{
	private static char[] RandomChars = new char[62]
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
		'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
		'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
		'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
		'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
		'y', 'z'
	};

	public string AccountName;
	public string Password;
	public string SecurityQuestion;
	public string SecurityAnswer;
	public DateTime CreationDate;
	public string PromoCode;
	public string ReferrerCode;
	public List<string> Referrerals = new List<string>();

	public static string GenerateTicket()
	{
		var ticket = "ULS21-";
		for (int i = 0; i < 32; i++)
			ticket += RandomChars[Random.Shared.Next(RandomChars.Length)];
		return ticket;
	}

	public AccountInfo(string name, string password, string question, string answer, string referrerCode)
	{
		AccountName = name;
		Password = password;
		SecurityQuestion = question;
		SecurityAnswer = answer;
		CreationDate = DateTime.Now;
		ReferrerCode = referrerCode;

		if (referrerCode != null && SMain.AccountRefferalCodes.TryGetValue(referrerCode, out var value))
		{
			value.Referrerals.Add(name);
			SMain.SaveAccount(value);
		}
	}
}
