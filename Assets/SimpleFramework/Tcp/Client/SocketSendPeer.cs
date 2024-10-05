using System;
using Google.Protobuf;

namespace xk_System.Net.TCP.Client
{
    public class SocketSendPeer : TcpSocket
	{
		public SocketSendPeer()
        {
			
		}

		public override void SendNetData(UInt16 nPackageId, IMessage data = null)
		{
			if (mSocketPeerState == SOCKETPEERSTATE.CONNECTED)
			{
				if (data == null)
				{
					ArraySegment<byte> mBufferSegment = NetPackageEncryption.Encryption(nPackageId, null);
					SendNetStream(mBufferSegment);
				}
				else
				{
					Span<byte> stream = Protocol3Utility1.SerializePackage(data);
					ArraySegment<byte> mBufferSegment = NetPackageEncryption.Encryption(nPackageId, stream);
					SendNetStream(mBufferSegment);
				}
			}
		}

		public override void SendLuaNetData(UInt16 nPackageId, byte[] buffer = null)
		{
			if (mSocketPeerState == SOCKETPEERSTATE.CONNECTED)
			{
				ArraySegment<byte> mBufferSegment = NetPackageEncryption.Encryption(nPackageId, buffer);
				SendNetStream(mBufferSegment);
			}
		}
	}
}
