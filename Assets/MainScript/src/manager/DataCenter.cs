using NetProtocols.SelectGate;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class DataCenter:SingleTonMonoBehaviour<DataCenter>
{
    // 当前存档时间
    public bool bMute = false;
    public int nWebTag = 0;
    public bool bReview = false;
    public int nThemeIndex = 1;
    public int nCoinCount = 0;
    public int nLevel;
    public bool bShowResurrectionUI = false;

    public readonly Regex mAccountRegex = new Regex(@"^[A-Za-z0-9]{8,20}$");
    public readonly Regex mPasswordRegex = new Regex(@"^[A-Za-z0-9]{8,20}$");
    public readonly Regex mMailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

    public readonly List<ServerItemData> mServerItemDataList = new List<ServerItemData>();
    public ServerItemData currentSelectServerItemData = null;
    public void Init()
    {
       
    }

    public void OnNetSyncServerItemList(packet_sc_ServerList_Result mReceiveMsg)
    {
        foreach (var item in mReceiveMsg.MServerInfoList)
        {
            ServerItemData itemData = mServerItemDataList.Find((x) => x.nServerId == item.NServerId);
            if (itemData == null)
            {
                itemData = new ServerItemData();
                mServerItemDataList.Add(itemData);
            }
            itemData.CopyFrom(item);
        }

        List<ServerItemData> mRemoveList = new List<ServerItemData>();
        foreach (var item in mServerItemDataList)
        {
            packet_SelectGateServerToPlayer_Data itemData = mReceiveMsg.MServerInfoList.First((x)=>x.NServerId == item.nServerId);
            if (itemData == null)
            {
                mRemoveList.Add(item);
            }
        }

        foreach(var item in mRemoveList)
        {
            mServerItemDataList.Remove(item);
        }
    }
}
