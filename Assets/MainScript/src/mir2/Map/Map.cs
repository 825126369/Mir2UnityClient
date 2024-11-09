using Client.MirObjects;
using System.IO;
using UnityEngine;

public class MapJsonClass
{
    public int Width, Height;
    public OkCell[,] Cells;
}

public class OkCell
{
    public CellAttribute Attribute;
    public sbyte FishingAttribute = -1;
}

public class MapData
{
    public string FileName;
    public int Width, Height;
    public CellInfo[,] MapCells;
}

public class Map : MonoBehaviour
{
    public int Width, Height;
    public CellInfo[,] MapCells;
    public string FileName;
    
    public MapCellItem_Front mCellItemFrontPrefab;
    public MapCellItem_Middle mCellItemMiddlePrefab;
    public MapCellItem_Back mCellItemBackPrefab;

    public const int CellWidth = 48;
    public const int CellHeight = 32;

    public static int OffSetX;
    public static int OffSetY;

    public static int ViewRangeX;
    public static int ViewRangeY;

    private bool bInit = false;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (bInit) return;
        bInit = true;

        OffSetX = Screen.width / 2 / CellWidth;
        OffSetY = Screen.height / 2 / CellHeight - 1;
        ViewRangeX = OffSetX + 6;
        ViewRangeY = OffSetY + 6;

        LoadMap();
    }

    public void LoadMap()
    {
        MapReader Map = new MapReader(FileName);
        this.MapCells = Map.MapCells;
        this.Width = Map.Width;
        this.Height = Map.Height;

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
