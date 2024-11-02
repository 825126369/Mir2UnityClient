using Client.MirObjects;
using System.IO;
using UnityEditor;

public class MapEditor
{
    const string OriDir = "Assets/CrystalMir2/Map/";

    [MenuItem("Mir2Editor/Ω‚√‹ Map ◊ ‘¥")]
    static void Do()
    {
        foreach (var v in Directory.GetFiles(OriDir, "*.map", SearchOption.TopDirectoryOnly))
        {
            LoadMap(v);
        }
    }

    static void LoadMap(string FileName)
    {
        MapReader Map = new MapReader(FileName);
        MapData mMapData = new MapData();
        mMapData.MapCells = Map.MapCells;
        mMapData.Width = Map.Width;
        mMapData.Height = Map.Height;
        mMapData.FileName = Path.GetFileNameWithoutExtension(FileName);

        string data = JsonTool.ToJson(mMapData);
        string path = $"Assets/CrystalMir2/Map2/{Path.GetFileNameWithoutExtension(FileName)}.json";
        File.WriteAllText(path, data);
    }

}