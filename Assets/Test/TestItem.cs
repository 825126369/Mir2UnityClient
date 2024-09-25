using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem
{
    static int nCount = 1;
    private int nId;
    public TestItem()
    {
        nId = nCount++;
        onFire.Instance.emit("AAAA", nId);
    }
    
    public void Update()
    {
        Debug.Log("nId: " + nId);
    }
}
