using System;
using System.Collections.Generic;

namespace AccountServer;

public sealed class AccountInfo
{
	public string AccountName;
	public string Password;
	public string SecurityQuestion;
	public string SecurityAnswer;
	public DateTime CreationDate;
	public string PromoCode;
	public string ReferrerCode;
	public List<string> Referrerals = new List<string>();


	public AccountInfo(string name, string password, string question, string answer, string referrerCode)
	{
		AccountName = name;
		Password = password;
		SecurityQuestion = question;
		SecurityAnswer = answer;
		CreationDate = DateTime.UtcNow;
		ReferrerCode = referrerCode;
	}
}
