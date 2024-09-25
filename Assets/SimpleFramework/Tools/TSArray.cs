using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TSArray<T> : List<T>
{
    public void push(T t)
    {
        this.Add(t);
    }

    public T pop()
    {
        T t = this[this.Count - 1];
        this.RemoveAt(this.Count - 1);
        return t;
    }

    public TSArray<T> splice(int nRemoveIndex, int nRemoveCount)
    {
        TSArray<T> m = new TSArray<T>();
        for(int i = 0; i < nRemoveCount; i++)
        {
            m.Add(this[nRemoveIndex + i]);
        }

        this.RemoveRange(nRemoveIndex, nRemoveCount);
        return m;
    }
    
    public TSArray<T> slice(int nIndex, int nCount)
    {
        TSArray<T> m = new TSArray<T>();
        for(int i = 0; i < nCount; i++)
        {
            m.Add(this[nIndex + i]);
        }
        return m;
    }

    public int length
    {
        get { return this.Count; }
    }
}
