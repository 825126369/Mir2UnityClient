using AKNet.Common;
using System;

public static class NetLogEx
{
    static NetLogEx()
    {
        AddNetLog();
        NetLog.bPrintLog = true;
    }
    
    public static void Init()
    {
       
    }

    private static void AddNetLog()
    {
        Action<string> LogFunc = (string message) =>
        {
            PrintTool.Log(message);
        };

        Action<string> LogErrorFunc = (string message) =>
        {
            PrintTool.LogError(message);
        };

        Action<string> LogWarningFunc = (string message) =>
        {
            PrintTool.LogWarning(message);
        };

        NetLog.AddLogFunc(LogFunc, LogErrorFunc, LogWarningFunc);
    }
}