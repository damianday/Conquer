using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace GameServer.Template;

public sealed class Terrain
{
    public static Dictionary<byte, Terrain> DataSheet;

    public byte MapID;
    public string MapName;
    public Point StartPoint;
    public Point EndPoint;
    public Size MapSize;
    public Point MapHeight;
    public uint[,] Matrix;

    public uint this[Point point] => Matrix[point.X - StartPoint.X, point.Y - StartPoint.Y];

    private static Terrain LoadTerrainFromFile(FileSystemInfo fileInfo)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
        var parts = fileNameWithoutExtension.Split('-');

        var terrain = new Terrain();
        terrain.MapName = parts[1];
        terrain.MapID = Convert.ToByte(parts[0]);
  
        using (var ms = new MemoryStream(File.ReadAllBytes(fileInfo.FullName)))
        {
            using var reader = new BinaryReader(ms);
            terrain.StartPoint = new Point(reader.ReadInt32(), reader.ReadInt32());
            terrain.EndPoint = new Point(reader.ReadInt32(), reader.ReadInt32());
            terrain.MapSize = new Size(terrain.EndPoint.X - terrain.StartPoint.X, terrain.EndPoint.Y - terrain.StartPoint.Y);
            terrain.MapHeight = new Point(reader.ReadInt32(), reader.ReadInt32());
            terrain.Matrix = new uint[terrain.MapSize.Width, terrain.MapSize.Height];
            
            for (var x = 0; x < terrain.MapSize.Width; x++)
            {
                for (var y = 0; y < terrain.MapSize.Height; y++)
                {
                    terrain.Matrix[x, y] = reader.ReadUInt32();
                }
            }
        }
        return terrain;
    }

    public static void LoadData()
    {
        DataSheet = new Dictionary<byte, Terrain>();

        var path = Settings.GameDataPath + "\\System\\GameMap\\Terrains\\";
        if (!Directory.Exists(path))
            return;

        var terrains = new ConcurrentBag<Terrain>();
        var terrainFiles = new DirectoryInfo(path).GetFiles("*.terrain");

        Parallel.ForEach(terrainFiles, delegate (FileInfo x)
        {
            var terrain = LoadTerrainFromFile(x);
            terrains.Add(terrain);
        });

        foreach (var obj in terrains)
            DataSheet.Add(obj.MapID, obj);
    }
}
