using NetProto.ShareData;

public class ItemData
{
    public uint nItemId;
    public uint nCount;

    public uint nBagIndex;
    public uint nSlotIndex;
    public uint nStarLevel;
    public uint nDura;

    public void CopyFrom(packet_data_ItemInfo ItemInfo)
    {
        //this.nItemId = ItemInfo.;
        //this.nCount = ItemInfo.NCount;
        //this.nBagIndex = (uint)ItemInfo.NBagIndex;
        //this.nSlotIndex = (uint)ItemInfo.NSlotIndex;
        //this.nStarLevel = ItemInfo.NStarLevel;
        //this.nDura = ItemInfo.NDura;
    }

}