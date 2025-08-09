using AKNet.Common;
using AKNet.Extentions.Protobuf;
using Google.Protobuf;
using Mir2;
using System.Linq;
using UnityEngine;
using S = NetProto.SCPacket;

public class NetClientGameMgr : SingleTonMonoBehaviour<NetClientGameMgr>
{
    public static CustomNetClientMain mNetClient = new CustomNetClientMain();
    private bool bInit = false;
    public void Init()
    {
        if (!bInit)
        {
            bInit = true;
            mNetClient.addListenClientPeerStateFunc(ListenClientPeerState);
            mNetClient.addNetListenFunc(NetProtocolCommand.GC_INNER_SERVER_NET_ERROR, receive_gc_innerServer_Error_Result);
            mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_SELECTROLE_ALL_ROLEINFO_RESULT, receive_sc_Request_selectRole_AllRoleInfo_Result);
            mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_SELECTROLE_CREATE_ROLE_RESULT, receive_sc_Request_selectRole_CreateRole_Result);
            mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_SELECTROLE_DELETE_ROLE_RESULT, receive_sc_Request_selectRole_DeleteRole_Result);
            mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_STARTGAME_RESULT, receive_sc_Request_StartGame_Result);
            mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_USER_LOCATION, receive_sc_UserLocation_Result);
            mNetClient.addNetListenFunc(NetProtocolCommand.SC_BROADCAST_LOCATION, receive_sc_broadcast_UserLocation_Result);
        }

        ServerItemData mData = DataCenter.Instance.currentSelectServerItemData;
        if (IPAddressHelper.TryParseConnectStr(mData.ServerConnectStr, out string Ip, out ushort nPort))
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
        //var mSendMsg = new packet_cs_request_AllRoleInfo();
        //mSendMsg.NAccountId = DataCenter.Instance.nAccountId;
        //mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SELECTROLE_ALL_ROLEINFO, mSendMsg);
    }

    private void Update()
    {
        mNetClient.Update(Time.deltaTime);
    }

    public static void SendNetData(ushort nPackageId, IMessage msg)
    {
        mNetClient.SendNetData(nPackageId, msg);
    }

    void receive_gc_innerServer_Error_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        string msg = $"内部服务器 网络故障!!!";
        UIMgr.CommonDialogView.ShowOk("提示", msg);
        UIMgr.CommonWindowLoading.Hide();
    }

    void receive_sc_Request_selectRole_CreateRole_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        S.packet_sc_NewCharacterSuccess mReceiveMsg = S.packet_sc_NewCharacterSuccess.Parser.ParseFrom(mNetPackage.GetData());
        PrintTool.Log("创建角色 成功");
        SelectRoleModel.Instance.m_packet_data_SelectInfo_List.Add(mReceiveMsg.CharInfo);
        EventMgr.Instance.Broadcast(GameEvent.packet_sc_NewCharacter);
    }

    void receive_sc_Request_selectRole_DeleteRole_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        S.packet_sc_DeleteCharacter mReceiveMsg = S.packet_sc_DeleteCharacter.Parser.ParseFrom(mNetPackage.GetData());
        UIMgr.CommonWindowLoading.Hide();

        if (mReceiveMsg.Result == NetErrorCode.NoError)
        {
            PrintTool.Log("删除角色 成功");
            EventMgr.Instance.Broadcast(GameEvent.packet_sc_DeleteCharacter);
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.Result);
        }
    }

    void receive_sc_Request_selectRole_AllRoleInfo_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        S.packet_sc_request_AllRoleInfo mReceiveMsg = S.packet_sc_request_AllRoleInfo.Parser.ParseFrom(mNetPackage.GetData());
        UIMgr.CommonWindowLoading.Hide();
        SelectRoleModel.Instance.m_packet_data_SelectInfo_List =  mReceiveMsg.Characters.ToList();
        EventMgr.Instance.Broadcast(GameEvent.packet_sc_request_AllRoleInfo);
    }

    void receive_sc_Request_StartGame_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        var mReceiveMsg = S.packet_sc_StartGame.Parser.ParseFrom(mNetPackage.GetData());
        UIMgr.CommonWindowLoading.Hide();

        if (mReceiveMsg.Result == NetErrorCode.NoError)
        {
            PrintTool.Log("开始游戏 成功");
            if (UIMgr.Instance.SelectRoleView != null)
            {
                Destroy(UIMgr.Instance.SelectRoleView.gameObject);
                UIMgr.Instance.SelectRoleView = null;
            }
            if (UIMgr.Instance.CreateRoleView != null)
            {
                Destroy(UIMgr.Instance.CreateRoleView.gameObject);
                UIMgr.Instance.CreateRoleView = null;
            }

            DataCenter.Instance.InitStartGameData(mReceiveMsg);
            SceneMgr.Instance.LoadSceneAsync(SceneNames.Game, (fProgress) =>
            {
                
            }, () =>
            {
                UIMgr.Instance.Show_MainUI();
            });
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.Result);
        }
    }
    
    void receive_sc_UserLocation_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        var mReceiveMsg = S.packet_sc_UserLocation.Parser.ParseFrom(mNetPackage.GetData());
        //Vector3Int pos = new Vector3Int(mReceiveMsg.Location.X, mReceiveMsg.Location.Y, mReceiveMsg.Location.Z);
        //MirDirection Dir = (MirDirection)mReceiveMsg.Direction;
        //WorldMgr.Instance.User.HandleServerLocation(pos, Dir);
    }
    
    void receive_sc_broadcast_UserLocation_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        var mReceiveMsg = S.packet_sc_UserLocation.Parser.ParseFrom(mNetPackage.GetData());
        //Vector3Int pos = new Vector3Int(mReceiveMsg.Location.X, mReceiveMsg.Location.Y, mReceiveMsg.Location.Z);
        //MirDirection Dir = (MirDirection)mReceiveMsg.Direction;
        //WorldMgr.Instance.HandleServerLocation(mReceiveMsg.ObjectID, pos, Dir);

    }

}