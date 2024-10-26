using NetProtocols.Login;
using UnityEngine;
using XKNet.Common;
using XKNet.Tcp.Client;

public class NetClientLoginMgr : SingleTonMonoBehaviour<NetClientLoginMgr>
{
    public readonly TcpNetClientMain mNetClient = new TcpNetClientMain();
    private bool bInit = false;

    public void InitLoginServerClient()
    {
        if (bInit) return;
        bInit = true;

        mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_LOGIN_RESULT, receive_scRequestLogin);
        mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_CHANGE_PASSWORD_RESULT, receive_scChangePassword);
        mNetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_REGISTER_RESULT, receive_scRequestRegister);

        if (IPAddressHelper.TryParseConnectStr(DataCenter.LoginServerConnectStr, out string Ip, out ushort nPort))
        {
            mNetClient.ConnectServer(Ip, nPort);
        }
    }

    public void Release()
    {
        mNetClient.Release();
        Destroy(this.gameObject);
    }

    private void Update()
    {
        mNetClient.Update(Time.deltaTime);
    }

    void receive_scRequestLogin(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            UIMgr.Instance.CommonTipPoolView.Show("登录成功");
            if (UIMgr.Instance.LoginView != null)
            {
                Destroy(UIMgr.Instance.LoginView.gameObject);
                UIMgr.Instance.LoginView = null;

                DataCenter.Instance.selectGateServerConnectStr = mReceiveMsg.SelectGateServerConnectStr;
                DataCenter.Instance.nAccountId = mReceiveMsg.NAccountId;
                NetClientSelectServerMgr.Instance.Init();
                this.mNetClient.Release();
            }
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }

        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }

    void receive_scRequestRegister(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            UIMgr.Instance.CommonTipPoolView.Show("注册账号成功");
            UIMgr.Instance.RegisterView.Hide();
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        
        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }

    void receive_scChangePassword(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            UIMgr.Instance.CommonTipPoolView.Show("修改密码成功");
            UIMgr.Instance.ChangePasswordView.Hide();
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }
}