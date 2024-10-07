﻿using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Net.TCP.Client
{
    public class TcpSocket : SocketReceivePeer
	{
		private Socket mSocket = null;
		private string ServerIp = "";
		private int nServerPort = 0;
		private IPEndPoint mIPEndPoint = null;

		ReadWriteIOContextPool mReadWriteIOContextPool = null;
		SimpleIOContextPool mSimpleIOContextPool = null;
		
		CircularBuffer<byte> mSendStreamList = null;

		private object lock_mSocket_object = new object();

		SocketAsyncEventArgs mConnectIOContex = null;
		SocketAsyncEventArgs mDisConnectIOContex = null;
		SocketAsyncEventArgs mSendIOContex = null;
		SocketAsyncEventArgs mReceiveIOContex = null;

		bool bConnectIOContexUsed = false;
		bool bSendIOContexUsed = false;

		public TcpSocket()
		{
			mSocketPeerState = SOCKETPEERSTATE.NONE;
			BufferManager mBufferManager = new BufferManager(Config.nIOContexBufferLength, 2);
			mReadWriteIOContextPool = new ReadWriteIOContextPool(2, mBufferManager, OnIOCompleted);
			mSimpleIOContextPool = new SimpleIOContextPool(2, OnIOCompleted);

			mConnectIOContex = mSimpleIOContextPool.Pop();
			mDisConnectIOContex = mSimpleIOContextPool.Pop();
			mSendIOContex = mReadWriteIOContextPool.Pop();
			mReceiveIOContex = mReadWriteIOContextPool.Pop();

			mSendStreamList = new CircularBuffer<byte>(Config.nBufferInitLength);
		}

		public void ReConnectServer()
		{
			if (mSocket != null && mSocket.Connected)
			{
				mSocketPeerState = SOCKETPEERSTATE.CONNECTED;
			}
			else
			{
				ConnectServer(this.ServerIp, this.nServerPort);
			}
		}

		public void ConnectServer(string ServerAddr, int ServerPort)
		{
			this.ServerIp = ServerAddr;
			this.nServerPort = ServerPort;

			lock (lock_mSocket_object)
			{
				if (mSocket != null)
				{
					mSocket.Close();
					mSocket = null;
				}

				if (mSocket == null)
				{
					mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				}

				if (mIPEndPoint == null)
				{
					IPAddress mIPAddress = IPAddress.Parse(ServerAddr);
					mIPEndPoint = new IPEndPoint(mIPAddress, ServerPort);
				}

				Debug.Log("Client 正在连接服务器: " + this.ServerIp + " | " + this.nServerPort);
				mSocketPeerState = SOCKETPEERSTATE.CONNECTING;

				bConnectIOContexUsed = false;
				if (!bConnectIOContexUsed)
				{
					bConnectIOContexUsed = true;
					mConnectIOContex.RemoteEndPoint = mIPEndPoint;
					if (!mSocket.ConnectAsync(mConnectIOContex))
					{
						ProcessConnect(mConnectIOContex);
					}
				}
			}
		}

		public bool DisConnectServer()
		{
			Debug.Log("客户端 主动 断开服务器 Begin......");

			lock (lock_mSocket_object)
			{
				if (mSocket != null && mSocket.Connected)
				{
					mSocketPeerState = SOCKETPEERSTATE.DISCONNECTING;
					mDisConnectIOContex.RemoteEndPoint = mIPEndPoint;
					if (!mSocket.DisconnectAsync(mDisConnectIOContex))
					{
						ProcessDisconnect(mDisConnectIOContex);
					}
				}
				else
				{
					Debug.Log("客户端 主动 断开服务器 Finish......");
					mSocketPeerState = SOCKETPEERSTATE.DISCONNECTED;
				}
			}

			return mSocketPeerState == SOCKETPEERSTATE.DISCONNECTED;
		}

		private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
		{
			switch (e.LastOperation)
			{
				case SocketAsyncOperation.Connect:
					ProcessConnect(e);
					break;
				case SocketAsyncOperation.Disconnect:
					ProcessDisconnect(e);
					break;
				case SocketAsyncOperation.Receive:
					this.ProcessReceive(e);
					break;
				case SocketAsyncOperation.Send:
					this.ProcessSend(e);
					break;
				default:
					Debug.LogError("The last operation completed on the socket was not a receive or send");
					break;
			}
		}

		private void ProcessConnect(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				Debug.Log(string.Format("Client 连接服务器: {0}:{1} 成功", this.ServerIp, this.nServerPort));
				mSocketPeerState = SOCKETPEERSTATE.CONNECTED;

				if (!mSocket.ReceiveAsync(mReceiveIOContex))
				{
					ProcessReceive(mReceiveIOContex);
				}
			}
			else
			{
				Debug.Log(string.Format("Client 连接服务器: {0}:{1} 失败：{2}", this.ServerIp, this.nServerPort, e.SocketError));
				if (mSocketPeerState == SOCKETPEERSTATE.CONNECTING)
				{
					mSocketPeerState = SOCKETPEERSTATE.RECONNECTING;
				}
			}

			bConnectIOContexUsed = false;
			e.RemoteEndPoint = null;
		}

		private void ProcessDisconnect(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				mSocketPeerState = SOCKETPEERSTATE.DISCONNECTED;
				Debug.Log("客户端 主动 断开服务器 Finish");
			}
			else
			{
				DisConnectedWithException(e.SocketError);
			}

			e.RemoteEndPoint = null;
		}

		private void ProcessReceive(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				if (e.BytesTransferred > 0)
				{
					ArraySegment<byte> readOnlySpan = new ArraySegment<byte>(e.Buffer, e.Offset, e.BytesTransferred);
					ReceiveSocketStream(readOnlySpan);

					lock (lock_mSocket_object)
					{
						if (mSocket != null)
						{
							if (!mSocket.ReceiveAsync(e))
							{
								ProcessReceive(e);
							}
						}
					}
				}
				else
				{
					DisConnectedWithNormal();
				}
			}
			else
			{
				DisConnectedWithException(e.SocketError);
			}
		}

		private void ProcessSend(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				SendNetStream1(e);
			}
			else
			{
				DisConnectedWithException(e.SocketError);
				bSendIOContexUsed = false;
			}
		}

		protected void SendNetStream(ArraySegment<byte> mBufferSegment)
		{
			Debug.Assert(mBufferSegment.Count <= Config.nBufferMaxLength, "发送尺寸超出最大限制" + mBufferSegment.Count + " | " + Config.nBufferMaxLength);

			lock (mSendStreamList)
			{
				if (!mSendStreamList.isCanWriteFrom(mBufferSegment.Count))
				{
					CircularBuffer<byte> mOldBuffer = mSendStreamList;

					int newSize = mOldBuffer.Capacity * 2;
					while (newSize < mOldBuffer.Length + mBufferSegment.Count)
					{
						newSize *= 2;
					}

					mSendStreamList = new CircularBuffer<byte>(newSize);
					mSendStreamList.WriteFrom(mOldBuffer, mOldBuffer.Length);

					Debug.LogWarning("mSendStreamList Size: " + mSendStreamList.Capacity);
				}

				mSendStreamList.WriteFrom(mBufferSegment.Array, mBufferSegment.Offset, mBufferSegment.Count);
			}
			
			if (!bSendIOContexUsed)
			{
				bSendIOContexUsed = true;
				SendNetStream1(mSendIOContex);
			}
			else
			{
				Debug.LogWarning("SendIOContexArgs is Null");
			}
		}

		private void SendNetStream1(SocketAsyncEventArgs e)
		{
			bool bContinueSend = false;
			lock (mSendStreamList)
			{
				if (mSendStreamList.Length >= Config.nIOContexBufferLength)
				{
					int nLength = Config.nIOContexBufferLength;
					mSendStreamList.WriteTo(0, e.Buffer, e.Offset, nLength);
					e.SetBuffer(e.Offset, nLength);
					bContinueSend = true;

				}
				else if (mSendStreamList.Length > 0)
				{
					int nLength = mSendStreamList.Length;
					mSendStreamList.WriteTo(0, e.Buffer, e.Offset, nLength);
					e.SetBuffer(e.Offset, nLength);
					bContinueSend = true;
				}
			}

			if (bContinueSend)
			{
				lock (lock_mSocket_object)
				{
					if (mSocket != null)
					{
						if (!mSocket.SendAsync(e))
						{
							ProcessSend(e);
						}
					}
					else
					{
						bSendIOContexUsed = false;
					}
				}
			}
			else
			{
				bSendIOContexUsed = false;
			}
			
		}

		private void DisConnectedWithNormal()
		{
			Debug.Log("客户端 正常 断开服务器 ");
			Reset();
		}

		private void DisConnectedWithException(SocketError e)
		{
			Debug.Log("客户端 异常 断开服务器: " + e.ToString());
			Reset();
			if (mSocketPeerState == SOCKETPEERSTATE.DISCONNECTING)
			{
				mSocketPeerState = SOCKETPEERSTATE.DISCONNECTED;
			}
			else if (mSocketPeerState == SOCKETPEERSTATE.CONNECTED)
			{
				mSocketPeerState = SOCKETPEERSTATE.RECONNECTING;
			}
		}

		private void CloseSocket()
		{
			lock (lock_mSocket_object)
			{
				if (mSocket != null)
				{
					try
					{
						mSocket.Close();
                    }
                    catch { }

					mSocket = null;
				}
			}
		}

        public override void Reset()
        {
			base.Reset();
			lock (mSendStreamList)
			{
				mSendStreamList.reset();
			}
			CloseSocket();
		}
    }
}
