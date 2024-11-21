using Mir2;
using System.Collections.Generic;
using UnityEngine;

public class Mir2TileMap : MonoBehaviour
{
    Dictionary<Vector3Int, MapTileDraw> mTileDic = new Dictionary<Vector3Int, MapTileDraw>();
    public MapTileDraw GetTile(Vector3Int mPos)
    {
        if (mTileDic.TryGetValue(mPos, out MapTileDraw mapTileDraw))
        {
            return mapTileDraw;
        }

        return null;
    }

    public void SetTile(Vector3Int mPos, MapTileDraw mTile)
    {
        var OldTile = GetTile(mPos);
        if (OldTile != null)
        {
            Destroy(OldTile.gameObject);
        }
        mTileDic[mPos] = mTile;
    }
}