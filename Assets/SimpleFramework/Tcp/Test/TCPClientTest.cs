using System.Collections;
using System.Collections.Generic;
using TcpProtocol;
using UnityEngine;
using XKNet.Common;
using XKNet.Tcp.Client;
using XKNet.Tcp.Common;

public class TCPClientTest : MonoBehaviour
{
	//string Ip = LuaUtils.Instance.GetInterNetIp("https://pokergameserverconsole.azurewebsites.net");
	//string Ip = "20.62.155.182";
	//string Ip = "40.73.126.219";
	//string Ip = "127.0.0.1";
	string Ip = "192.168.1.2";
	int port = 10002;
	
	public int nClientCount = 30;
	public int nPackageCount = 10;
	List<TcpNetClientMain> mClientList = new List<TcpNetClientMain>();

	private void Start()
	{
		for (int i = 0; i < nClientCount; i++)
		{
            TcpNetClientMain mNetSystem = new TcpNetClientMain();
			mNetSystem.addNetListenFun(TcpNetCommand.COMMAND_TESTCHAT, Receive_ServerSenddata);
			mClientList.Add(mNetSystem);

			mNetSystem.ConnectServer(Ip, port);
			StartCoroutine(Run(mNetSystem, i));
		}
	}

	private void Update()
	{
		for (int i = 0; i < nClientCount; i++)
		{
            TcpNetClientMain mNetSystem = mClientList[i];
			mNetSystem.Update(Time.deltaTime);
		}
	}

	private static int nSendCount = 0;
	private static int nReceiveCount = 0;

	IEnumerator Run(TcpNetClientMain mNetSystem, int nIndex)
	{
		while (true)
		{
			int TestCount = 0;
			while (TestCount < nPackageCount)
			{
				TestCount++;
				request_ClientSendData(mNetSystem, nIndex);
			}

			yield return null;
		}
	}

	public void request_ClientSendData(TcpNetClientMain mNetSystem, int channelId)
	{
		TESTChatMessage mdata = IMessagePool<TESTChatMessage>.Pop();
		mdata.Id = 0;
		mdata.TalkMsg = string.Empty;

		mdata.Id = (uint)channelId;
		if (UnityEngine.Random.Range(1, 3) == 1)
		{
			mdata.TalkMsg = "Hello World";
		}
		else
		{
			mdata.TalkMsg = "==Begin== 111111111111111111111111111111111111111111111111111111111111111111111111111dgdgsdfshsdfh,as" +
				"mfamsfdmasdfamslmdfamsd;fmamfdamfd;amsfdwsjdfasjfasjfdkjaskfdjas;ojfd;asjdfasjdfsfasdfaksfdk" +
				"safdasdfjajfdjadjsajkf;lsdjf;alsjdf;lasjdf;lajsl;fdjalsjdfa;jsdf;aj;fjda;sjdfsjfdkjsdfkjasdf" +
				"jsfdasfsdfasdfasfdasdfasdfjkasjdfassfjpojpeoi97893472941947194y913742057sdfasfdasdfasdfsfasdfasfdasd" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfasfdasdfasdfasfddddddddddddddddddddddddddddddddddddddddddddddddddfasdfasdfasdfasdfasdfsdfasdfasfda" +
				"sfsfsfsdfsd09035923-940592394523096-548623489510948920384*((*&^&%^$%$$%#sfsfd ==End==";
		}

		mNetSystem.SendNetData(TcpNetCommand.COMMAND_TESTCHAT, mdata);
		
		mdata.Id = 0;
		mdata.TalkMsg = string.Empty;
		IMessagePool<TESTChatMessage>.recycle(mdata);

		nSendCount++;
	}

	private void Receive_ServerSenddata(ClientPeerBase clientPeer, NetPackage package)
	{
        TESTChatMessage mdata = Protocol3Utility.getData<TESTChatMessage>(package);
        nReceiveCount++;
        Debug.Log("Client 接受 渠道ID " + mdata.Id + " | " + package.mBufferSegment.Count + " | " + nReceiveCount);
        IMessagePool<TESTChatMessage>.recycle(mdata);
    }

}
