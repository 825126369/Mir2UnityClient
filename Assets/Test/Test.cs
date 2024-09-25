using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        StartCoroutine(StartCo());
    }

    IEnumerator StartCo()
    {
        yield return null;
        GameProfiler.TestStart();
        for(int i = 0; i < 100; i++)
        {
            Image mImage = obj.GetComponent<Image>();
        }
        GameProfiler.TestFinishAndLog("AAAAA");

        yield return null;
        yield return null;
        yield return null;

        GameProfiler.TestStart();
        for(int i = 0; i < 100; i++)
        {
            
        }
        GameProfiler.TestFinishAndLog("BBBB");
    }
}
