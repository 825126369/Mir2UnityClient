using NetProtocols.Login;
using TcpProtocol;

public static class IMessageExtention
{
    public static void Reset(this TESTChatMessage message)
    {
        message.Id = 0;
        message.TalkMsg = string.Empty;
    }

    public static void Reset(this packet_cs_Login message)
    {
        message.Account = string.Empty;
        message.Password = string.Empty;
        message.NLoginType = 0;
    }

    public static void Reset(this packet_sc_Login_Result message)
    {
        message.NErrorCode = 0;
        message.GateServerIp = string.Empty;
    }
}
