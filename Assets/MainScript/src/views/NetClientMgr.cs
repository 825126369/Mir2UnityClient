

using Net.TCP.Client;

public class NetClientMgr : SingleTonMonoBehaviour<NetClientMgr>
{
    public NetClient mNetClient = null;
    public void Init()
    {
        mNetClient = new NetClient();
        mNetClient.ConnectServer("127.0.0.1", 8000);
    }
}