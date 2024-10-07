using UnityEngine;

namespace Net.TCP.Client
{
    public class ClientPeer : SocketSendPeer
	{
		private double fReConnectServerCdTime = 0.0;
		public override void Update(double elapsed)
		{
			base.Update(elapsed);
			switch (mSocketPeerState)
			{
				case SOCKETPEERSTATE.CONNECTED:
					fSendHeartBeatTime += elapsed;
					if (fSendHeartBeatTime >= Config.fSendHeartBeatMaxTimeOut)
					{
						SendHeartBeat();
						fSendHeartBeatTime = 0.0;
					}

					fReceiveHeartBeatTime += elapsed;
					if (fReceiveHeartBeatTime >= Config.fReceiveHeartBeatMaxTimeOut)
					{
						fReceiveHeartBeatTime = 0.0;
						fReConnectServerCdTime = 0.0;
						mSocketPeerState = SOCKETPEERSTATE.RECONNECTING;
						Debug.Log("心跳 超时 ");
					}

					break;
				case SOCKETPEERSTATE.RECONNECTING:
					fReConnectServerCdTime += elapsed;
					if (fReConnectServerCdTime >= Config.fReceiveReConnectMaxTimeOut)
					{
						mSocketPeerState = SOCKETPEERSTATE.CONNECTING;
						fReConnectServerCdTime = 0.0;
						ReConnectServer();
					}
					break;
				default:
					break;
			}
		}

        public override void Reset()
        {
            base.Reset();
			fReConnectServerCdTime = 0.0f;
		}
    }
}


