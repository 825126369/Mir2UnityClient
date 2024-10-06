using System;

namespace xk_System.Net.TCP.Client
{
    public class NetPackage
	{
		public ushort nPackageId = 0;
		public ArraySegment<byte> mBufferSegment;
	}
}

