using System;
using System.Collections.Generic;
using System.IO;

namespace AccountServer;

public static class SAccounts
{
    public static string AccountDirectory = ".\\Accounts";
    public static Dictionary<string, AccountInfo> Accounts = new Dictionary<string, AccountInfo>();
    public static Dictionary<string, AccountInfo> AccountRefferalCodes = new Dictionary<string, AccountInfo>();


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


    public static int AccountCount => Accounts.Count;
    public static int CreatedAccounts;

    public static string CreatePromoCode()
    {
        string code;
        do
        {
            code = "";
            for (int i = 0; i < 4; i++)
            {
                code += RandomChars[Random.Shared.Next(RandomChars.Length)];
            }
        }
        while (AccountRefferalCodes.ContainsKey(code));
        return code;
    }

    public static string GenerateTicket()
    {
        var ticket = "ULS21-";
        for (int i = 0; i < 32; i++)
            ticket += RandomChars[Random.Shared.Next(RandomChars.Length)];
        return ticket;
    }

    public static bool LoadAccounts()
    {
        Accounts.Clear();
        AccountRefferalCodes.Clear();

        if (!Directory.Exists(AccountDirectory))
            return false;

        var array = Serializer.Deserialize<AccountInfo>(AccountDirectory);
        foreach (var account in array)
        {
            if (account.PromoCode == null || account.PromoCode == string.Empty)
            {
                account.PromoCode = CreatePromoCode();
                SaveAccount(account);
            }
            Accounts[account.AccountName] = account;
            AccountRefferalCodes[account.PromoCode] = account;
        }

        return true;
    }

    public static void AddAccount(AccountInfo account)
    {
        if (Accounts.ContainsKey(account.AccountName))
            return;

        account.PromoCode = CreatePromoCode();

        AccountRefferalCodes[account.PromoCode] = account;
        Accounts[account.AccountName] = account;

        SaveAccount(account);

        if (!string.IsNullOrEmpty(account.ReferrerCode))
        {
            if (AccountRefferalCodes.TryGetValue(account.ReferrerCode, out var referrer))
            {
                referrer.Referrerals.Add(account.AccountName);
                SaveAccount(referrer);
            }
        }

        CreatedAccounts++;
    }

    public static void SaveAccount(AccountInfo account)
    {
        File.WriteAllText(AccountDirectory + "\\" + account.AccountName + ".txt", Serializer.Serialize(account));
    }

    public static AccountInfo Find(string name) => Accounts.TryGetValue(name, out var account) ? account : null;

    public static bool AccountExists(string name) => Accounts.ContainsKey(name);

    public static bool ReferrerExists(string code) => AccountRefferalCodes.ContainsKey(code);
}
