using UnityEngine;

namespace xk_System.Net.TCP.Client
{
	public class NetClient : ClientPeer
	{
        public override void Update(double elapsed)
        {
            if (elapsed >= 0.3)
            {
                Debug.LogWarning("NetClient 帧 时间 太长: " + elapsed);
            }

            base.Update(elapsed);
        }
    }
}