using NetProtocols.Game;
using UnityEngine;
using XKNet.Common;
using XKNet.Tcp.Client;

public class NetClientGameMgr : SingleTonMonoBehaviour<NetClientGameMgr>
{
    public TcpNetClientMain mNetClient = null;
    public void Init()
    {
        ServerItemData mData = DataCenter.Instance.currentSelectServerItemData;

        mNetClient = new TcpNetClientMain();
        mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_SELECTROLE_ALL_ROLEINFO_RESULT, receive_sc_Request_selectRole_AllRoleInfo_Result);
        if (IPAddressHelper.TryParseConnectStr(mData.ServerConnectStr, out string Ip, out ushort nPort))
        {
            mNetClient.ConnectServer(Ip, nPort);
        }
    }

    private void Start()
    {
        //var mSendMsg = IMessagePool<packet_cs_request_ServerList>.Pop();
        //mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SERVER_LIST, mSendMsg);
        //IMessagePool<packet_cs_request_ServerList>.recycle(mSendMsg);
    }

    private void Update()
    {
        mNetClient.Update(Time.deltaTime);
    }

    void receive_sc_Request_selectRole_CreateRole_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_request_CreateRole_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_request_CreateRole_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            PrintTool.Log("创建角色 成功");
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        IMessagePool<packet_sc_request_CreateRole_Result>.recycle(mReceiveMsg);
    }

    void receive_sc_Request_selectRole_DeleteRole_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_request_DeleteRole_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_request_DeleteRole_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            PrintTool.Log("删除角色 成功");
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        IMessagePool<packet_sc_request_DeleteRole_Result>.recycle(mReceiveMsg);
    }

    void receive_sc_Request_selectRole_AllRoleInfo_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_request_AllRoleInfo_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_request_AllRoleInfo_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            PrintTool.Log("packet_sc_request_AllRoleInfo_Result 成功");
            UIMgr.Instance.SelectRoleView.RefreshView();
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        IMessagePool<packet_sc_request_AllRoleInfo_Result>.recycle(mReceiveMsg);
    }

}