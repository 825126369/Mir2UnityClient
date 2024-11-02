using AKNet.Common;
using AKNet.Tcp.Client;
using NetProtocols.Login;
using UnityEngine;

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
            UIMgr.Instance.CommonTipPoolView.Show("��¼�ɹ�");
            this.mNetClient.Release();

            DataCenter.Instance.selectGateServerConnectStr = mReceiveMsg.SelectGateServerConnectStr;
            DataCenter.Instance.nAccountId = mReceiveMsg.NAccountId;
            NetClientSelectServerMgr.Instance.Init();
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