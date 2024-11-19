using System.Collections.Generic;
using UnityEngine;

namespace Mir2
{
    public class WorldMgr : SingleTonMonoBehaviour<WorldMgr>
    {
        public readonly List<MapObject> mMapObjects = new List<MapObject>();
        public readonly List<Door> mDoorList = new List<Door>();

        public List<PlayerObject> mPlayerList = new List<PlayerObject>();
        public UserObject User;
        public TileMapMgr MapMgr;

        private float _doorTime;

        public void Start()
        {
            User.Init();
            MapMgr.Load();
        }

        public bool ValidPoint(Vector3Int p)
        {
            var M2CellInfo = DataCenter.Instance.MapData.mMapBasicInfo.MapCells;
            return (M2CellInfo[p.x, p.y].BackImage & 0x20000000) == 0;
        }

        public bool EmptyCell(Vector3Int p)
        {
            var M2CellInfo = DataCenter.Instance.MapData.mMapBasicInfo.MapCells;
            if ((M2CellInfo[p.x, p.y].BackImage & 0x20000000) != 0 || (M2CellInfo[p.x, p.y].FrontImage & 0x8000) != 0)
                return false;

            //for (int i = 0; i < mMapObjects.Count; i++)
            //{
            //    MapObject ob = mMapObjects[i];

            //    if (ob.CurrentLocation == p && ob.Blocking)
            //        return false;
            //}

            return true;
        }

        public bool CheckDoorOpen(Vector3Int p)
        {
            var M2CellInfo = DataCenter.Instance.MapData.mMapBasicInfo.MapCells;
            if (M2CellInfo[p.x, p.y].DoorIndex == 0) return true;
            Door DoorInfo = GetDoor(M2CellInfo[p.x, p.y].DoorIndex);
            if (DoorInfo == null) return false;
            if ((DoorInfo.DoorState == DoorState.Closed) || (DoorInfo.DoorState == DoorState.Closing))
            {
                if (Time.time > _doorTime)
                {
                    _doorTime = Time.time + 4;
                    //Network.Enqueue(new C.Opendoor() { DoorIndex = DoorInfo.index });
                }

                return false;
            }
            if ((DoorInfo.DoorState == DoorState.Open) && (DoorInfo.LastTick + 4 > Time.time))
            {
                if (Time.time > _doorTime)
                {
                    _doorTime = Time.time + 4;
                    //Network.Enqueue(new C.Opendoor() { DoorIndex = DoorInfo.index });
                }
            }
            return true;
        }

        public Door GetDoor(byte Index)
        {
            for (int i = 0; i < mDoorList.Count; i++)
            {
                if (mDoorList[i].index == Index)
                    return mDoorList[i];
            }
            return null;
        }

        //----------------------------------------------ÍøÂçÏûÏ¢----------------------------------------------------
        public void HandleServerLocation(uint ObjectID, Vector3Int Location, MirDirection dir)
        {
            foreach (var v in mPlayerList)
            {

            }
        }

    }
}
