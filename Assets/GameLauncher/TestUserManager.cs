using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestUserManager
{
    public static void Print()
    {
        Debug.Log("-------------Test User Id-----------" + GetId() + "-------------------------------");
    }

    public static string GetId()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }

    //public string GetTestUserId()
    //{
    //    if (!PlayerPrefs.HasKey("TestUserId") || string.IsNullOrWhiteSpace(PlayerPrefs.GetString("TestUserId")))
    //    {
    //        string uuId = SystemInfo.deviceUniqueIdentifier;
    //        PlayerPrefs.SetString("TestUserId", uuId);
    //        PlayerPrefs.Save();
    //    }
    //    return PlayerPrefs.GetString("TestUserId");
    //}
}
