using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace xk_System
{
    public class MainThreadEventMgr : SingleTonMonoBehaviour<MainThreadEventMgr>
	{
		readonly ConcurrentDictionary<int, Action<object>> mEventDic = new ConcurrentDictionary<int,Action<object>>();
		readonly ConcurrentQueue<EventData> mEventDataQueue = new ConcurrentQueue<EventData> ();

		private struct EventData
		{
			public int eventId;
			public object data;
		}

		public void AddFunListen(int eventId, Action<object> mEventFunc)
		{
			if (!mEventDic.ContainsKey(eventId))
			{
                mEventDic.TryAdd(eventId, mEventFunc);
            }
			else
			{
				mEventDic[eventId] += mEventFunc;
			}
        }

		public void RemoveFunListen(int eventId, Action<object> mEventFunc)
		{
			if (mEventDic.ContainsKey(eventId))
			{
				mEventDic[eventId] -= mEventFunc;
			}
		}

		public void RemoveFunListen(int eventId)
		{
            mEventDic[eventId] = null;
        }

		public void DispatchEvent(int eventId, object mdata)
		{
			EventData mEventData = new EventData();
			mEventData.eventId = eventId;
			mEventData.data = mdata;
			mEventDataQueue.Enqueue (mEventData);
		}

		void Update()
		{
			while (mEventDataQueue.Count > 0)
			{
				EventData mEventData;
				if (mEventDataQueue.TryDequeue(out mEventData))
				{
					int eventId = mEventData.eventId;
					object data = mEventData.data;
					Action<object> mEventFunc;
					if (mEventDic.TryGetValue(eventId, out mEventFunc))
					{
						mEventFunc(data);
					}
				}
			}
		}

	}
}