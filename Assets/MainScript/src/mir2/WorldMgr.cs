using NetProtocols.Game;
using System.Collections.Generic;
using UnityEngine;

namespace Mir2
{
    public class WorldMgr : SingleTonMonoBehaviour<WorldMgr>
    {
        public readonly List<MapObject> mMapObjects = new List<MapObject>();
        private bool bInit = false;

        public List<PlayerObject> mPlayerList = new List<PlayerObject>();
        public UserObject User;

        private void Init()
        {
            if (bInit) return;
            bInit = true;
            User = GameObject.Find("Me").GetComponent<UserObject>();
            PrintTool.Assert(User != null, "User == null");
        }

        public void Start()
        {
            Init();
            User.Init();
            LoadMap(DataCenter.Instance.nCurrentMapIndex);
        }

        public void LoadMap(uint nMapInfoIndex)
        {
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

        public void HandleServerLocation(uint ObjectID, Vector3Int Location, MirDirection dir)
        {
            foreach(var v in mPlayerList)
            {
                
            }
        }
    }
}
