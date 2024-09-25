using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_AB_TEST
{
    A,
    B,
}

public class minigameconfig:SingleTonMonoBehaviour<minigameconfig>
{
    public bool gameCanDebug = true;
    public bool bRobotPlay = false;
    public readonly string Game_BaseResURL = "https://res.solitaireworld.xyz/mega/ejson/";
    public readonly int Ads_Level_Open =  6;
    public E_AB_TEST ABTest = E_AB_TEST.B;
    public readonly int IWand_Level_Open = 0;
    public readonly int Fail_Steps = 50;
    public readonly int Collect_Task_Coins = 200;
    public readonly int MagicWand_Price = 200;
    public readonly int Win_Award_Base_Coins = 100;
    public readonly int Win_Award_NewRecord_Coins = 20;

    // Test: 每组的步长。
    public readonly int Layer_Gap = 1;

    public readonly int  MAX_Layer_1 = 100;
    public readonly int  MAX_Layer_2 = 100;
    public readonly int  MAX_Layer_3 = 100;
    public readonly int  MAX_Layer_4 = 100;
    public readonly int  MAX_Layer_5 = 100;
    public readonly int  MAX_Layer_6 = 100;
    public readonly int  MAX_Layer_7 = 100;
    public readonly int  MAX_Layer_8 = 100;
    public readonly int  MAX_Layer_9 = 100;
    public readonly int  MAX_Layer_10 = 100;
    
    public string GetAppVersion()
    {
        return Application.version;
    }

    public int GetAdUnLockLevel()
    {
        int nUnLockLevel = Ads_Level_Open;
        if (DataCenter.Instance.nWebTag == 1)
        {
            nUnLockLevel = 6;
        }
        else if(DataCenter.Instance.nWebTag == 2)
        {
            nUnLockLevel = 10;
        }
        return nUnLockLevel;
    }
}

public static class ADPlaces
{
    public const string Exchange_BP = "Exchange_BP";
    public const string Exchange_Coin = "Exchange_Coin";
}
