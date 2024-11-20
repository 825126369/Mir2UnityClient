using CrystalMir2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mir2
{
    public class TileMapMgr : MonoBehaviour
    {
        public Tilemap Map_Back;
        public Tilemap Map_Middle;
        public Tilemap Map_Front;

        readonly ScriptableObjectPool<Tile> mTilePool = new ScriptableObjectPool<Tile>();
        private bool bInit = false;
        readonly List<Vector3Int> mDrawPosList = new List<Vector3Int>();

        public MapReader mMapData;

        public Vector3Int nowCenter;
        public Vector3Int Range = new Vector3Int(100, 100, 0);
        public string mapFileName = "0";
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            if (bInit) return;
            bInit = true;
            mTilePool.Init(1000);
        }

        public void LoadMapTest()
        {
            string path = $"D:\\Me\\MyProject\\Mir2Server\\Mir2Config\\Maps\\{mapFileName}.map";
            mMapData = new MapReader(path);

            ClearTile();
            GetDrawPosList();
            StartCoroutine(DrawMap(mDrawPosList));
        }

        public void Load()
        {
            mMapData = DataCenter.Instance.MapData.mMapBasicInfo;
            UpdateMap();
        }

        public void UpdateMap()
        {
            nowCenter = DataCenter.Instance.UserData.MapLocation;
            ClearTile();
            GetDrawPosList();
            StartCoroutine(DrawMap(mDrawPosList));
        }

        private void ClearTile()
        {
            foreach (var mPos in mDrawPosList)
            {
                int x = mPos.x;
                int y = mPos.y;
                if (!orInRange(mPos))
                {
                    Vector3Int tilePosition = GetTilePos(x, y, 0);
                    RecycleTile(Map_Back, tilePosition);
                    RecycleTile(Map_Middle, tilePosition);
                    RecycleTile(Map_Front, tilePosition);
                }
            }
        }

        private bool orInRange(Vector3Int tilePosition)
        {
            int nMinX = nowCenter.x - Range.x;
            int nMaxX = nowCenter.x + Range.x;
            int nMinY = nowCenter.y - Range.y;
            int nMaxY = nowCenter.y + Range.y;

            if (tilePosition.x < nMinX || tilePosition.x > nMaxX || tilePosition.y < nMinY || tilePosition.y > nMaxY)
            {
                return false;
            }

            return true;
        }

        private void GetDrawPosList()
        {
            int nMinX = nowCenter.x - Range.x;
            int nMaxX = nowCenter.x + Range.x;
            int nMinY = nowCenter.y - Range.y;
            int nMaxY = nowCenter.y + Range.y;

            int nRangeMax = Math.Max(Range.x, Range.y);
            mDrawPosList.Clear();
            for (int i = 1; i <= nRangeMax; i++)
            {
                int x = i;
                int y = 0;
                for (y = nowCenter.y - i; y <= nowCenter.y + i; y++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height && x >= nMinX && x <= nMaxX && y >= nMinY && y <= nMaxY)
                    {
                        mDrawPosList.Add(new Vector3Int(x, y));
                    }
                }

                x = -i;
                for (y = nowCenter.y - i; y <= nowCenter.y + i; y++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height && x >= nMinX && x <= nMaxX && y >= nMinY && y <= nMaxY)
                    {
                        mDrawPosList.Add(new Vector3Int(x, y));
                    }
                }

                y = i;
                for (x = nowCenter.x - i + 1; x <= nowCenter.x + i - 1; x++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height && x >= nMinX && x <= nMaxX && y >= nMinY && y <= nMaxY)
                    {
                        mDrawPosList.Add(new Vector3Int(x, y));
                    }
                }

                y = -i;
                for (x = nowCenter.x - i + 1; x <= nowCenter.x + i - 1; x++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height && x >= nMinX && x <= nMaxX && y >= nMinY && y <= nMaxY)
                    {
                        mDrawPosList.Add(new Vector3Int(x, y));
                    }
                }
            }
        }

        private IEnumerator DrawMap(List<Vector3Int> mDrawPosList)
        {
            int nRefreshPosCount = 0;
            foreach (var v in mDrawPosList)
            {
                if (nRefreshPosCount++ > 1000)
                {
                    nRefreshPosCount = 0;
                    yield return null;
                }

                DrawMapBackCell(v);
                DrawMapMiddleCell(v);
                DrawMapFrontCell(v);
            }
        }

        private void DrawMapBackCell(Vector3Int mPos)
        {
            int x = mPos.x;
            int y = mPos.y;
            Vector3Int tilePosition = GetTilePos(x, y, 0);
            if (Map_Back.GetTile(tilePosition) != null) return;

            if (x <= 0 || x % 2 == 1) return;
            if(x >= mMapData.Width) return;
            if (mMapData.MapCells[x, y].BackImage == 0 || mMapData.MapCells[x, y].BackIndex == -1) return;

            int nIndex1 = mMapData.MapCells[x, y].BackIndex;
            int nIndex2 = (mMapData.MapCells[x, y].BackImage & 0x1FFFFFFF) - 1;

            if (y % 2 == 0 && x % 2 == 0 && nIndex1 >= 0 && nIndex2 >= 0)
            {
                Mir2Res.Instance.SetMapSprite(nIndex1, nIndex2, (mSprite) =>
                {
                    if (mSprite != null)
                    {
                        if (orInRange(mPos))
                        {
                            Tile tile = GetTile(Map_Back, tilePosition);
                            tile.sprite = mSprite;
                            Map_Back.SetTile(tilePosition, tile);
                        }
                    }
                });
            }

        }

        private void DrawMapMiddleCell(Vector3Int mPos)
        {
            int x = mPos.x;
            int y = mPos.y;
            Vector3Int tilePosition = GetTilePos(x, y, 0);
            if (Map_Middle.GetTile(tilePosition) != null) return;
            if (x < 0) return;
            if (x >= mMapData.Width) return;

            int nIndex1 = mMapData.MapCells[x, y].MiddleIndex;
            int nIndex2 = (mMapData.MapCells[x, y].MiddleImage) - 1;

            if ((nIndex2 < 0) || nIndex1 == -1) return;

            if (nIndex1 >= 0)
            {
                Mir2Res.Instance.SetMapSprite(nIndex1, nIndex2, (mSprite) =>
                {
                    if (mSprite != null)
                    {
                        if (mSprite.texture.width != DataCenter.CellWidth || mSprite.texture.height != DataCenter.CellHeight) return;

                        if (orInRange(mPos))
                        {
                            Tile tile = GetTile(Map_Middle, tilePosition);
                            tile.sprite = mSprite;
                            Map_Middle.SetTile(tilePosition, tile);
                        }
                    }
                });
            }
        }

        private void DrawMapFrontCell(Vector3Int mPos)
        {
            int x = mPos.x;
            int y = mPos.y;
            Vector3Int tilePosition = GetTilePos(x, y, 0);
            if (Map_Front.GetTile(tilePosition) != null) return;
            if (x < 0) return;
            if (x >= mMapData.Width) return;

            int nIndex1 = mMapData.MapCells[x, y].FrontIndex;
            int nIndex2 = (mMapData.MapCells[x, y].FrontImage & 0x7FFF) - 1;

            if (nIndex1 >= 0 && nIndex2 >= 0)
            {
                if (nIndex1 == 200) return; //fixes random bad spots on old school 4.map
                Mir2Res.Instance.SetMapSprite(nIndex1, nIndex2, (mSprite) =>
                {
                    if (mSprite != null)
                    {
                        if ((mSprite.texture.width != DataCenter.CellWidth || mSprite.texture.height != DataCenter.CellHeight) &&
                            ((mSprite.texture.width != DataCenter.CellWidth * 2) || (mSprite.texture.height != DataCenter.CellHeight * 2)))
                        {
                            return;
                        }

                        if (orInRange(mPos))
                        {
                            Tile tile = GetTile(Map_Front, tilePosition);
                            tile.sprite = mSprite;
                            Map_Front.SetTile(tilePosition, tile);
                        }
                    }
                });
            }
        }

        public Vector3Int GetTilePos(int x, int y, int z)
        {
            return new Vector3Int(x, -y, 0);
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
            }
        }

    }
}
