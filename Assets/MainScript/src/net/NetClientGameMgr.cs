using NetProtocols.Game;
using UnityEngine;
using XKNet.Common;
using XKNet.Tcp.Client;

public class NetClientGameMgr : SingleTonMonoBehaviour<NetClientGameMgr>
{
    public TcpNetClientMain mNetClient = null;
    public void Init()
    {
        mNetClient = new TcpNetClientMain();
        mNetClient.ConnectServer("127.0.0.1", 9100);
        //mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_SERVER_LIST_RESULT, receive_scServerList);
    }

    private void Start()
    {
       // var mSendMsg = IMessagePool<packet_cs_request_ServerList>.Pop();
       // mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SERVER_LIST, mSendMsg);
        //IMessagePool<packet_cs_request_ServerList>.recycle(mSendMsg);
    }

    private void Update()
    {
        mNetClient.Update(Time.deltaTime);
    }

    void receive_scServerList(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        //packet_sc_ServerList_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_ServerList_Result>(mNetPackage);
        //if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        //{
        //    PrintTool.Log("receive_scServerList 成功");
        //    DataCenter.Instance.OnNetSyncServerItemList(mReceiveMsg);
        //    UIMgr.Instance.Show_SelectServerView();
        //}
        //else
        //{
        //    UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        //}
        //IMessagePool<packet_sc_ServerList_Result>.recycle(mReceiveMsg);
    }

}