using Mir2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapTest : MonoBehaviour
{
    public Tilemap mTileMap;
    public TileBase mTileBase;

    private void Start()
    {
        for (int x = 0; x <= 10; x++)
        {
            for (int y = 0; y <= 10; y++)
            {
                Vector3Int mPos = new Vector3Int(x, -y, 0);
                mTileMap.SetTile(mPos, mTileBase);
            }
        }
    }
}
