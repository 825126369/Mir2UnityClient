using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Google.Protobuf;

namespace ProtobufHelper
{
	public static class MessageParserPool<T> where T : class, IMessage, IMessage<T>, new()
	{
		static ConcurrentQueue<MessageParser<T>> mObjectPool = new ConcurrentQueue<MessageParser<T>>();

		public static int Count()
		{
			return mObjectPool.Count;
		}

		public static MessageParser<T> Pop()
		{
			MessageParser<T> t = null;
			if (!mObjectPool.TryDequeue(out t))
			{
				t = new MessageParser<T>(() => IMessagePool<T>.Pop());
			}

			return t;
		}

		public static void recycle(MessageParser<T> t)
		{
			mObjectPool.Enqueue(t);
		}

		public static void release()
		{
			
		}
	}

	public static class IMessagePool<T> where T : class, IMessage, IMessage<T>, new()
	{
		static ConcurrentQueue<T> mObjectPool = new ConcurrentQueue<T>();

		public static int Count()
		{
			return mObjectPool.Count;
		}

		public static T Pop()
		{
			T t = null;
			if (!mObjectPool.TryDequeue(out t))
			{
				t = new T();
			}

			return t;
		}

		public static void recycle(T t)
		{
			mObjectPool.Enqueue(t);
		}

		public static void release()
		{

		}
	}
}
