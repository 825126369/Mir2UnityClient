using CrystalMir2;
using System.Collections.Generic;

public class MapData
{
    public MapReader mMapBasicInfo;
    public MapInfo mMapInfo;

    public readonly List<Door> mDoorList = new List<Door>();

    public Door GetDoor(byte Index)
    {
        for (int i = 0; i < mDoorList.Count; i++)
        {
            if (mDoorList[i].index == Index)
                return mDoorList[i];
        }
        return null;
    }
}