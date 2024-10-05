using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Object 池子
namespace xk_System.Net.TCP.Client
{
	public class ObjectPool<T> where T : class, new()
	{
		readonly Queue<T> mObjectPool = new Queue<T>();
		readonly List<T> mUsedObjectPool = new List<T>();

		public int Count()
		{
			return mObjectPool.Count;
		}

		public void initObj(int count)
		{
			for (int i = 0; i < count; i++)
			{
				T t = new T();
				mObjectPool.Enqueue(t);
			}
		}

		public T popObj()
		{
			T mItem = null;
			if (mObjectPool.Count > 0)
			{
				mItem = mObjectPool.Dequeue();
			}
			else
			{
				mItem = new T();
			}

			if (GameLauncher.Instance.m_PrintLog)
			{
				PrintTool.Assert(!mUsedObjectPool.Contains(mItem));
			}

			mUsedObjectPool.Add(mItem);
			return mItem;
		}

		public void recycleAllObj()
		{
			for (int i = 0; i < mUsedObjectPool.Count; i++)
			{
				mObjectPool.Enqueue(mUsedObjectPool[i]);
			}
			mUsedObjectPool.Clear();
		}

		public void recycle(T t)
		{
			mObjectPool.Enqueue(t);
		}
    }
}

	