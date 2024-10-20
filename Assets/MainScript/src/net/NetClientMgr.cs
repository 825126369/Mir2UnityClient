using NetProtocols.Login;
using UnityEngine;
using XKNet.Common;
using XKNet.Tcp.Client;

public class NetClientMgr : SingleTonMonoBehaviour<NetClientMgr>
{
    public TcpNetClientMain LoginServer_NetClient = null;
    public void InitLoginServerClient()
    {
        LoginServer_NetClient = new TcpNetClientMain();
        LoginServer_NetClient.ConnectServer("127.0.0.1", 9000);
        LoginServer_NetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_LOGIN_RESULT, receive_scRequestLogin);
        LoginServer_NetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_CHANGE_PASSWORD_RESULT, receive_scChangePassword);
        LoginServer_NetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_REGISTER_RESULT, receive_scRequestRegister);
    }

    private void Update()
    {
        LoginServer_NetClient.Update(Time.deltaTime);
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
            }

            UIMgr.Instance.Show_SelectView();
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }

        //mReceiveMsg.Reset();
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

        mReceiveMsg.Reset();
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

        mReceiveMsg.Reset();
        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }
}