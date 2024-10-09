using NetProtocols.Login;
using UnityEngine;
using XKNet.Common;
using XKNet.Tcp.Client;
using XKNet.Tcp.Common;

public class NetClientMgr : SingleTonMonoBehaviour<NetClientMgr>
{
    public TcpNetClientMain LoginServer_NetClient = null;
    public void InitLoginServerClient()
    {
        LoginServer_NetClient = new TcpNetClientMain();
        LoginServer_NetClient.ConnectServer("127.0.0.1", 9000);
        LoginServer_NetClient.addNetListenFun(LoginServer_NetCommand.SC_REQUEST_LOGIN_RESULT, receive_scRequestLogin);
    }

    private void Update()
    {
        LoginServer_NetClient.Update(Time.deltaTime);
    }

    void receive_scRequestLogin(ClientPeerBase clientPeer, NetPackage mNetPackage)
    {
        packet_sc_Login_Result mReceiveMsg = Protocol3Utility.getData<packet_sc_Login_Result>(mNetPackage);
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