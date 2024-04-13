using System;
using System.IO;
using System.Text;

namespace GameServer.Database;

public sealed class MailInfo : DBObject
{
    public readonly DataMonitor<bool> SystemMail;
    public readonly DataMonitor<bool> Unread;

    public readonly DataMonitor<string> Subject;
    public readonly DataMonitor<string> Message;

    public readonly DataMonitor<DateTime> CreatedDate;
    public readonly DataMonitor<ItemInfo> Attachment;

    public readonly DataMonitor<CharacterInfo> Author;
    public readonly DataMonitor<CharacterInfo> Recipient;

    public int MailID => Index.V;
    public int MailTime => Compute.TimeSeconds(CreatedDate.V);

    public int Quantity
    {
        get
        {
            if (Attachment.V == null)
                return 0;
            if (Attachment.V.CanStack)
                return Attachment.V.Dura.V;
            return 1;
        }
    }

    public MailInfo()
    {
    }

    public MailInfo(CharacterInfo sender, string subject, string msg, ItemInfo attachment)
    {
        Author.V = sender;
        Subject.V = subject;
        Message.V = msg;
        Attachment.V = attachment;
        Unread.V = true;
        SystemMail.V = sender == null;
        CreatedDate.V = SEngine.CurrentTime;
        Session.MailInfoTable.Add(this, true);
    }

    public override string ToString()
    {
        return Subject?.V;
    }

    public override void Remove()
    {
        Attachment.V?.Remove();
        base.Remove();
    }

    public byte[] MailMessageDescription()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(MailID);
        writer.Write(0);
        writer.Write(MailTime);
        writer.Write(SystemMail.V);
        writer.Write(Unread.V);
        writer.Write(Attachment.V != null);
        byte[] array = new byte[32];
        if (!SystemMail.V)
        {
            Encoding.UTF8.GetBytes(Author.V.UserName.V + "\0").CopyTo(array, 0);
        }
        writer.Write(array);
        byte[] array2 = new byte[61];
        Encoding.UTF8.GetBytes(Subject.V + "\0").CopyTo(array2, 0);
        writer.Write(array2);
        return ms.ToArray();
    }

    public byte[] 邮件内容描述()
    {
        using var ms = new MemoryStream(new byte[672]);
        using var writer = new BinaryWriter(ms);
        writer.Write(0);
        writer.Write(MailTime);
        writer.Write(SystemMail.V);
        writer.Write(Attachment.V?.ID ?? (-1));
        writer.Write(Quantity);
        byte[] array = new byte[32];
        if (!SystemMail.V)
        {
            Encoding.UTF8.GetBytes(Author.V.UserName.V + "\0").CopyTo(array, 0);
        }
        writer.Write(array);
        byte[] array2 = new byte[61];
        Encoding.UTF8.GetBytes(Subject.V + "\0").CopyTo(array2, 0);
        writer.Write(array2);
        byte[] array3 = new byte[554];
        Encoding.UTF8.GetBytes(Message.V + "\0").CopyTo(array3, 0);
        writer.Write(array3);
        writer.Write(MailID);
        writer.Write(0);
        return ms.ToArray();
    }
}
