using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoreReviewTool
{
    public static void DoAndroid()
    {
#if UNITY_ANDROID
        DataCenter.Instance.player.bReview = true;
        Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}");
#endif
    }

    public static void DoIOS()
    {
#if UNITY_IOS
        DataCenter.Instance.bReview = true;
        UnityEngine.iOS.Device.RequestStoreReview();
#endif
    }

}
