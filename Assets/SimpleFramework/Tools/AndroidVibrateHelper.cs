using UnityEngine;
using System;



public static class AndroidVibrateHelper
{
    private static bool m_Open = true;
    public static void SetVibrateOpen(bool bOpen)
    {
        m_Open = bOpen;
    }

    public static void Call_VIBRATOR(long milliseconds, int amplitude = 1)
    {
        if (!m_Open)
        {
            return;
        }
        
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject AndroidVibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

        var VibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
        var VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", new object[] { milliseconds, amplitude });
        AndroidVibrator.Call("vibrate", VibrationEffect);
    }
}
