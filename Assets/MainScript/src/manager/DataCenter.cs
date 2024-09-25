using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DataCenter:SingleTonMonoBehaviour<DataCenter>
{
    // 当前存档时间
    public bool bMute = false;
    public int nWebTag = 0;
    public bool bReview = false;
    public int nThemeIndex = 1;
    public int nCoinCount = 0;
    public int nLevel;
    public bool bShowResurrectionUI = false;

    public void Init()
    {
        
    }
}
