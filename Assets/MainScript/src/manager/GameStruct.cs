using NetProtocols.SelectGate;

public class ServerItemData
{
    public uint nServerId;
    public string ServerName;
    public string ServerConnectStr;
    public uint nState;

    public void CopyFrom(packet_SelectGateServerToPlayer_Data data)
    {
        this.nServerId = data.NServerId;
        this.ServerName = data.ServerName;
        this.ServerConnectStr = data.ServerConnectStr;
        this.nState = data.NState;
    }
}
