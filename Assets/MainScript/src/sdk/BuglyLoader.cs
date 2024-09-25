using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BuglyLoader : MonoBehaviour
{
    void Awake()
    {
        Application.logMessageReceived += _OnLogCallbackHandler;
        System.AppDomain.CurrentDomain.UnhandledException += _OnUncaughtExceptionHandler;
    }
    
    private static void _OnUncaughtExceptionHandler(object sender, System.UnhandledExceptionEventArgs args)
    {
        Exception exception = args.ExceptionObject as Exception;
        UploadFeiShuTool.Do("AppDomain内部捕获的异常: " + exception.Message + " | " + exception.StackTrace);
    }

    private static void _OnLogCallbackHandler(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Log || type == LogType.Warning)
        {
            return;
        }
        UploadFeiShuTool.Do("Unity内部捕获的异常: " + type.ToString() +": " + stackTrace);
    }
}
