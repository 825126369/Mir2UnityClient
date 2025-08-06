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
        public Mir2TileMap Map_AnimationLayer;
        public Mir2TileMap Map_Mir3MiddleLayer;
        public Mir2TileMap Map_FrontAnimationLayer;
        public SpriteRenderer mBackground;

        public MapTileDraw MapTileDrawPrefab;
        readonly NodeComponentPool<MapTileDraw> mMapTileDrawPool = new NodeComponentPool<MapTileDraw>();
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
            mMapTileDrawPool.Init(MapTileDrawPrefab.gameObject, 10);

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
            Range = new Vector3Int(20, 20, 0);
            mMapData = DataCenter.Instance.MapData.mMapBasicInfo;
            UpdateMap();
            DrawBackground();
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

                    RecycleMapTile(Map_AnimationLayer, tilePosition);
                    RecycleMapTile(Map_Mir3MiddleLayer, tilePosition);
                    RecycleMapTile(Map_FrontAnimationLayer, tilePosition);
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
            mDrawPosList.Add(nowCenter);
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
            if (Map_AnimationLayer.GetTile(tilePosition) != null) return;

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
                            var tile = GetMapTileDraw(Map_AnimationLayer);
                            tile.SetData(mSprite, MLibraryMgr.Instance.GetMapImage(190, index));
                            Vector3 point = mWorldPos;
                            tile.DrawUp(point);
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
            if (Map_Mir3MiddleLayer.GetTile(tilePosition) != null) return;

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

                        if (animation > 0)
                        {
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
                                            var tile = GetMapTileDraw(Map_Mir3MiddleLayer);
                                            tile.SetData(mSprite, MLibraryMgr.Instance.GetMapImage(nIndex1, nIndex2));
                                            Vector3 point = mWorldPos;
                                            tile.DrawUpBlend(point);
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
                                            var tile = GetMapTileDraw(Map_Mir3MiddleLayer);
                                            tile.SetData(mSprite, MLibraryMgr.Instance.GetMapImage(nIndex1, nIndex2));
                                            Vector3 point = mWorldPos;
                                            tile.DrawUp(point);
                                        }
                                    }
                                });
                            }
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
                                    var tile = GetMapTileDraw(Map_Mir3MiddleLayer);
                                    tile.SetData(mSprite, MLibraryMgr.Instance.GetMapImage(mMapData.MapCells[x, y].MiddleIndex, index));
                                    Vector3 point = mWorldPos;
                                    tile.DrawUp(point);
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
            if (Map_FrontAnimationLayer.GetTile(tilePosition) != null) return;

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
                                var tile = GetMapTileDraw(Map_FrontAnimationLayer);
                                tile.SetData(mSprite, MLibraryMgr.Instance.GetMapImage(fileIndex, index));
                                Vector3 point = mWorldPos + new Vector3(0, 3 * DataCenter.CellHeight);
                                tile.DrawBlend(point, Color.white, true);
                            }
                            else
                            {
                                var tile = GetMapTileDraw(Map_FrontAnimationLayer);
                                tile.SetData(mSprite, MLibraryMgr.Instance.GetMapImage(fileIndex, index));
                                Vector3 point = mWorldPos + new Vector3(0, mSprite.texture.height);
                                tile.DrawBlend(point, Color.white, index >= 2723 && index <= 2732);
                            }
                        }
                        else
                        {
                            var tile = GetMapTileDraw(Map_FrontAnimationLayer);
                            tile.SetData(mSprite, MLibraryMgr.Instance.GetMapImage(fileIndex, index));
                            Vector3 point = mWorldPos + new Vector3(0, mSprite.texture.height);
                            tile.Draw(point);
                        }
                    }
                }
            });
        }

        private void DrawBackground()
        {
            string cleanFilename = mapFileName.Replace(@".\Map\", "");
            if (cleanFilename.StartsWith("ID1") || cleanFilename.StartsWith("ID2"))
            {
                //mountains
                var mImage = MLibraryMgr.Instance.GetImage(MLibrarys.Background, 10);
                if (mImage != null)
                {
                    var mSprite = MLibraryMgr.CreateSprite(mImage.Image);
                    if (mSprite != null)
                    {
                        mBackground.sprite = mSprite;
                        mBackground.transform.position = Vector3.zero;
                    }
                }
            }
            else if (cleanFilename.StartsWith("ID3_013"))
            {
                //desert
                var mImage = MLibraryMgr.Instance.GetImage(MLibrarys.Background, 22);
                if (mImage != null)
                {
                    var mSprite = MLibraryMgr.CreateSprite(mImage.Image);
                    if (mSprite != null)
                    {
                        mBackground.sprite = mSprite;
                        mBackground.transform.position = Vector3.zero;
                    }
                }
            }
            else if (cleanFilename.StartsWith("ID3_015"))
            {
                //greatwall
                var mImage = MLibraryMgr.Instance.GetImage(MLibrarys.Background, 23);
                if (mImage != null)
                {
                    var mSprite = MLibraryMgr.CreateSprite(mImage.Image);
                    if (mSprite != null)
                    {
                        mBackground.sprite = mSprite;
                        mBackground.transform.position = Vector3.zero;
                    }
                }
            }
            else if (cleanFilename.StartsWith("ID3_023") || cleanFilename.StartsWith("ID3_025"))
            {
                //village entrance
                var mImage = MLibraryMgr.Instance.GetImage(MLibrarys.Background, 21);
                if (mImage != null)
                {
                    var mSprite = MLibraryMgr.CreateSprite(mImage.Image);
                    if (mSprite != null)
                    {
                        mBackground.sprite = mSprite;
                        mBackground.transform.position = Vector3.zero;
                    }
                }
            }
        }

        private void DrawLight()
        {
            //#region Night Lights
            //Color darkness;

            //switch (setting)
            //{
            //    case LightSetting.Night:
            //        {
            //            switch (MapDarkLight)
            //            {
            //                case 1:
            //                    darkness = Color.FromArgb(255, 20, 20, 20);
            //                    break;
            //                case 2:
            //                    darkness = Color.LightSlateGray;
            //                    break;
            //                case 3:
            //                    darkness = Color.SkyBlue;
            //                    break;
            //                case 4:
            //                    darkness = Color.Goldenrod;
            //                    break;
            //                default:
            //                    darkness = Color.Black;
            //                    break;
            //            }
            //        }
            //        break;
            //    case LightSetting.Evening:
            //    case LightSetting.Dawn:
            //        darkness = Color.FromArgb(255, 50, 50, 50);
            //        break;
            //    default:
            //    case LightSetting.Day:
            //        darkness = Color.FromArgb(255, 255, 255, 255);
            //        break;
            //}

            //if (MapObject.User.Poison.HasFlag(PoisonType.Blindness))
            //{
            //    darkness = GetBlindLight(darkness);
            //}

            //DXManager.Device.Clear(ClearFlags.Target, darkness, 0, 0);

            //#endregion
        }

        public Vector3Int GetTilePos(int x, int y, int z)
        {
            return new Vector3Int(x, -y, 0);
        }

        private MapTileDraw GetMapTileDraw(Mir2TileMap mTileMap)
        {
            var mTile = mMapTileDrawPool.popObj();
            mTile.transform.SetParent(mTileMap.transform, false);
            mTile.transform.localScale = Vector3.one;
            mTile.transform.position = Vector3.zero;
            return mTile;
        }

        private void RecycleMapTile(Mir2TileMap mMap, Vector3Int position)
        {
            MapTileDraw tile = mMap.GetTile(position);
            if (tile != null)
            {
                tile.Clear();
                mMapTileDrawPool.recycleObj(tile);
                mMap.SetTile(position, null);
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
