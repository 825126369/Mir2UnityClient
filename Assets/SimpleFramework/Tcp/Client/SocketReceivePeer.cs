using System;
using UnityEngine;
using xk_System.DataStructure;

namespace xk_System.Net.TCP.Client
{
    //和线程打交道
    public class SocketReceivePeer : ClientPeerBase
	{
		protected CircularBuffer<byte> mReceiveStreamList = null;
		protected PackageManager mPackageManager = null;
		protected ObjectPoolManager mObjectPoolManager = null;

		private object lock_mReceiveStreamList_object = new object();

		public SocketReceivePeer()
		{
			mObjectPoolManager = new ObjectPoolManager();
			mPackageManager = new PackageManager();
			mReceiveStreamList = new CircularBuffer<byte>(Config.nBufferInitLength);
		}

		public void addNetListenFun(ushort nPackageId, Action<ClientPeerBase, NetPackage> fun)
		{
			mPackageManager.addNetListenFun(nPackageId, fun);
		}

		public void removeNetListenFun(ushort nPackageId, Action<ClientPeerBase, NetPackage> fun)
		{
			mPackageManager.removeNetListenFun(nPackageId, fun);
		}

		public override void Update(double elapsed)
		{
			base.Update(elapsed);

			switch (mSocketPeerState)
			{
				case SOCKETPEERSTATE.CONNECTED:
					int nPackageCount = 0;

					while (NetPackageExecute())
					{
						nPackageCount++;
					}

					if (nPackageCount > 0)
					{
						ReceiveHeartBeat(); 
					}

					if (nPackageCount > 50)
					{
						Debug.LogWarning("Client 处理逻辑包的数量： " + nPackageCount);
					}

					break;
				default:
					break;
			}
		}

		protected void ReceiveSocketStream(ArraySegment<byte> readOnlySpan)
		{
			lock (lock_mReceiveStreamList_object)
			{
				if (!mReceiveStreamList.isCanWriteFrom(readOnlySpan.Count))
				{
					CircularBuffer<byte> mOldBuffer = mReceiveStreamList;

					int newSize = mOldBuffer.Capacity * 2;
					while (newSize < mOldBuffer.Length + readOnlySpan.Count)
					{
						newSize *= 2;
					}

					mReceiveStreamList = new CircularBuffer<byte>(newSize);
					mReceiveStreamList.WriteFrom(mOldBuffer, mOldBuffer.Length);

					Debug.LogWarning("mReceiveStreamList Size: " + mReceiveStreamList.Capacity);
				}

				mReceiveStreamList.WriteFrom(readOnlySpan.Array, readOnlySpan.Offset, readOnlySpan.Count);
			}
		}

		private bool NetPackageExecute()
		{
			NetPackage mNetPackage = null;
			bool bSuccess = false;

			lock (lock_mReceiveStreamList_object)
			{
				if (mReceiveStreamList.Length > 0)
				{
					mNetPackage = mObjectPoolManager.mPackagePool.popObj();
					bSuccess = NetPackageEncryption.DeEncryption(mReceiveStreamList, mNetPackage);
				}
			}

			if (bSuccess)
			{
				mPackageManager.NetPackageExecute(this, mNetPackage);
			}

			if (mNetPackage != null)
			{
				mObjectPoolManager.mPackagePool.recycle(mNetPackage);
			}

			return bSuccess;
		}

		public override void Reset()
		{
			base.Reset();
			lock (mReceiveStreamList)
			{
				mReceiveStreamList.reset();
			}
		}

		public override void Release()
		{
			base.Release();
		}
	}
}
