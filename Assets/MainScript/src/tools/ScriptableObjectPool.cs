using System.Collections.Generic;
using System;
using UnityEngine;

public class ScriptableObjectPool<T> where T : ScriptableObject
{
    public readonly Queue<T> pool = new Queue<T>();
    public readonly List<T> usedArray = new List<T>();
    private int nMaxCapicity = -1;

    public void Init(int nInitCount = 0)
    {
        preLoadObj(nInitCount);
    }

    private T InnerCreateItem()
    {
        T mItem = ScriptableObject.CreateInstance<T>();
        return mItem;
    }

    public void recycleObj(T obj)
    {
        int nIndex = this.usedArray.IndexOf(obj);
        PrintTool.Assert(nIndex >= 0, "recyleObj 000 Error: ", nIndex);
        this.usedArray.RemoveAt(nIndex);
        nIndex = this.usedArray.IndexOf(obj);
        PrintTool.Assert(nIndex == -1, "recyleObj 111 Error");
        this.pool.Enqueue(obj);
    }

    public void RecycleAll()
    {
        while (usedArray.Count > 0)
        {
            recycleObj(usedArray[usedArray.Count - 1]);
        }
    }

    public T popObj()
    {
        T mItem = null;
        if (this.pool.Count > 0)
        {
            mItem = this.pool.Dequeue();
        }
        else
        {
            mItem = this.InnerCreateItem();
        }
        this.usedArray.Add(mItem);
        if (this.nMaxCapicity > 0 && this.usedArray.Count + this.pool.Count > this.nMaxCapicity)
        {
            PrintTool.LogError("超出最大容量限制： ", this.nMaxCapicity);
        }
        return mItem;
    }

    public void SetMaxCapacity(int nMaxCapacity)
    {
        this.nMaxCapicity = nMaxCapacity;
    }

    public int GetSumCount()
    {
        return this.usedArray.Count + this.pool.Count;
    }

    public void preLoadObj(int nMaxCount)
    {
        int nNowCount = this.GetSumCount();
        for (int j = nNowCount; j < nMaxCount; j++)
        {
            this.pool.Enqueue(this.InnerCreateItem());
        }
    }

    public void preLoadObj(int nFrameCount, int nSumCount, Action finishFunc = null)
    {
        Action mFinishFunc = finishFunc;
        int nCreateCountSingle = Mathf.CeilToInt(nSumCount / nFrameCount);

        Action preLoadInnerFunc = () =>
        {
            for (int j = 0; j < nCreateCountSingle; j++)
            {
                if (this.GetSumCount() >= nSumCount)
                {
                    if (mFinishFunc != null)
                    {
                        mFinishFunc();
                        mFinishFunc = null;
                    }
                    break;
                }
                this.pool.Enqueue(this.InnerCreateItem());
            }
        };

        Timer mTimer = Timer.New(preLoadInnerFunc, 1 / 60f, nFrameCount);
        mTimer.Start();
    }
}
