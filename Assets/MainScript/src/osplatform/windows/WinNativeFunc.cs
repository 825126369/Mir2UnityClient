#if UNITY_WIN
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class WinNativeFunc
{
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool SetWindowText(IntPtr hwnd, string lpString);

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    public static void ChangeWindowTitle(string newTitle)
    {
        // 获取主窗口句柄
        var hWnd = FindWindow(null, Application.productName);
        if (hWnd != IntPtr.Zero)
        {
            // 设置新的窗口标题
            SetWindowText(hWnd, newTitle);
        }
    }
}
#endif
