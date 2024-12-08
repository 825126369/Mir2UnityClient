using AKNet.Common;
using AKNet.Extentions.Protobuf;
using Google.Protobuf;
using NetProtocols.Login;
using UnityEngine;

public class NetClientLoginMgr : SingleTonMonoBehaviour<NetClientLoginMgr>
{
    public static CustomNetClientMain mNetClient = new CustomNetClientMain();
    private bool bInit = false;

    public void InitLoginServerClient()
    {
        if (bInit) return;
        bInit = true;
        mNetClient.addListenClientPeerStateFunc(ListenClientPeerState);
        mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_LOGIN_RESULT, receive_scRequestLogin);
        mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_CHANGE_PASSWORD_RESULT, receive_scChangePassword);
        mNetClient.addNetListenFunc(NetProtocolCommand.SC_REQUEST_REGISTER_RESULT, receive_scRequestRegister);

        if (IPAddressHelper.TryParseConnectStr(DataCenter.LoginServerConnectStr, out string Ip, out ushort nPort))
        {
            mNetClient.ConnectServer(Ip, nPort);
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
        }
    }

    public void Release()
    {
        mNetClient.Release();
        mNetClient = null;
        Destroy(this.gameObject);
    }

    private void Update()
    {
        mNetClient.Update(Time.deltaTime);
    }

    public static void SendNetData(ushort nPackageId, IMessage msg)
    {
        mNetClient.SendNetData(nPackageId, msg);
    }

    void receive_scRequestLogin(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);

        UIMgr.CommonWindowLoading.Hide();
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            UIMgr.CommonTipPoolView.Show("登录成功");
            Release();

            DataCenter.Instance.selectGateServerConnectStr = mReceiveMsg.SelectGateServerConnectStr;
            DataCenter.Instance.nAccountId = mReceiveMsg.NAccountId;
            NetClientSelectServerMgr.Instance.Init();
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }

        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }

    void receive_scRequestRegister(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);

        UIMgr.CommonWindowLoading.Hide();
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            UIMgr.CommonTipPoolView.Show("注册账号成功");
            UIMgr.Instance.RegisterView.Hide();
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        
        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }

    void receive_scChangePassword(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);

        UIMgr.CommonWindowLoading.Hide();
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            UIMgr.CommonTipPoolView.Show("修改密码成功");
            UIMgr.Instance.ChangePasswordView.Hide();
        }
        else
        {
            UIMgr.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }
}