using NetProtocols.Game;
using System.Collections.Generic;
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
        mNetClient.addNetListenFun(NetProtocolCommand.GC_INNER_SERVER_NET_ERROR, receive_gc_innerServer_Error_Result);
        mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_SELECTROLE_ALL_ROLEINFO_RESULT, receive_sc_Request_selectRole_AllRoleInfo_Result);
        mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_SELECTROLE_CREATE_ROLE_RESULT, receive_sc_Request_selectRole_CreateRole_Result);
        mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_SELECTROLE_DELETE_ROLE_RESULT, receive_sc_Request_selectRole_DeleteRole_Result);

        if (IPAddressHelper.TryParseConnectStr(mData.ServerConnectStr, out string Ip, out ushort nPort))
        {
            mNetClient.ConnectServer(Ip, nPort);
        }
    }

    private void Start()
    {
        var mSendMsg = IMessagePool<packet_cs_request_AllRoleInfo>.Pop();
        mSendMsg.NAccountId = DataCenter.Instance.nAccountId;
        mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SELECTROLE_ALL_ROLEINFO, mSendMsg);
        IMessagePool<packet_cs_request_AllRoleInfo>.recycle(mSendMsg);
    }

    private void Update()
    {
        mNetClient.Update(Time.deltaTime);
    }

    void receive_gc_innerServer_Error_Result(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        string msg = $"内部服务器 网络故障!!!";
        UIMgr.Instance.CommonDialogView.ShowOk("提示", msg);
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
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
    }

}