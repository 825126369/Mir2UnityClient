

using Net.TCP;
using Net.TCP.Client;
using NetProtocols.Login;
using ProtobufHelper;
using TcpProtocol;

public class NetClientMgr : SingleTonMonoBehaviour<NetClientMgr>
{
    public NetClient LoginServer_NetClient = null;
    public void InitLoginServerClient()
    {
        LoginServer_NetClient = new NetClient();
        LoginServer_NetClient.ConnectServer("127.0.0.1", 8000);

        LoginServer_NetClient.addNetListenFun(LoginServer_NetCommand.SC_REQUEST_LOGIN_RESULT, receive_scChat);
        LoginServer_NetClient.addNetListenFun(LoginServer_NetCommand.SC_REQUEST_LOGIN_RESULT, receive_scRequestLogin);
    }

    private void receive_scChat(ClientPeerBase clientPeer, NetPackage package)
    {
        TESTChatMessage mSendMsg = Protocol3Utility1.getData<TESTChatMessage>(package);
        clientPeer.SendNetData(TcpNetCommand.COMMAND_TESTCHAT, mSendMsg);
        mSendMsg.Reset();
        IMessagePool<TESTChatMessage>.recycle(mSendMsg);
    }

    void receive_scRequestLogin(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility1.getData<packet_sc_Login_Result>(mNetPackage);
        if (mReceiveMsg.NErrorCode == LoginServer_NetErrorCode.NoError)
        {
            UIMgr.Instance.CommonTipPoolView.Show("登录成功");
        }
        else
        {
            UIMgr.Instance.CommonDialogView.ShowOk("提示", "ServerCode: " + mReceiveMsg.NErrorCode);
        }

        mReceiveMsg.Reset();
        IMessagePool<packet_sc_Login_Result>.recycle(mReceiveMsg);
    }
}