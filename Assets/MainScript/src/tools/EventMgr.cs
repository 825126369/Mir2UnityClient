using System;
using System.Collections.Generic;

public  class EventMgr : Singleton<EventMgr>
{
    public class Listener
    {
        public Action<object> func;
        public bool once = false;
    }

    private readonly Dictionary<string, List<Listener>> mEventDic = new Dictionary<string, List<Listener>>();

    public void AddListener(string eventName, Action<object> func, bool once = false)
    {
        List<Listener> mListener = null;
        if (!this.mEventDic.TryGetValue(eventName, out mListener))
        {
            mListener = new List<Listener>();
            mEventDic[eventName] = mListener;
        }

        mListener.Add(new Listener()
        {
            func = func,
            once = once,
        });
    }

    public void Broadcast(string eventName, object data = null)
    {
        List<Listener> listeners = null;
        if (!this.mEventDic.TryGetValue(eventName, out listeners))
        {
            return;
        }

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            Listener ml = listeners[i];
            ml.func(data);
            if (ml.once)
            {
                RemoveListener(eventName, ml.func);
            }
        }

    }

    public void RemoveListener(string eventName, Action<object> func)
    {
        List<Listener> listeners = null;
        if (this.mEventDic.TryGetValue(eventName, out listeners))
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                if (listeners[i].func == func)
                {
                    listeners.RemoveAt(i);
                }
            }
        }
    }

}
