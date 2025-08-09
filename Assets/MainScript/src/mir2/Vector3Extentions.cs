using NetProto.ShareData;
using UnityEngine;
public static class VectorIntExtentions
{
    public static void CopyFrom(this packet_data_Vector3Int mData, Vector3Int vecor3Int)
    {
        mData.X = vecor3Int.x;
        mData.Y = vecor3Int.y;
        mData.Z = vecor3Int.z;
    }

    public static Vector3Int ToVector3Int(this packet_data_Vector3Int mData)
    {
        return new Vector3Int(mData.X, mData.Y, mData.Z);
    }
}
