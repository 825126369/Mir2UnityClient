using AKNet.Common;
using AKNet.Extentions.Protobuf;
using Google.Protobuf;
using Mir2;
using NetProtocols.Game;
using System.Collections.Generic;
using UnityEngine;

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
        var mSendMsg = IMessagePool<packet_cs_request_AllRoleInfo>.Pop();
        mSendMsg.NAccountId = DataCenter.Instance.nAccountId;
        mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SELECTROLE_ALL_ROLEINFO, mSendMsg);
        IMessagePool<packet_cs_request_AllRoleInfo>.recycle(mSendMsg);
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
        packet_sc_request_CreateRole_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_request_CreateRole_Result>(mNetPackage);

        UIMgr.CommonWindowLoading.Hide();
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            PrintTool.Log("创建角色 成功");
            List<packet_data_SelectRole_RoleInfo> mList = new List<packet_data_SelectRole_RoleInfo>(mReceiveMsg.MRoleList);
            mList.Sort((x, y) =>
            {
                if (x.NCreateTime < y.NCreateTime)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
            DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.bindData = mList;
            DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.DispatchEvent();
            UIMgr.Instance.CreateRoleView.gameObject.SetActive(false);
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
    }

    void receive_sc_Request_selectRole_DeleteRole_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_request_DeleteRole_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_request_DeleteRole_Result>(mNetPackage);

        UIMgr.CommonWindowLoading.Hide();
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            PrintTool.Log("删除角色 成功");
            List<packet_data_SelectRole_RoleInfo> mList = new List<packet_data_SelectRole_RoleInfo>(mReceiveMsg.MRoleList);
            mList.Sort((x, y) =>
            {
                if (x.NCreateTime < y.NCreateTime)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
            DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.bindData = mList;
            DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.DispatchEvent();
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
    }

    void receive_sc_Request_selectRole_AllRoleInfo_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_request_AllRoleInfo_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_request_AllRoleInfo_Result>(mNetPackage);

        UIMgr.CommonWindowLoading.Hide();
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            PrintTool.Log("packet_sc_request_AllRoleInfo_Result 成功");
            if (UIMgr.Instance.SelectServerView != null)
            {
                Destroy(UIMgr.Instance.SelectServerView.gameObject);
                UIMgr.Instance.SelectServerView = null;
            }

            List<packet_data_SelectRole_RoleInfo> mList = new List<packet_data_SelectRole_RoleInfo>(mReceiveMsg.MRoleList);
            mList.Sort((x, y) =>
            {
                if (x.NCreateTime < y.NCreateTime)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
            DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.bindData = mList;
            DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.DispatchEvent();

            UIMgr.Instance.Show_SelectRoleView();
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
    }

    void receive_sc_Request_StartGame_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        var mReceiveMsg = Protocol3Utility.getData<packet_sc_request_StartGame_Result>(mNetPackage);
        UIMgr.CommonWindowLoading.Hide();

        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
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
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
    }
    
    void receive_sc_UserLocation_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        var mReceiveMsg = Protocol3Utility.getData<packet_sc_UserLocation>(mNetPackage);
        Vector3Int pos = new Vector3Int(mReceiveMsg.Location.X, mReceiveMsg.Location.Y, mReceiveMsg.Location.Z);
        MirDirection Dir = (MirDirection)mReceiveMsg.Direction;
        WorldMgr.Instance.User.HandleServerLocation(pos, Dir);
    }
    
    void receive_sc_broadcast_UserLocation_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        var mReceiveMsg = Protocol3Utility.getData<packet_sc_broadcast_Location>(mNetPackage);
        Vector3Int pos = new Vector3Int(mReceiveMsg.Location.X, mReceiveMsg.Location.Y, mReceiveMsg.Location.Z);
        MirDirection Dir = (MirDirection)mReceiveMsg.Direction;
        WorldMgr.Instance.HandleServerLocation(mReceiveMsg.ObjectID, pos, Dir);

    }

}