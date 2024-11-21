using CrystalMir2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace Mir2
{
    public class TileMapMgr : MonoBehaviour
    {
        public Tilemap Map_Back;
        public Tilemap Map_Middle;
        public Tilemap Map_Front;
        public SortingGroup Map_AnimationLayer;
        public SortingGroup Map_Mir3MiddleLayer;
        public SortingGroup Map_FrontAnimationLayer;
        public SpriteRenderer SpritePrefab;

        readonly NodeComponentPool<SpriteRenderer> mSpriteRendererPool = new NodeComponentPool<SpriteRenderer>();
        readonly ScriptableObjectPool<Tile> mTilePool = new ScriptableObjectPool<Tile>();
        private bool bInit = false;
        readonly List<Vector3Int> mDrawPosList = new List<Vector3Int>();

        public MapReader mMapData;

        public Vector3Int nowCenter;
        public Vector3Int Range = new Vector3Int(100, 100, 0);
        public string mapFileName = "0";


        private int AnimationCount = 0;
        private float MoveTime = 0;
        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (Time.time >= MoveTime)
            {
                MoveTime += 0.1f; //Move Speed
                AnimationCount++;
            }
        }

        private void Init()
        {
            if (bInit) return;
            bInit = true;
            mTilePool.Init(1000);
            mSpriteRendererPool.Init(SpritePrefab.gameObject, 10);

            Map_Back.ClearAllEditorPreviewTiles();
            Map_Middle.ClearAllEditorPreviewTiles();
            Map_Front.ClearAllEditorPreviewTiles();

            Map_Back.ClearAllTiles();
            Map_Middle.ClearAllTiles();
            Map_Front.ClearAllTiles();
        }

        public void LoadMapTest()
        {
            Init();
            string path = $"D:\\Me\\MyProject\\Mir2Server\\Mir2Config\\Maps\\{mapFileName}.map";
            mMapData = new MapReader(path);

            ClearTile();
            GetDrawPosList();
            StartCoroutine(DrawMap(mDrawPosList));
        }

        public void Load()
        {
            Init();
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
            RecycleSpriteRenderer(Map_AnimationLayer);
            RecycleSpriteRenderer(Map_Mir3MiddleLayer);
            RecycleSpriteRenderer(Map_FrontAnimationLayer);

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
            mDrawPosList.Clear();
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
                int x = nowCenter.x + i;
                int y = 0;
                for (y = nowCenter.y - i; y <= nowCenter.y + i; y++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height && x >= nMinX && x <= nMaxX && y >= nMinY && y <= nMaxY)
                    {
                        mDrawPosList.Add(new Vector3Int(x, y));
                    }
                }

                x = nowCenter.x - i;
                for (y = nowCenter.y - i; y <= nowCenter.y + i; y++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height && x >= nMinX && x <= nMaxX && y >= nMinY && y <= nMaxY)
                    {
                        mDrawPosList.Add(new Vector3Int(x, y));
                    }
                }

                y = nowCenter.y + i;
                for (x = nowCenter.x - i + 1; x <= nowCenter.x + i - 1; x++)
                {
                    if (x >= 0 && x < mMapData.Width && y >= 0 && y < mMapData.Height && x >= nMinX && x <= nMaxX && y >= nMinY && y <= nMaxY)
                    {
                        mDrawPosList.Add(new Vector3Int(x, y));
                    }
                }

                y = nowCenter.y - i;
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

                DrawMapAnimationLayerCell(v);
                DrawMapMir3MiddleLayerCell(v);
                DrawMapFrontAnimationLayerCell(v);
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
                Mir2Res.Instance.GetMapSpriteByMLibrary(nIndex1, nIndex2, (mSprite) =>
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
                Mir2Res.Instance.GetMapSpriteByMLibrary(nIndex1, nIndex2, (mSprite) =>
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
                Debug.Log(nIndex1 + " | " + nIndex2);
                if (nIndex1 == 200) return; //fixes random bad spots on old school 4.map

                if (mMapData.MapCells[x, y].DoorIndex > 0)
                {
                    Door DoorInfo = DataCenter.Instance.MapData.GetDoor(mMapData.MapCells[x, y].DoorIndex);
                    if (DoorInfo == null)
                    {
                        DoorInfo = new Door() { index = mMapData.MapCells[x, y].DoorIndex, DoorState = 0, ImageIndex = 0, LastTick = (long)Time.time * 1000 };
                        DataCenter.Instance.MapData.mDoorList.Add(DoorInfo);
                    }
                    else
                    {
                        if (DoorInfo.DoorState != 0)
                        {
                            nIndex2 += (DoorInfo.ImageIndex + 1) * mMapData.MapCells[x, y].DoorOffset;
                        }
                    }
                }

                Mir2Res.Instance.GetMapSpriteByMLibrary(nIndex1, nIndex2, (mSprite) =>
                {
                    if (mSprite != null)
                    {
                        if (
                            (mSprite.texture.width != DataCenter.CellWidth || mSprite.texture.height != DataCenter.CellHeight) &&
                            (mSprite.texture.width != DataCenter.CellWidth * 2 || mSprite.texture.height != DataCenter.CellHeight * 2)
                            )
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

        private void DrawMapAnimationLayerCell(Vector3Int mPos)
        {
            int x = mPos.x;
            int y = mPos.y;
            Vector3Int tilePosition = GetTilePos(x, y + 1, 0);
            Vector3 mWorldPos = new Vector3(tilePosition.x * DataCenter.CellWidth, tilePosition.y * DataCenter.CellHeight);

            if (x < 0) return;
            if (x >= mMapData.Width) return;

            int index = mMapData.MapCells[x, y].TileAnimationImage;
            byte animation = mMapData.MapCells[x, y].TileAnimationFrames;
            if ((index > 0) & (animation > 0))
            {
                index--;
                int animationoffset = mMapData.MapCells[x, y].TileAnimationOffset ^ 0x2000;
                index += animationoffset * (AnimationCount % animation);

                Mir2Res.Instance.GetMapSpriteByMLibrary(190, index, (mSprite) =>
                {
                    if (mSprite != null)
                    {
                        if (orInRange(mPos))
                        {
                            var tile = GetSpriteRenderer(Map_AnimationLayer);
                            tile.transform.position = mWorldPos + new Vector3(0, mSprite.texture.height, 0);
                            tile.sprite = mSprite;
                        }
                    }
                });
            }
        }

        private void DrawMapMir3MiddleLayerCell(Vector3Int mPos)
        {
            int x = mPos.x;
            int y = mPos.y;
            Vector3Int tilePosition = GetTilePos(x, y + 1, 0);
            Vector3 mWorldPos = new Vector3(tilePosition.x * DataCenter.CellWidth, tilePosition.y * DataCenter.CellHeight);

            if ((mMapData.MapCells[x, y].MiddleIndex >= 0) && (mMapData.MapCells[x, y].MiddleIndex != -1))
            { 
                int index = mMapData.MapCells[x, y].MiddleImage - 1;
                if (index > 0)
                {
                    byte animation = mMapData.MapCells[x, y].MiddleAnimationFrame;
                    bool blend = false;
                    if ((animation > 0) && (animation < 255))
                    {
                        if ((animation & 0x0f) > 0)
                        {
                            blend = true;
                            animation &= 0x0f;
                        }

                        byte animationTick = mMapData.MapCells[x, y].MiddleAnimationTick;
                        index += (AnimationCount % (animation + (animation * animationTick))) / (1 + animationTick);

                        if (blend && (animation == 10 || animation == 8)) //diamond mines, abyss blends
                        {
                            int nIndex1 = mMapData.MapCells[x, y].MiddleIndex;
                            int nIndex2 = index;
                            Mir2Res.Instance.GetMapSpriteByMLibrary(nIndex1, nIndex2, (mSprite) =>
                            {
                                if (mSprite != null)
                                {
                                    if (orInRange(mPos))
                                    {
                                        var tile = GetSpriteRenderer(Map_Mir3MiddleLayer);
                                        tile.transform.position = mWorldPos + new Vector3(0, mSprite.texture.height, 0);
                                        tile.sprite = mSprite;
                                    }
                                }
                            });
                        }
                        else
                        {
                            int nIndex1 = mMapData.MapCells[x, y].MiddleIndex;
                            int nIndex2 = index;
                            Mir2Res.Instance.GetMapSpriteByMLibrary(nIndex1, nIndex2, (mSprite) =>
                            {
                                if (mSprite != null)
                                {
                                    if (orInRange(mPos))
                                    {
                                        var tile = GetSpriteRenderer(Map_Mir3MiddleLayer);
                                        tile.transform.position = mWorldPos + new Vector3(0, mSprite.texture.height, 0);
                                        tile.sprite = mSprite;
                                    }
                                }
                            });
                        }

                    }

                    Mir2Res.Instance.GetMapSpriteByMLibrary(mMapData.MapCells[x, y].MiddleIndex, index, (mSprite) =>
                    {
                        if (mSprite != null)
                        {
                            if ((mSprite.texture.width != DataCenter.CellWidth || mSprite.texture.height != DataCenter.CellHeight) &&
                                ((mSprite.texture.width != DataCenter.CellWidth * 2) || (mSprite.texture.height != DataCenter.CellHeight * 2)) &&
                                !blend)
                            {
                                if (orInRange(mPos))
                                {
                                    var tile = GetSpriteRenderer(Map_Mir3MiddleLayer);
                                    tile.transform.position = mWorldPos + new Vector3(0, mSprite.texture.height, 0);
                                    tile.sprite = mSprite;
                                }
                            }
                        }
                    });
                }
            }
        }

        private void DrawMapFrontAnimationLayerCell(Vector3Int mPos)
        {
            int x = mPos.x;
            int y = mPos.y;
            Vector3Int tilePosition = GetTilePos(x, y + 1, 0);
            Vector3 mWorldPos = new Vector3(tilePosition.x * DataCenter.CellWidth, tilePosition.y * DataCenter.CellHeight);

            int index = (mMapData.MapCells[x, y].FrontImage & 0x7FFF) - 1;

            if (index < 0) return;

            int fileIndex = mMapData.MapCells[x, y].FrontIndex;
            if (fileIndex == -1) return;
            byte animation = mMapData.MapCells[x, y].FrontAnimationFrame;

            bool blend = false;
            if ((animation & 0x80) > 0)
            {
                blend = true;
                animation &= 0x7F;
            }
            
            if (animation > 0)
            {
                byte animationTick = mMapData.MapCells[x, y].FrontAnimationTick;
                index += (AnimationCount % (animation + (animation * animationTick))) / (1 + animationTick);
            }
            
            if (mMapData.MapCells[x, y].DoorIndex > 0)
            {
                Door DoorInfo = DataCenter.Instance.MapData.GetDoor(mMapData.MapCells[x, y].DoorIndex);
                if (DoorInfo == null)
                {
                    DoorInfo = new Door() { index = mMapData.MapCells[x, y].DoorIndex, DoorState = 0, ImageIndex = 0, LastTick = (long)Time.time * 1000 };
                    DataCenter.Instance.MapData.mDoorList.Add(DoorInfo);
                }
                else
                {
                    if (DoorInfo.DoorState != 0)
                    {
                        index += (DoorInfo.ImageIndex + 1) * mMapData.MapCells[x, y].DoorOffset;
                    }
                }
            }

            Mir2Res.Instance.GetMapSpriteByMLibrary(fileIndex, index, (mSprite) =>
            {
                if (mSprite != null)
                {
                    if (mSprite.texture.width == DataCenter.CellWidth && mSprite.texture.height == DataCenter.CellHeight && animation == 0) return;
                    if (mSprite.texture.width == DataCenter.CellWidth * 2 && mSprite.texture.height == DataCenter.CellHeight * 2 && animation == 0) return;

                    if (orInRange(mPos))
                    {
                        if (blend)
                        {
                            if ((fileIndex > 99) & (fileIndex < 199))
                            {
                                var tile = GetSpriteRenderer(Map_FrontAnimationLayer);
                                tile.transform.position = mWorldPos + new Vector3(0, mSprite.texture.height, 0);
                                tile.sprite = mSprite;
                            }
                            else
                            {
                                var tile = GetSpriteRenderer(Map_FrontAnimationLayer);
                                tile.transform.position = mWorldPos + new Vector3(0, mSprite.texture.height, 0);
                                tile.sprite = mSprite;
                            }
                        }
                        else
                        {
                            var tile = GetSpriteRenderer(Map_FrontAnimationLayer);
                            tile.transform.position = mWorldPos + new Vector3(0, mSprite.texture.height, 0);
                            tile.sprite = mSprite;
                        }
                    }
                }
            });
        }

        public Vector3Int GetTilePos(int x, int y, int z)
        {
            return new Vector3Int(x, -y, 0);
        }

        private SpriteRenderer GetSpriteRenderer(SortingGroup sortingGroup)
        {
            var mSpriteRenderer = mSpriteRendererPool.popObj();
            mSpriteRenderer.transform.SetParent(sortingGroup.transform, false);
            mSpriteRenderer.transform.localScale = Vector3.one;
            mSpriteRenderer.transform.position = Vector3.zero;
            return mSpriteRenderer;
        }

        private void RecycleSpriteRenderer(SortingGroup mMap)
        {
            foreach(Transform v in mMap.transform)
            {
                mSpriteRendererPool.recycleObj(v.GetComponent<SpriteRenderer>());
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
                tile.sprite = null;
                mTilePool.recycleObj(tile);
                mMap.SetTile(position, null);
            }
        }

    }
}
