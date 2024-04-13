using ClientPacketSniffer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GamePackets;
using GamePackets.Client;
using GamePackets.Server;

namespace ClientPacketSnifferApp
{
    public partial class FRawFile : Form
    {
        public struct PacketInfo
        {
            public DateTime Date;
            public bool PacketFromClient;
            public byte[] Data;
        }

        public class PacketParsed
        {
            public DateTime Date;
            public byte[] Data = Array.Empty<byte>();
            public GamePacketInfo PacketInfo;

            public override string ToString()
            {
                return $"{PacketInfo} - {{{(PacketInfo.Length == 0 ? string.Join(",", BitConverter.GetBytes((ushort)Data.Length).Select(x => x.ToString()).ToArray()) : "")}}} {{{string.Join(",", Data.Select(x => x.ToString()).ToArray())}}}";
            }
        }

        public class ParseGamePacketException : ApplicationException
        {
            public bool PacketFromclient { get; set; }
            public ushort PacketId { get; set; }
            public byte[] PacketRaw { get; set; }

            public ParseGamePacketException(bool packetFromClient, ushort id, byte[] raw) : base($"FALTAL!! PACKET ID NOT FOUND {id} IN SOURCE: {(packetFromClient ? 0 : 1)}")
            {
                PacketId = id;
                PacketFromclient = packetFromClient;
                PacketRaw = raw;
            }
        }

        public class ListBoxItem
        {
            public string Text { get; set; }
            public GamePacket Packet { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        public List<PacketInfo> m_RawPackets = new List<PacketInfo>();
        public List<GamePacket> m_AllPackets = new List<GamePacket>();

        public FRawFile()
        {
            InitializeComponent();
        }

        public void LoadBuffer(byte[] buffer)
        {
            m_RawPackets.Clear();
            m_AllPackets.Clear();

            LoadPackets(buffer);
            ParsePackets();

            foreach (var packet in m_AllPackets)
            {
                listBox1.Items.Add(new ListBoxItem { Text = $"Source: {(packet.Info.Source == PacketSource.Client ? "C" : "S")} ID: {packet.Info.ID}", Packet = packet });
            }
        }

        private void LoadPackets(byte[] buffer)
        {
            using (var br = new BinaryReader(new MemoryStream(buffer)))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    m_RawPackets.Add(new PacketInfo
                    {
                        Date = DateTime.FromFileTimeUtc(br.ReadInt64()),
                        PacketFromClient = br.ReadBoolean(),
                        Data = br.ReadBytes(br.ReadInt32())
                    });
                }
            }
        }

        public void ParsePackets()
        {
            /*var extraBuffers = new Dictionary<int, byte[]>() {
                { 0, Array.Empty<byte>() },
                { 1, Array.Empty<byte>() }
            };*/

            var rawClientData = Array.Empty<byte>();
            var rawServerData = Array.Empty<byte>();

            for (var i = 0; i < m_RawPackets.Count; i++)
            {
                var rawPacket = m_RawPackets[i];
                if (rawPacket.Data.Length == 0) continue;

                var src = rawPacket.Data;
                var dataRead = src.Length;

                if (rawPacket.PacketFromClient)
                {
                    byte[] dst = new byte[rawClientData.Length + dataRead];
                    Buffer.BlockCopy(rawClientData, 0, dst, 0, rawClientData.Length);
                    Buffer.BlockCopy(src, 0, dst, rawClientData.Length, dataRead);
                    rawClientData = dst;

                    while (true)
                    {
                        GamePacket packet = GamePacket.GetClientPacket(rawClientData, out rawClientData);
                        if (packet == null)
                            break;
                        m_AllPackets.Add(packet);
                    }
                }
                else
                {
                    byte[] dst = new byte[rawServerData.Length + dataRead];
                    Buffer.BlockCopy(rawServerData, 0, dst, 0, rawServerData.Length);
                    Buffer.BlockCopy(src, 0, dst, rawServerData.Length, dataRead);
                    rawServerData = dst;

                    while (true)
                    {
                        GamePacket packet = GamePacket.GetServerPacket(rawServerData, out rawServerData);
                        if (packet == null)
                            break;
                        m_AllPackets.Add(packet);
                    }
                }





                /*var extraBuffer = extraBuffers[rawPacket.PacketFromClient ? 0 : 1];
                extraBuffers[rawPacket.PacketFromClient ? 0 : 1] = Array.Empty<byte>();

                var fullBuffer = new byte[extraBuffer.Length + rawPacket.Data.Length];
                Array.Copy(extraBuffer, 0, fullBuffer, 0, extraBuffer.Length);
                Array.Copy(rawPacket.Data, 0, fullBuffer, extraBuffer.Length, rawPacket.Data.Length);

                var p = 0u;
                do
                {
                    var buffer = new byte[fullBuffer.Length - p];
                    Array.Copy(fullBuffer, p, buffer, 0, fullBuffer.Length - p);

                    if (buffer.Length == 1 && buffer[0] == 0)
                    {
                        p++;
                        continue;
                    }

                    var packetId = BitConverter.ToUInt16(buffer, 0);
                    if (packetId == 0) continue;

                    var packets = rawPacket.PacketFromClient ? GamePacketInfoRepository.Instance.ClientPackets : GamePacketInfoRepository.Instance.ServerPackets;

                    if (!packets.TryGetValue(packetId, out var packetType))
                        throw new ParseGamePacketException(rawPacket.PacketFromClient, packetId, buffer);

                    if (packetType.Length == 0 && buffer.Length < (packetType.UseIntSize ? 6 : 4))
                    {
                        extraBuffers[rawPacket.PacketFromClient ? 0 : 1] = buffer;
                        break;
                    }

                    var isMasked = packetId != 1002 && packetId != 1001;

                    var headerLength = packetType.Length == 0 ? (packetType.UseIntSize ? 6 : 4) : 2;

                    uint length = 0;

                    if (packetType.Length == 0 && packetType.UseIntSize)
                    {
                        var buff = new byte[4];
                        Array.Copy(buffer, 2, buff, 0, 4);
                        buff[2] ^= 129;
                        buff[3] ^= 129;
                        length = BitConverter.ToUInt32(buff) + 6;
                    }
                    else if (packetType.Length == 0 && !packetType.UseIntSize)
                    {
                        length = BitConverter.ToUInt16(buffer, 2);
                    }
                    else
                    {
                        length = packetType.Length;
                    }

                    if (length > buffer.Length)
                    {
                        extraBuffers[rawPacket.PacketFromClient ? 0 : 1] = buffer;
                        break;
                    }

                    byte[] data = Array.Empty<byte>();


                    if (length > headerLength)
                    {
                        data = new byte[length - headerLength];
                        Array.Copy(buffer, headerLength, data, 0, length - headerLength);

                        if (isMasked)
                            for (var b = packetType.Length == 0 ? 0 : 2; b < data.Length; b++)
                                data[b] ^= 129;
                    }

                    output.Add(new PacketParsed
                    {
                        Data = data,
                        Date = rawPacket.Date,
                        PacketInfo = packetType
                    });

                    p += length + (uint)headerLength;
                } while (p < fullBuffer.Length);*/
            }

            /*fullReaded = extraBuffers.All(x => x.Value.Length == 0);

            var sb = new StringBuilder();

            foreach (var packet in output)
            {
                sb.AppendLine($"// [{packet.Date.ToString("HH:mm:ss")}] Packet ID: {packet.PacketInfo.Id}, Name: {packet.PacketInfo.Name} ({(packet.PacketInfo.Source == 0 ? "Client" : "Server")})");
                sb.AppendLine($"网络连接.SendRaw({packet.PacketInfo.Id}, {packet.PacketInfo.Length}, new byte[] {{{string.Join(", ", packet.Data.Select(x => x.ToString()).ToArray())}}});");
            }

            var raw = sb.ToString();*/
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GamePacket packet = null;

            if (listBox1.SelectedIndex >= 0)
                packet = ((ListBoxItem)listBox1.SelectedItem).Packet;

            if (packet != null)
            {
                textBox1.Text = packet.ToString();
            }
        }
    }
}
