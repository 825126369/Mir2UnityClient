using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapMgr : MonoBehaviour
{
    public RuleTile tilePrefab1; // ��Unity�༭������ק������TileԤ����
    public TileBase tilePrefab; // ��Unity�༭������ק������TileԤ����

    public Tilemap Map_Back;
    public Tilemap Map_Middle; 
    public Tilemap Map_Front; 

    public int mapWidth = 1000; // ��ͼ�Ŀ��
    public int mapHeight = 1000; // ��ͼ�ĸ߶�

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
                // ����Tileλ��
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (y % 2 == 0 && x % 2 == 0 && mMapData.MapCells[x, y].BackIndex >= 0 && mMapData.MapCells[x, y].BackImage >= 0)
                {
                    var tile = new RuleTile();
                    yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].BackIndex, mMapData.MapCells[x, y].BackImage);
                    // ΪTileλ������Tile
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
                // ����Tileλ��
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (mMapData.MapCells[x, y].MiddleIndex >= 0 && mMapData.MapCells[x, y].MiddleImage >= 0)
                {
                    var tile = new RuleTile();
                    yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].MiddleIndex, mMapData.MapCells[x, y].MiddleImage);
                    // ΪTileλ������Tile
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
                // ����Tileλ��
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (mMapData.MapCells[x, y].FrontIndex >= 0 && mMapData.MapCells[x, y].FrontImage >= 0)
                {
                    var tile = new RuleTile();
                    yield return Mir2Res.Instance.SetMapSprite2(tile, mMapData.MapCells[x, y].FrontIndex, mMapData.MapCells[x, y].FrontImage);
                    // ΪTileλ������Tile
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
                // ����Tileλ��
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                // ΪTileλ������Tile
                //tilemap.SetTile(tilePosition, tilePrefab1);
            }
        }
    }
}
