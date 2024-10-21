using NetProtocols.SelectGate;

public enum EServerState
{
    Normal = 0, //����
    jam = 1,//ӵ��
    Maintenance = 2, //ά��
}

public class ServerItemData
{
    public uint nServerId;
    public string ServerName;
    public string ServerConnectStr;
    public EServerState nState;

    public void CopyFrom(packet_SelectGateServerToPlayer_Data data)
    {
        this.nServerId = data.NServerId;
        this.ServerName = data.ServerName;
        this.ServerConnectStr = data.ServerConnectStr;
        this.nState = (EServerState)data.NState;
    }

    public override string ToString()
    {
        return $"nServerId: {nServerId}, ServerName: {ServerName},ServerConnectStr: {ServerConnectStr},nState: {nState}";
    }
}
