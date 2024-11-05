using AKNet.Common;
using AKNet.Tcp.Client;
using Google.Protobuf;
using NetProtocols.SelectGate;
using UnityEngine;

public class NetClientSelectServerMgr : SingleTonMonoBehaviour<NetClientSelectServerMgr>
{
    public static TcpNetClientMain mNetClient = new TcpNetClientMain();
    private bool bInit = false;
    public void Init()
    {
        if (bInit) return;
        bInit = true;

        mNetClient.addListenClientPeerStateFunc(ListenClientPeerState);
        mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_SERVER_LIST_RESULT, receive_scServerList);
        if (IPAddressHelper.TryParseConnectStr(DataCenter.Instance.selectGateServerConnectStr, out string Ip, out ushort nPort))
        {
            mNetClient.ConnectServer(Ip, nPort);
        }
    }

    public void Release()
    {
        mNetClient.Release();
        mNetClient = null;
        Destroy(this.gameObject);
    }

    private void ListenClientPeerState(ClientPeerBase mClientPeer)
    {
        if (mClientPeer.GetSocketState() == SOCKET_PEER_STATE.DISCONNECTED)
        {
            UIMgr.CommonDialogView.ShowYesCancel("提示", "网络已断开, 是否重连?");
        }
        else if (mClientPeer.GetSocketState() != SOCKET_PEER_STATE.CONNECTED)
        {
            UIMgr.CommonWindowLoading.Show();
        }
        else
        {
            UIMgr.CommonWindowLoading.Hide();
            SendFirstMsg();
        }
    }

    private void SendFirstMsg()
    {
        UIMgr.CommonWindowLoading.Show();
        var mSendMsg = IMessagePool<packet_cs_request_ServerList>.Pop();
        mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SERVER_LIST, mSendMsg);
        IMessagePool<packet_cs_request_ServerList>.recycle(mSendMsg);
    }

    private void Update()
    {
        mNetClient.Update(Time.deltaTime);
    }

    public static void SendNetData(ushort nPackageId, IMessage msg)
    {
        mNetClient.SendNetData(nPackageId, msg);
    }

    void receive_scServerList(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_ServerList_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_ServerList_Result>(mNetPackage);

        UIMgr.CommonWindowLoading.Hide();
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            PrintTool.Log("receive_scServerList 成功");
            if (UIMgr.Instance.LoginView != null)
            {
                Destroy(UIMgr.Instance.LoginView.gameObject);
                UIMgr.Instance.LoginView = null;
            }

            DataCenter.Instance.OnNetSyncServerItemList(mReceiveMsg);
            UIMgr.Instance.Show_SelectServerView();
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        IMessagePool<packet_sc_ServerList_Result>.recycle(mReceiveMsg);
    }

}