using NetProtocols.Login;
using TcpProtocol;

public static class IMessageExtention
{
    public static void Reset(this TESTChatMessage message)
    {
        message.Id = default;
        message.TalkMsg = string.Empty;
    }

    public static void Reset(this packet_cs_Login message)
    {
        message.Account = default;
        message.Password = default;
        message.NLoginType = default;
    }

    public static void Reset(this packet_sc_Login_Result message)
    {
        message.NErrorCode = default;
        message.GateServerIp = default;
        message.Port = default;
    }
}
