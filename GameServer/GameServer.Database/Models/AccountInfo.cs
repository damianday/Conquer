using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using GameServer.Map;
using GameServer.Networking;

using GamePackets;
using GamePackets.Client;
using GamePackets.Server;

namespace GameServer.Database;

[SearchAttribute(SearchName = "AccountName")]
public sealed class AccountInfo : DBObject
{
    public SConnection Connection;

    public readonly DataMonitor<string> AccountName;
    public readonly DataMonitor<DateTime> BlockDate;
    public readonly DataMonitor<DateTime> DeletetionDate;

    public readonly HashMonitor<CharacterInfo> Characters;
    public readonly HashMonitor<CharacterInfo> FrozenCharacters;
    public readonly HashMonitor<CharacterInfo> DeletedCharacters;

    public AccountInfo()
    {
    }

    public AccountInfo(string name)
    {
        AccountName.V = name;
        Session.AccountInfoTable.Add(this, true);
    }

    public void Enqueue(GamePacket p)
    {
        if (Connection == null) return;
        Connection.SendPacket(p);
    }

    public override string ToString()
    {
        return AccountName?.V;
    }

    public byte[] 角色列表描述()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        var count = Math.Min(4, Characters.Count) + Math.Min(5, FrozenCharacters.Count);

        writer.Write((byte)count);
        List<CharacterInfo> list = Characters.OrderByDescending(x => x.Level.V).ToList();
        for (var i = 0; i < 4 && i < list.Count; i++)
            writer.Write(list[i].RoleDescription());

        List<CharacterInfo> list2 = FrozenCharacters.OrderByDescending(x => x.Level.V).ToList();
        for (var i = 0; i < 5 && i < list2.Count; i++)
            writer.Write(list2[i].RoleDescription());

        return ms.ToArray();
    }

    public byte[] 登录协议描述()
    {
        return new byte[]
        {
            51, 52, 55, 57, 56, 49, 48, 51, 48, 50, 0, 48, 50, 0, 126, 
            126, 129, 129, 145, 166, 224, 100, 102, 58, 55, 53, 58, 49, 97, 0, 
            85, 76, 83, 50, 49, 45, 99, 49, 48, 55, 100, 50, 52, 51, 55, 
            54, 48, 48, 52, 97, 98, 100, 57, 98, 48, 52, 53, 98, 53, 98, 
            53, 57, 51, 52, 101, 1, 129, 110, 94, 0, 0, 7, 0, 0, 0, 
            49, 57, 55, 48, 45, 48, 49, 45, 48, 49, 0, 120, 156, 237, 86, 
            189, 110, 19, 65, 16, 94, 159, 239, 199, 220, 217, 119, 198, 4, 34, 
            8, 130, 40, 65, 14, 34, 16, 129, 64, 65, 130, 2, 129, 34, 10, 
            58, 76, 13, 162, 224, 73, 144, 64, 84, 60, 4, 13, 45, 79, 0, 
            5, 37, 111, 65, 67, 67, 199, 3, 132, 25, 223, 247, 113, 159, 23, 
            18, 132, 20, 41, 5, 30, 105, 180, 187, 179, 243, 247, 205, 238, 206, 
            93, 8, 33, 12, 194, 225, 84, 27, 239, 131, 62, 200, 188, 178, 249, 
            134, 172, 73, 121, 63, 132, 89, 47, 132, 28, 123, 238, 63, 255, 131, 
            158, 83, 118, 192, 158, 203, 78, 26, 39, 198, 171, 198, 37, 242, 108, 
            160, 63, 4, 143, 160, 231, 251, 99, 172, 71, 208, 45, 192, 183, 129, 
            193, 245, 87, 16, 171, 192, 248, 10, 118, 119, 140, 159, 66, 230, 246, 
            142, 237, 146, 248, 112, 222, 65, 236, 2, 254, 114, 196, 29, 137, 206, 
            54, 98, 187, 252, 153, 177, 149, 34, 156, 48, 78, 161, 159, 97, 239, 
            172, 96, 62, 15, 31, 25, 124, 228, 176, 171, 5, 183, 214, 168, 20, 
            189, 60, 154, 159, 134, 141, 143, 103, 96, 83, 98, 188, 96, 227, 4, 
            113, 104, 199, 90, 167, 209, 25, 36, 98, 155, 34, 63, 175, 223, 158, 
            232, 106, 141, 221, 199, 231, 126, 23, 135, 126, 78, 97, 62, 68, 77, 
            11, 140, 142, 111, 75, 114, 168, 32, 103, 77, 115, 137, 63, 145, 185, 
            238, 179, 158, 169, 248, 25, 192, 119, 41, 178, 57, 158, 164, 197, 208, 
            7, 51, 111, 234, 174, 71, 245, 245, 177, 65, 108, 158, 131, 203, 110, 
            200, 217, 179, 78, 190, 183, 43, 54, 5, 114, 170, 36, 111, 218, 51, 
            127, 50, 239, 133, 98, 90, 149, 249, 68, 114, 172, 69, 175, 150, 125, 
            226, 44, 176, 159, 128, 93, 70, 251, 169, 220, 43, 114, 46, 186, 190, 
            190, 40, 103, 89, 66, 54, 198, 232, 58, 3, 193, 156, 68, 182, 9, 
            176, 164, 88, 55, 178, 78, 37, 79, 98, 153, 251, 201, 218, 243, 235, 
            71, 181, 39, 206, 184, 102, 28, 137, 157, 248, 39, 152, 15, 46, 119, 
            118, 220, 119, 251, 77, 241, 163, 53, 104, 228, 46, 48, 254, 11, 155, 
            175, 25, 223, 10, 139, 239, 67, 239, 35, 49, 240, 124, 53, 255, 90, 
            106, 207, 88, 122, 94, 247, 141, 175, 10, 70, 234, 250, 250, 125, 210, 
            230, 52, 20, 12, 51, 224, 187, 34, 119, 129, 239, 85, 207, 47, 190, 
            87, 46, 219, 8, 139, 249, 191, 197, 121, 234, 251, 212, 254, 177, 164, 
            255, 147, 30, 132, 197, 30, 231, 119, 225, 26, 214, 246, 164, 230, 111, 
            225, 102, 104, 239, 201, 199, 180, 213, 221, 14, 139, 61, 121, 11, 235, 
            25, 230, 46, 155, 130, 245, 14, 150, 98, 199, 183, 163, 239, 143, 125, 
            44, 255, 11, 15, 15, 144, 39, 135, 200, 139, 104, 237, 177, 222, 37, 
            221, 219, 172, 35, 155, 235, 161, 237, 5, 204, 233, 155, 96, 81, 76, 
            187, 24, 239, 25, 111, 138, 111, 253, 54, 178, 103, 104, 127, 162, 143, 
            105, 232, 122, 99, 22, 213, 130, 239, 220, 191, 9, 95, 196, 206, 105, 
            45, 116, 223, 26, 151, 123, 239, 168, 132, 217, 171, 181, 55, 205, 251, 
            164, 212, 252, 87, 31, 234, 181, 227, 155, 164, 235, 171, 21, 106, 210, 
            68, 62, 246, 35, 210, 222, 72, 217, 39, 193, 249, 53, 180, 189, 136, 
            107, 246, 183, 215, 145, 157, 143, 123, 145, 127, 63, 31, 126, 191, 30, 
            90, 142, 59, 182, 190, 187, 110, 115, 187, 148, 143, 76, 246, 163, 183, 
            88, 147, 113, 84, 35, 173, 51, 101, 43, 230, 240, 59, 112, 62, 49, 
            62, 135, 189, 176, 164, 37, 45, 233, 200, 233, 184, 254, 45, 188, 167, 
            189, 60, 166, 216, 71, 73, 236, 185, 94, 199, 199, 214, 187, 158, 135, 
            223, 191, 109, 174, 227, 61, 178, 255, 15, 126, 127, 2, 3, 35, 255, 
            75
        };
    }

    public void Disconnect()
    {
        Connection.Account = null;
        Connection = null;
        NetworkManager.ActiveConnections--;
    }

    public void AccountLogin(SConnection conn, string addr)
    {
        conn.Account = this;
        conn.Stage = GameStage.SelectPlayer;
        Connection = conn;
        Connection.MACAddress = addr;
        NetworkManager.ActiveConnections++;

        Enqueue(new LoginSuccessPacket
        {
            协议数据 = 登录协议描述()
        });
        Enqueue(new 同步服务状态());
        Enqueue(new 行会最大成员
        {
            U1 = 50,
            U2 = 60,
            U3 = 70,
            U4 = 80,
            U5 = 90,
            U6 = 100,
            U7 = 100
        });
        Enqueue(new 附加组队设置
        {
            整数变量 = 1
        });
        Enqueue(new 返回角色列表
        {
            Description = 角色列表描述()
        });
    }

    public void 返回登录(SConnection conn)
    {
        conn.Disconnect(new Exception("客户端返回登录."));
    }

    public void NewCharacter(SConnection conn, 客户创建角色 P)
    {
        if (Session.CharacterInfoTable.DataSheet.Count >= 1_000_000)
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 304 });
            return;
        }
        if (Characters.Count >= 4)
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 267 });
            return;
        }
        if (Encoding.UTF8.GetBytes(P.Name).Length > 24)
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 270 });
            return;
        }
        if (Session.CharacterInfoTable[P.Name] != null)
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 272 });
            return;
        }
        if (!Enum.TryParse<GameObjectRace>(P.Job.ToString(), out var race) || !Enum.IsDefined(typeof(GameObjectRace), race))
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 258 });
            return;
        }
        if (!Enum.TryParse<GameObjectGender>(P.Gender.ToString(), out var gender) || !Enum.IsDefined(typeof(GameObjectGender), gender))
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 258 });
            return;
        }
        if (!Enum.TryParse<ObjectHairColor>(P.HairColor.ToString(), out var hairColor) || !Enum.IsDefined(typeof(ObjectHairColor), hairColor))
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 258 });
            return;
        }
        if (!Enum.TryParse<ObjectHairStyle>((P.Job * 65536 + P.Gender * 256 + P.HairStyle).ToString(), out var hairStyle) || !Enum.IsDefined(typeof(ObjectHairStyle), hairStyle))
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 258 });
            return;
        }
        if (!Enum.TryParse<ObjectFaceStyle>((P.Job * 65536 + P.Gender * 256 + P.FaceStyle).ToString(), out var faceStyle) || !Enum.IsDefined(typeof(ObjectFaceStyle), faceStyle))
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 258 });
            return;
        }

        Enqueue(new 角色创建成功
        {
            Description = new CharacterInfo(this, P.Name, race, gender, hairStyle, hairColor, faceStyle).RoleDescription()
        });
    }

    public void FreezeCharacter(SConnection conn, 客户删除角色 P)
    {
        if (Session.CharacterInfoTable.DataSheet.TryGetValue(P.CharacterID, out var value) && value is CharacterInfo character && Characters.Contains(character))
        {
            if (character.Guild.V != null)
            {
                Enqueue(new LoginErrorMessagePacket { ErrorCode = 280 });
                return;
            }
            if (character.Mentor.V != null && (character.Mentor.V.StudentsInfo.Contains(character) || character.Mentor.V.StudentsInfo.Count != 0))
            {
                Enqueue(new LoginErrorMessagePacket { ErrorCode = 280 });
                return;
            }
            if (FrozenCharacters.Count >= 5)
            {
                conn.Disconnect(new Exception("删除角色时找回列表已满, 断开连接."));
                return;
            }

            character.FrozenDate.V = SEngine.CurrentTime;
            Characters.Remove(character);
            FrozenCharacters.Add(character);
            Enqueue(new 删除角色应答 { CharacterID = character.ID });
        }
        else
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 277 });
        }
    }

    public void DeleteCharacter(SConnection conn, 彻底删除角色 P)
    {
        if (Session.CharacterInfoTable.DataSheet.TryGetValue(P.CharacterID, out var value) && value is CharacterInfo character && FrozenCharacters.Contains(character))
        {
            if (character.CurrentLevel >= 40)
            {
                Enqueue(new LoginErrorMessagePacket { ErrorCode = 291 });
                return;
            }
            if (DeletetionDate.V.Date == SEngine.CurrentTime.Date)
            {
                Enqueue(new LoginErrorMessagePacket { ErrorCode = 282 });
                return;
            }

            var tm = SEngine.CurrentTime;
            character.DeletetionDate.V = tm;
            DeletetionDate.V = tm;
            FrozenCharacters.Remove(character);
            DeletedCharacters.Add(character);
            Enqueue(new 永久删除角色
            {
                CharacterID = character.ID
            });
        }
        else
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 277 });
        }
    }

    public void RestoreCharacter(SConnection conn, 客户找回角色 P)
    {
        if (Session.CharacterInfoTable.DataSheet.TryGetValue(P.CharacterID, out var value) && value is CharacterInfo character && FrozenCharacters.Contains(character))
        {
            if (Characters.Count >= 4)
            {
                conn.Disconnect(new Exception("找回角色时角色列表已满, 断开连接."));
                return;
            }
            character.FrozenDate.V = default(DateTime);
            FrozenCharacters.Remove(character);
            Characters.Add(character);
            Enqueue(new 找回角色应答
            {
                CharacterID = character.ID
            });
        }
        else
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 277 });
        }
    }

    public void EnterGame(SConnection conn, 客户进入游戏 P)
    {
        if (Session.CharacterInfoTable.DataSheet.TryGetValue(P.CharacterID, out var value) && value is CharacterInfo character && Characters.Contains(character))
        {
            if (SEngine.CurrentTime < BlockDate.V)
            {
                Enqueue(new LoginErrorMessagePacket
                {
                    ErrorCode = 285,
                    Param1 = Compute.TimeSeconds(BlockDate.V)
                });
            }
            else if (SEngine.CurrentTime < character.BlockDate.V)
            {
                Enqueue(new LoginErrorMessagePacket
                {
                    ErrorCode = 285,
                    Param1 = Compute.TimeSeconds(character.BlockDate.V)
                });
            }
            else
            {
                Enqueue(new StartGamePacket
                {
                    CharacterID = character.ID
                });
                conn.Player = new PlayerObject(character, conn);
                conn.Stage = GameStage.Loading;
            }
        }
        else
        {
            Enqueue(new LoginErrorMessagePacket { ErrorCode = 284 });
        }
    }

    public void ChangeCharacter(SConnection conn)
    {
        Enqueue(new 更换角色计时
        {
            成功 = true
        });
        Enqueue(new 更换角色应答());
        Enqueue(new ObjectDisappearPacket
        {
            ObjectID = conn.Player.ObjectID
        });
        conn.Player.Disconnect();
        Enqueue(new 返回角色列表
        {
            Description = 角色列表描述()
        });
    }
}
