using System.Collections.Generic;

namespace Mir2
{
    public class WorldMgr : SingleTonMonoBehaviour<WorldMgr>
    {
        public readonly List<MapObject> mMapObjects = new List<MapObject>();
        private bool bStarted = false;
        private uint nCurrentMapIndex = 0;

        private bool bInit = false;
        public void Init()
        {
            if (bInit) return;
            bInit = true;

            ExcelTableMgr.Instance.Init();
        }

        public void Start()
        {
            Init();
            LoadMap(this.nCurrentMapIndex);
        }

        public void StartGame()
        {

        }

        public void LoadMap(uint nMapInfoIndex)
        {
            this.nCurrentMapIndex = nMapInfoIndex;
            var mMapInfo = ExcelTableMgr.Instance.MapInfoList.Find((x) => x.Index == nMapInfoIndex);
            if (mMapInfo != null)
            {
                if (TileMapMgr.readOnlyInstance != null)
                {
                    TileMapMgr.readOnlyInstance.LoadMap(mMapInfo.FileName);
                }
            }
            else
            {
                PrintTool.LogError("mMapInfo == null: " + nMapInfoIndex);
            }
        }
    }
}
