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
        // ��ȡ�����ھ��
        var hWnd = FindWindow(null, Application.productName);
        if (hWnd != IntPtr.Zero)
        {
            // �����µĴ��ڱ���
            SetWindowText(hWnd, newTitle);
        }
    }
}
#endif
