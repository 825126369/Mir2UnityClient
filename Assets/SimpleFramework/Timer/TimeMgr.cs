using System;
using UnityEngine;

public class TimeOutGenerator
{
    float fLastUpdateTime = 0;
    float fInternalTime = 0;
    public static TimeOutGenerator New(float fInternalTime)
    {
        var temp = new TimeOutGenerator();
        temp.Init(fInternalTime);
        return temp;
    }

    public void Init(float fInternalTime = 1.0f)
    {
        this.fInternalTime = fInternalTime;
        this.Reset();
    }

    public void Reset()
    {
        this.fLastUpdateTime = Time.time;
    }

    public bool orTimeOut()
    {
        if ((Time.time - fLastUpdateTime) > fInternalTime)
        {
            this.Reset();
            return true;
        }

        return false;
    }

    public bool orTimeOutWithSpecialTime(float fInternalTime)
    {
        if (Time.time - fLastUpdateTime > fInternalTime)
        {
            this.Reset();
            return true;
        }

        return false;
    }
}

//-------------------------------------------------------------------------------------

public class TimeMgr:SingleTonMonoBehaviour<TimeMgr>
{
    event Action mapUpdateFunc;
    
    public void Update()
    {
        if (mapUpdateFunc != null)
        {
            mapUpdateFunc();
        }
    }

    public void AddListener(Action func)
    {
        if (CheckFunIsExist(func))
        {
            PrintTool.LogError("AddListener Errir");
        }
        else
        {
            mapUpdateFunc += func;
        }
    }

    public void RemoveListener(Action func)
    {
        this.mapUpdateFunc -= func;
    }

    private bool CheckFunIsExist(Action fun)
    {
        Delegate[] mList = mapUpdateFunc.GetInvocationList();
        return Array.Exists<Delegate>(mList, (x) => x.Equals(fun));
    }
}
