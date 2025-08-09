using AKNet.Common;
using AKNet.Extentions.Protobuf;
using AKNet.Tcp.Client;
using Google.Protobuf;
using NetProtocols.SelectGate;
using UnityEngine;

public class NetClientSelectServerMgr : SingleTonMonoBehaviour<NetClientSelectServerMgr>
{
    public static CustomNetClientMain mNetClient = new CustomNetClientMain();
    private bool bInit = false;
    public void Init()
    {
        if (bInit) return;
        bInit = true;

        mNetClient.addListenClientPeerStateFunc(ListenClientPeerState);
        mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_SERVER_LIST_RESULT, receive_scServerList);
        if (IPAddressHelper.TryParseConnectStr(DataCenter.Instance.selectGateServerConnectStr, out string Ip, out ushort nPort))
        {
            mNetClient.ConnectServer(Ip, nPort);
        }
    }

    public void Release()
    {
        if (mNetClient != null)
        {
            mNetClient.Release();
            mNetClient = null;
            Destroy(this.gameObject);
        }
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
        mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SERVER_LIST);
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
        packet_sc_ServerList_Result mReceiveMsg = packet_sc_ServerList_Result.Parser.ParseFrom(mNetPackage.GetData());

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
    }

}