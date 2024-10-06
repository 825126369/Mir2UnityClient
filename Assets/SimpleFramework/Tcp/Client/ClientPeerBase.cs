﻿using Google.Protobuf;

namespace xk_System.Net.TCP.Client
{
    public class ClientPeerBase
	{
		protected double fSendHeartBeatTime = 0.0;
		protected double fReceiveHeartBeatTime = 0.0;
		protected SOCKETPEERSTATE mSocketPeerState = SOCKETPEERSTATE.NONE;

		public SOCKETPEERSTATE GetSocketState()
		{
			return mSocketPeerState;
		}

		public virtual void Update(double elapsed)
		{
			
		}

		protected void SendHeartBeat()
		{
			SendNetData(TcpNetCommand.COMMAND_HEARTBEAT);
		}

		protected void ReceiveHeartBeat()
		{
			fReceiveHeartBeatTime = 0.0;
		}

		public virtual void SendNetData(ushort nPackageId, IMessage data = null)
		{

		}

		public virtual void SendLuaNetData(ushort nPackageId, byte[] buffer = null)
        {

        }

		public virtual void Reset()
		{
			fSendHeartBeatTime = 0.0;
			fReceiveHeartBeatTime = 0.0;
		}

		public virtual void Release()
		{
			Reset();
		}
	}
}
