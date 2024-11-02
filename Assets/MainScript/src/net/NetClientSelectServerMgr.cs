using AKNet.Common;
using AKNet.Tcp.Client;
using NetProtocols.SelectGate;
using UnityEngine;

public class NetClientSelectServerMgr : SingleTonMonoBehaviour<NetClientSelectServerMgr>
{
    public readonly TcpNetClientMain mNetClient = new TcpNetClientMain();
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
        Destroy(this.gameObject);
    }

    private void ListenClientPeerState(ClientPeerBase mClientPeer)
    {
        if (mClientPeer.GetSocketState() == SOCKET_PEER_STATE.CONNECTED)
        {
            SendFirstMsg();
        }
    }

    private void SendFirstMsg()
    {
        var mSendMsg = IMessagePool<packet_cs_request_ServerList>.Pop();
        mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SERVER_LIST, mSendMsg);
        IMessagePool<packet_cs_request_ServerList>.recycle(mSendMsg);
    }

    private void Update()
    {
        mNetClient.Update(Time.deltaTime);
    }

    void receive_scServerList(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_ServerList_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_ServerList_Result>(mNetPackage);
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
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        IMessagePool<packet_sc_ServerList_Result>.recycle(mReceiveMsg);
    }

}