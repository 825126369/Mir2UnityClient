using CrystalMir2;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mir2
{
    public class TileMapMgr:MonoBehaviour
    {
        public Tilemap Map_Back;
        public Tilemap Map_Middle;
        public Tilemap Map_Front;

        private MapReader mMapData;
        readonly ScriptableObjectPool<Tile> mTilePool = new ScriptableObjectPool<Tile>();
        private bool bInit = false;
        private Vector3Int lastCenter;
        private Vector3Int nowCenter;
        private Vector3Int Range = new Vector3Int(10, 10, 0);

        private string mapFileName = string.Empty;
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

        public void LoadMap(string fileName = "3")
        {
           // if(this.mapFileName == fileName) return;
            this.mapFileName = fileName;    
            string path = $"D:\\Me\\MyProject\\Mir2Server\\Mir2Config\\Maps\\" + fileName + ".map";
            mMapData = new MapReader(path);
            UpdateMap();
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
            StartCoroutine(DrawMapBack());
            StartCoroutine(DrawMapMiddle());
            StartCoroutine(DrawMapFront());
            lastCenter = nowCenter;
        }

        private void ClearTile()
        {
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
                        Vector3Int fakeTilePosition = new Vector3Int(x, y, 0);
                        if (!orInRange(fakeTilePosition))
                        {
                            Vector3Int tilePosition = GetTilePos(x, y, 0);
                            RecycleTile(Map_Back, tilePosition);
                            RecycleTile(Map_Middle, tilePosition);
                            RecycleTile(Map_Front, tilePosition);
                        }
                    }
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

        private IEnumerator DrawMapBack()
        {
            int nMinX = nowCenter.x - Range.x;
            int nMaxX = nowCenter.x + Range.x;
            int nMinY = nowCenter.y - Range.y;
            int nMaxY = nowCenter.y + Range.y;

            for (int x = nMinX; x <= nMaxX; x++)
            {
                for (int y = nMinY; y <= nMaxY; y++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height)
                    {
                        int nIndex1 = mMapData.MapCells[x, y].BackIndex;
                        int nIndex2 = (mMapData.MapCells[x, y].BackImage & 0x1FFFFFFF) - 1;

                        if (y % 2 == 0 && x % 2 == 0 && nIndex1 >= 0 && nIndex2 >= 0)
                        {
                            yield return Mir2Res.Instance.RequestMapSprite(nIndex1, nIndex2);
                            Sprite mSprite = Mir2Res.Instance.GetMapSprite(nIndex1, nIndex2);
                            Vector3Int fakeTilePosition = new Vector3Int(x, y, 0);
                            if (orInRange(fakeTilePosition))
                            {
                                Vector3Int tilePosition = GetTilePos(x, y, 0);
                                Tile tile = GetTile(Map_Back, tilePosition);
                                tile.sprite = mSprite;
                                Map_Back.SetTile(tilePosition, tile);
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator DrawMapMiddle()
        {
            int nMinX = nowCenter.x - Range.x;
            int nMaxX = nowCenter.x + Range.x;
            int nMinY = nowCenter.y - Range.y;
            int nMaxY = nowCenter.y + Range.y;

            for (int x = nMinX; x <= nMaxX; x++)
            {
                for (int y = nMinY; y <= nMaxY; y++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height)
                    {
                        int nIndex1 = mMapData.MapCells[x, y].MiddleIndex;
                        int nIndex2 = (mMapData.MapCells[x, y].MiddleImage) - 1;

                        if (nIndex1 >= 0 && nIndex2 >= 0)
                        {
                            yield return Mir2Res.Instance.RequestMapSprite(nIndex1, nIndex2);
                            Sprite mSprite = Mir2Res.Instance.GetMapSprite(nIndex1, nIndex2);
                            Vector3Int fakeTilePosition = new Vector3Int(x, y, 0);
                            if (orInRange(fakeTilePosition))
                            {
                                Vector3Int tilePosition = GetTilePos(x, y, 0);
                                Tile tile = GetTile(Map_Middle, tilePosition);
                                tile.sprite = mSprite;
                                Map_Middle.SetTile(tilePosition, tile);
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator DrawMapFront()
        {
            int nMinX = nowCenter.x - Range.x;
            int nMaxX = nowCenter.x + Range.x;
            int nMinY = nowCenter.y - Range.y;
            int nMaxY = nowCenter.y + Range.y;

            for (int x = nMinX; x <= nMaxX; x++)
            {
                for (int y = nMinY; y <= nMaxY; y++)
                {
                    if (x >= 0 && x < mMapData.Width && y > 0 && y < mMapData.Height)
                    {
                        int nIndex1 = mMapData.MapCells[x, y].FrontIndex;
                        int nIndex2 = (mMapData.MapCells[x, y].FrontImage & 0x7FFF) - 1;

                        if (nIndex1 >= 0 && nIndex2 >= 0)
                        {
                            if (nIndex1 == 200) continue; //fixes random bad spots on old school 4.map
                            yield return Mir2Res.Instance.RequestMapSprite(nIndex1, nIndex2);
                            Sprite mSprite = Mir2Res.Instance.GetMapSprite(nIndex1, nIndex2);
                            if ((mSprite.texture.width != DataCenter.CellWidth || mSprite.texture.height != DataCenter.CellHeight) &&
                                ((mSprite.texture.width != DataCenter.CellWidth * 2) || (mSprite.texture.height != DataCenter.CellHeight * 2)))
                            {
                                continue;
                            }

                            Vector3Int fakeTilePosition = new Vector3Int(x, y, 0);
                            if (orInRange(fakeTilePosition))
                            {
                                Vector3Int tilePosition = GetTilePos(x, y, 0);
                                Tile tile = GetTile(Map_Front, tilePosition);
                                tile.sprite = mSprite;
                                Map_Front.SetTile(tilePosition, tile);
                            }
                        }
                    }
                }
            }
        }

        public Vector3Int GetUserTileMapPos(Vector3Int MapLocation)
        {
            return new Vector3Int(MapLocation.x, -MapLocation.y, 0);
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
