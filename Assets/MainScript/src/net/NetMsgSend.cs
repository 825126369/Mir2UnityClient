
using NetProtocols.Game;

namespace Mir2
{

    public static class NetMsgSend
    {
        public static void SendTurnDirMsg(MirDirection Direction)
        {
            var mSendMsg = new packet_cs_request_TurnDir();
            mSendMsg.Direction = (uint)Direction;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_TURNDIR, mSendMsg);
        }

        public static void SendWalkMsg(MirDirection Direction)
        {
            var mSendMsg = new packet_cs_request_Walk();
            mSendMsg.Direction = (uint)Direction;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_WALK, mSendMsg);
        }

        public static void SendRunMsg(MirDirection Direction)
        {
            var mSendMsg = new packet_cs_request_Run();
            mSendMsg.Direction = (uint)Direction;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_RUN, mSendMsg);
        }
    }
}