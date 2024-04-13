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
        /*return new byte[761]
        {
            51, 53, 50, 54, 51, 53, 49, 57, 56, 48,
            0, 56, 48, 0, 58, 50, 53, 58, 97, 97,
            58, 51, 102, 58, 50, 50, 58, 48, 54, 0,
            85, 76, 83, 50, 49, 45, 53, 53, 51, 50,
            101, 48, 54, 57, 50, 100, 50, 51, 52, 98,
            98, 48, 57, 97, 56, 98, 99, 50, 100, 102,
            100, 102, 57, 97, 51, 1, 129, 110, 6, 0,
            0, 7, 0, 0, 0, 50, 48, 49, 54, 45,
            48, 50, 45, 50, 54, 0, 120, 156, 237, 86,
            205, 106, 20, 65, 16, 238, 237, 249, 91, 247,
            103, 102, 93, 163, 65, 34, 26, 140, 108, 196,
            104, 80, 148, 8, 122, 16, 37, 228, 224, 205,
            120, 86, 60, 120, 243, 45, 4, 197, 231, 240,
            226, 197, 103, 208, 131, 111, 226, 197, 139, 55,
            31, 32, 86, 237, 124, 159, 243, 109, 147, 68,
            68, 49, 151, 45, 40, 170, 187, 186, 254, 187,
            171, 102, 66, 8, 161, 31, 142, 135, 218, 240,
            0, 240, 81, 214, 67, 91, 95, 150, 61, 161,
            204, 66, 216, 239, 133, 80, 226, 204, 237, 151,
            135, 200, 57, 20, 71, 156, 57, 239, 180, 97,
            52, 92, 53, 28, 32, 206, 6, 242, 35, 224,
            24, 114, 126, 62, 193, 126, 12, 217, 10, 120,
            23, 57, 184, 252, 10, 124, 85, 160, 111, 160,
            119, 207, 240, 25, 120, 174, 239, 185, 93, 17,
            27, 142, 219, 240, 93, 193, 94, 9, 191, 99,
            145, 217, 130, 111, 231, 63, 55, 180, 82, 132,
            83, 134, 57, 228, 11, 156, 157, 151, 156, 47,
            192, 70, 1, 27, 37, 244, 106, 201, 91, 107,
            52, 16, 185, 50, 89, 159, 133, 142, 211, 115,
            208, 25, 128, 94, 52, 58, 133, 31, 234, 177,
            214, 121, 114, 7, 81, 116, 115, 196, 231, 245,
            219, 21, 89, 173, 177, 219, 248, 146, 117, 126,
            104, 231, 12, 214, 35, 212, 180, 2, 245, 252,
            54, 37, 134, 33, 248, 165, 32, 253, 215, 178,
            102, 205, 41, 83, 32, 158, 74, 98, 202, 32,
            83, 73, 28, 49, 182, 57, 100, 64, 198, 77,
            217, 245, 164, 190, 78, 27, 212, 139, 247, 224,
            188, 91, 114, 247, 172, 147, 159, 237, 136, 78,
            133, 152, 134, 18, 55, 245, 25, 63, 145, 239,
            66, 115, 90, 149, 245, 84, 98, 172, 69, 174,
            150, 115, 230, 89, 225, 60, 2, 157, 71, 253,
            153, 188, 43, 98, 41, 178, 190, 191, 36, 119,
            57, 0, 111, 2, 234, 50, 125, 201, 57, 38,
            186, 17, 185, 228, 216, 55, 178, 207, 37, 78,
            230, 50, 183, 83, 180, 247, 151, 37, 181, 103,
            158, 105, 205, 72, 153, 59, 243, 159, 98, 221,
            191, 218, 233, 233, 27, 218, 16, 59, 90, 131,
            70, 222, 2, 253, 191, 180, 245, 154, 225, 157,
            176, 216, 31, 140, 141, 241, 243, 110, 53, 246,
            90, 234, 78, 63, 122, 87, 15, 13, 175, 75,
            126, 148, 245, 253, 135, 216, 198, 51, 146, 248,
            247, 144, 219, 53, 121, 7, 236, 85, 222, 157,
            203, 46, 97, 9, 127, 3, 143, 194, 226, 140,
            243, 183, 117, 3, 123, 107, 169, 121, 47, 220,
            14, 109, 63, 124, 202, 91, 217, 173, 208, 245,
            196, 38, 214, 251, 88, 187, 254, 12, 168, 125,
            55, 16, 29, 246, 142, 246, 30, 103, 88, 249,
            27, 28, 29, 193, 143, 199, 240, 171, 100, 239,
            190, 222, 199, 174, 55, 235, 68, 231, 102, 104,
            231, 0, 99, 250, 22, 22, 103, 1, 229, 118,
            64, 31, 24, 110, 136, 109, 253, 46, 114, 102,
            232, 108, 162, 141, 89, 232, 230, 98, 145, 212,
            130, 125, 238, 223, 131, 87, 162, 231, 176, 22,
            186, 239, 140, 243, 125, 118, 12, 5, 57, 167,
            117, 54, 205, 103, 164, 212, 252, 215, 28, 234,
            181, 244, 93, 236, 102, 234, 16, 53, 105, 18,
            27, 7, 9, 232, 108, 36, 239, 179, 228, 249,
            53, 180, 255, 136, 220, 115, 190, 189, 77, 244,
            156, 238, 38, 246, 253, 126, 248, 237, 218, 179,
            24, 183, 109, 127, 127, 221, 214, 246, 32, 31,
            27, 239, 71, 111, 177, 38, 147, 164, 70, 90,
            103, 167, 223, 77, 127, 37, 107, 101, 158, 134,
            37, 44, 97, 9, 255, 3, 202, 19, 242, 235,
            115, 237, 245, 9, 249, 254, 151, 192, 185, 235,
            117, 124, 98, 243, 235, 69, 56, 252, 187, 231,
            115, 50, 251, 3, 187, 63, 1, 233, 74, 233,
            158
        };*/

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
