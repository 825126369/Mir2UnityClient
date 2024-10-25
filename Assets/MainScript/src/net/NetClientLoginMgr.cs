using NetProtocols.Login;
using UnityEngine;
using XKNet.Common;
using XKNet.Tcp.Client;

public class NetClientLoginMgr : SingleTonMonoBehaviour<NetClientLoginMgr>
{
    public TcpNetClientMain LoginServer_NetClient = null;
    public void InitLoginServerClient()
    {
        LoginServer_NetClient = new TcpNetClientMain();
        LoginServer_NetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_LOGIN_RESULT, receive_scRequestLogin);
        LoginServer_NetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_CHANGE_PASSWORD_RESULT, receive_scChangePassword);
        LoginServer_NetClient.addNetListenFun(NetProtocolCommand.SC_REQUEST_REGISTER_RESULT, receive_scRequestRegister);

        if (IPAddressHelper.TryParseConnectStr(DataCenter.LoginServerConnectStr, out string Ip, out ushort nPort))
        {
            LoginServer_NetClient.ConnectServer(Ip, nPort);
        }
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
            UIMgr.Instance.CommonTipPoolView.Show("��¼�ɹ�");
            if (UIMgr.Instance.LoginView != null)
            {
                Destroy(UIMgr.Instance.LoginView.gameObject);
                UIMgr.Instance.LoginView = null;

                DataCenter.Instance.selectGateServerConnectStr = mReceiveMsg.SelectGateServerConnectStr;
                DataCenter.Instance.nAccountId = mReceiveMsg.NAccountId;
                NetClientSelectServerMgr.Instance.Init();
            }
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("��ʾ", "ServerCode: " + mReceiveMsg.NErrorCode);
        }

        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }

    void receive_scRequestRegister(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            UIMgr.Instance.CommonTipPoolView.Show("ע���˺ųɹ�");
            UIMgr.Instance.RegisterView.Hide();
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("��ʾ", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        
        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }

    void receive_scChangePassword(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == NetErrorCode.NoError)
        {
            UIMgr.Instance.CommonTipPoolView.Show("�޸�����ɹ�");
            UIMgr.Instance.ChangePasswordView.Hide();
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("��ʾ", "ServerCode: " + mReceiveMsg.NErrorCode);
        }
        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }
}