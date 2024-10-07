using UnityEngine;

namespace Net.TCP.Client
{
    public class ObjectPoolManager
	{
		public ObjectPool<NetPackage> mPackagePool = null;

		public ObjectPoolManager()
		{
			mPackagePool = new ObjectPool<NetPackage>();
		}

		public void CheckPackageCount()
		{
			Debug.LogWarning("Client mUdpFixedSizePackagePool: " + mPackagePool.Count());
		}
	}
}