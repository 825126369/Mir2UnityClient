using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapMgr : MonoBehaviour
{
    public RuleTile tilePrefab1; // 从Unity编辑器中拖拽进来的Tile预设体
    public TileBase tilePrefab; // 从Unity编辑器中拖拽进来的Tile预设体

    public Tilemap Map_Back;
    public Tilemap Map_Middle; 
    public Tilemap Map_Front; 

    public int mapWidth = 1000; // 地图的宽度
    public int mapHeight = 1000; // 地图的高度

    private MapData mMapData;

    private void Start()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        string path = $"Assets/CrystalMir2/Map2/0.json";
        string json = File.ReadAllText(path);
        MapData mData = JsonTool.FromJson<MapData>(json);
        mMapData = mData;
        
        StartCoroutine(DrawMapBack());
        StartCoroutine(DrawMapMiddle());
        StartCoroutine(DrawMapFront());
    }

    private IEnumerator DrawMapBack()
    { 
        for (int x = 0; x < mMapData.Width; x++)
        {
            for (int y = 0; y < mMapData.Height; y++)
            {
                // 计算Tile位置
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (y % 2 == 0 && x % 2 == 0 && mMapData.MapCells[x, y].BackIndex >= 0 && mMapData.MapCells[x, y].BackImage >= 0)
                {
                    var tile = new RuleTile();
                    yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].BackIndex, mMapData.MapCells[x, y].BackImage);
                    // 为Tile位置设置Tile
                    Map_Back.SetTile(tilePosition, tile);
                }
            }
        }
    }

    private IEnumerator DrawMapMiddle()
    {
        for (int x = 0; x < mMapData.Width; x++)
        {
            for (int y = 0; y < mMapData.Height; y++)
            {
                // 计算Tile位置
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (mMapData.MapCells[x, y].MiddleIndex >= 0 && mMapData.MapCells[x, y].MiddleImage >= 0)
                {
                    var tile = new RuleTile();
                    yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].MiddleIndex, mMapData.MapCells[x, y].MiddleImage);
                    // 为Tile位置设置Tile
                    Map_Middle.SetTile(tilePosition, tile);
                }
            }
        }
    }

    private IEnumerator DrawMapFront()
    {
        for (int x = 0; x < mMapData.Width; x++)
        {
            for (int y = 0; y < mMapData.Height; y++)
            {
                // 计算Tile位置
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (mMapData.MapCells[x, y].FrontIndex >= 0 && mMapData.MapCells[x, y].FrontImage >= 0)
                {
                    var tile = new RuleTile();
                    yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].FrontIndex, mMapData.MapCells[x, y].FrontImage);
                    // 为Tile位置设置Tile
                    Map_Front.SetTile(tilePosition, tile);
                }
            }
        }
    }

    private void GenerateLegendMap()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // 计算Tile位置
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                // 为Tile位置设置Tile
                //tilemap.SetTile(tilePosition, tilePrefab1);
            }
        }
    }
}
