using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class TSMap<T1, T2> : Dictionary<T1, T2>
{
    public TSMap():base()
    {
        
    }

    protected TSMap(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    public void set(T1 key, T2 value)
    {
        this[key] = value;
    }

    public T2 get(T1 key)
    {
        if (this.ContainsKey(key))
        {
            T2 t = this[key];
            return t;
        }
        else
        {
            return default;
        }
    }

    public int size
    {
        get { return this.Count; }
    }
}
