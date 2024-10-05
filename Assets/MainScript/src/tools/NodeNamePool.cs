using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeNamePool
{
    public readonly Dictionary<string, NodePool> mapNamePool = new Dictionary<string, NodePool>();

    public NodePool GetNodePool(string name)
    {
        return mapNamePool[name];
    }

    public NodePool PreLoadPool(int nInitCount, GameObject goPrefab, string prefabName = null)
    {
        if (string.IsNullOrWhiteSpace(prefabName))
        {
            prefabName = goPrefab.name;
        }

        NodePool mNodePool = null;
        if (!mapNamePool.TryGetValue(prefabName, out mNodePool))
        {
            mNodePool = new NodePool();
            mNodePool.Init(goPrefab, nInitCount);
            mapNamePool[prefabName] = mNodePool;
        }

        mNodePool.preLoadObj(nInitCount);
        return mNodePool;
    }

    public GameObject popObj(string name)
    {
        GameObject go = GetNodePool(name).popObj();
        if (go.name != name)
        {
            go.name = name;
        }
        return go;
    }

    public void recycleObj(GameObject obj, string name = null)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            name = obj.name;
        }

        GetNodePool(name).recycleObj(obj);
    }

}
