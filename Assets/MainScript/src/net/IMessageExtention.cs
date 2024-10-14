using NetProtocols.Login;
using TestProtocol;

public static partial class IMessageExtention
{
    public static void Reset(this TESTChatMessage message)
    {
        message.Id = 0;
        message.TalkMsg = string.Empty;
    }

    internal static void Reset(this packet_cs_Login message)
    {
        message.Account = string.Empty;
        message.Password = string.Empty;
        message.NLoginType = 0;
    }

    internal static void Reset(this packet_sc_Login_Result message)
    {
        message.NErrorCode = 0;
        message.GateServerIp = string.Empty;
    }
}
