using Client.MirObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapMgr : SingleTonMonoBehaviour<TileMapMgr>
{
    public const int CellWidth = 48;
    public const int CellHeight = 32;

    public Tilemap Map_Back;
    public Tilemap Map_Middle; 
    public Tilemap Map_Front; 
    public int mapWidth = 1000; // 地图的宽度
    public int mapHeight = 1000; // 地图的高度
    private MapReader mMapData;

    readonly ScriptableObjectPool<Tile> mTilePool = new ScriptableObjectPool<Tile>();
    private bool bInit =false;
    private Vector3Int lastCenter;
    readonly Vector3Int Range = new Vector3Int(10, 10, 0);
    private void Start()
    {
        Init();
        LoadMap();
    }

    private void Init()
    {
        if (bInit) return;
        bInit = true;
        mTilePool.Init(1000);
    }

    public void LoadMap(string fileName = "0")
    {
        string path = $"D:/Me/MyProject/CrystalMir2/Client8/Map/" + fileName + ".map";
        mMapData = new MapReader(path);
        UpdateMap();
    }

    public void UpdateMap()
    {
        Vector3Int center = Mir2Me.Instance.MapLocation;
        ClearTile(center);
        StartCoroutine(DrawMapBack(center, Range));
        StartCoroutine(DrawMapMiddle(center, Range));
        StartCoroutine(DrawMapFront(center, Range));
        lastCenter = center;
    }

    private void ClearTile(Vector3Int center)
    {
        int nMinX = center.x - Range.x;
        int nMaxX = center.x + Range.x;
        int nMinY = center.y - Range.y;
        int nMaxY = center.y + Range.y;

        int nLastMinX = lastCenter.x - Range.x;
        int nLastMaxX = lastCenter.x + Range.x;
        int nLastMinY = lastCenter.y - Range.y;
        int nLastMaxY = lastCenter.y + Range.y;

        for (int x = nLastMinX; x <= nLastMaxX; x++)
        {
            for (int y = nLastMinY; y <= nLastMaxY; y++)
            {
                if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    if (tilePosition.x < nMinX || tilePosition.x > nMaxX || tilePosition.y < nMinY || tilePosition.y > nMaxY)
                    {
                        PrintTool.Log("000000");
                        RecycleTile(Map_Back, tilePosition);
                        RecycleTile(Map_Middle, tilePosition);
                        RecycleTile(Map_Front, tilePosition);
                    }
                }
            }
        }

    }

    private IEnumerator DrawMapBack(Vector3Int center, Vector3Int Range)
    {
        int nMinX = center.x - Range.x;
        int nMaxX = center.x + Range.x;
        int nMinY = center.y - Range.y;
        int nMaxY = center.y + Range.y;

        for (int x = nMinX; x <= nMaxX; x++)
        {
            for (int y = nMinY; y <= nMaxY; y++)
            {
                if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    if (y % 2 == 0 && x % 2 == 0 && mMapData.MapCells[x, y].BackIndex >= 0 && mMapData.MapCells[x, y].BackImage >= 0)
                    {
                        Tile tile = GetTile(Map_Back, tilePosition);
                        yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].BackIndex, mMapData.MapCells[x, y].BackImage);
                        Map_Back.SetTile(tilePosition, tile);
                    }
                }
            }
        }
    }

    private IEnumerator DrawMapMiddle(Vector3Int center, Vector3Int Range)
    {
        int nMinX = center.x - Range.x;
        int nMaxX = center.x + Range.x;
        int nMinY = center.y - Range.y;
        int nMaxY = center.y + Range.y;

        for (int x = nMinX; x <= nMaxX; x++)
        {
            for (int y = nMinY; y <= nMaxY; y++)
            {
                if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height)
                {
                    if (mMapData.MapCells[x, y].MiddleIndex >= 0 && mMapData.MapCells[x, y].MiddleImage >= 0)
                    {
                        Vector3Int tilePosition = new Vector3Int(x, y, 0);
                        Tile tile = GetTile(Map_Middle, tilePosition);
                        yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].MiddleIndex, mMapData.MapCells[x, y].MiddleImage);
                        Map_Middle.SetTile(tilePosition, tile);
                    }
                }
            }
        }
    }

    private IEnumerator DrawMapFront(Vector3Int center, Vector3Int Range)
    {
        int nMinX = center.x - Range.x;
        int nMaxX = center.x + Range.x;
        int nMinY = center.y - Range.y;
        int nMaxY = center.y + Range.y;

        for (int x = nMinX; x <= nMaxX; x++)
        {
            for (int y = nMinY; y <= nMaxY; y++)
            {
                if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height)
                {
                    if (mMapData.MapCells[x, y].FrontIndex >= 0 && mMapData.MapCells[x, y].FrontImage >= 0)
                    {
                        Vector3Int tilePosition = new Vector3Int(x, y, 0);
                        Tile tile = GetTile(Map_Front, tilePosition);
                        yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].FrontIndex, mMapData.MapCells[x, y].FrontImage);
                        Map_Front.SetTile(tilePosition, tile);
                    }
                }
            }
        }
    }

    private Tile GetTile(Tilemap mMap, Vector3Int position)
    {
        Tile tile = mMap.GetTile<Tile>(position);
        if (tile == null)
        {
            tile = mTilePool.popObj();
        }
        return tile;
    }

    private void RecycleTile(Tilemap mMap, Vector3Int position)
    {
        Tile tile = mMap.GetTile<Tile>(position);
        if (tile != null)
        {
            mTilePool.recycleObj(tile);
            mMap.SetTile(position, null);

            PrintTool.Log("回收");
        }
    }

}
