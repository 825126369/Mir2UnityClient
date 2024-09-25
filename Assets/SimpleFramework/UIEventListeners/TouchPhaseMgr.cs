using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPhaseMgr : SingleTonMonoBehaviour<TouchPhaseMgr>
{
    private Vector2 startPos;
    private Vector2 offsetPos;
    private readonly Dictionary<TouchPhase, Action> mEventDic = new Dictionary<TouchPhase, Action>();

    public void AddEvent(TouchPhase nTouchPhase, Action mEvent)
    {
        Action mAction = null;
        if(mEventDic.TryGetValue(nTouchPhase, out mAction))
        {
            mAction += mEvent;
        }
        else
        {
            mEventDic[nTouchPhase] = mEvent;
        }
    }

    public void RemoveEvent(TouchPhase nTouchPhase, Action mEvent)
    {
        Action mAction = null;
        if(mEventDic.TryGetValue(nTouchPhase, out mAction))
        {
            mAction -= mEvent;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    break;
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    offsetPos = touch.position - startPos;
                    break;
                case TouchPhase.Ended:
                    break;
            }
            
            Debug.Log(touch.phase);
            Action mAction = null;
            if(mEventDic.TryGetValue(touch.phase, out mAction))
            {
                mAction();
            }
        }
    }
}
