using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class PrintTool
{
    private static readonly StringBuilder mStringBuilder = new StringBuilder();

    public static void LogWithColor(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null, object data7 = null, object data8 = null, object data9 = null)
    {
        if (!GameLauncher.Instance.m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6, data7, data8, data9);
        Debug.Log($"<color=yellow>{content}</color>");
    }
    
    public static void LogJsonObj(object data)
    {
        if (!GameLauncher.Instance.m_PrintToolLog) return;
        Debug.Log(JsonTool.ToJson(data));
    }

    private static string GetStr(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null, object data7 = null, object data8 = null, object data9 = null)
    {
        mStringBuilder.Clear();
        if (data1 != null)
        {
            mStringBuilder.Append(data1 + "___");
        }
        if (data2 != null)
        {
            mStringBuilder.Append(data2 + "___");
        }
        if (data3 != null)
        {
            mStringBuilder.Append(data3 + "___");
        }
        if (data4 != null)
        {
            mStringBuilder.Append(data4 + "___");
        }
        if (data5 != null)
        {
            mStringBuilder.Append(data5 + "___");
        }
        if (data6 != null)
        {
            mStringBuilder.Append(data6 + "___");
        }
        if (data7 != null)
        {
            mStringBuilder.Append(data7 + "___");
        }
        if (data8 != null)
        {
            mStringBuilder.Append(data8 + "___");
        }
        if (data9 != null)
        {
            mStringBuilder.Append(data9 + "___");
        }
        return mStringBuilder.ToString();
    }

    public static void Log(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null, object data7 = null, object data8 = null, object data9 = null)
    {
        if (!GameLauncher.Instance.m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6, data7, data8, data9);
        Debug.Log(content);
    }

    public static void LogWarning(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null, object data7 = null, object data8 = null, object data9 = null)
    {
        if (!GameLauncher.Instance.m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6, data7, data8, data9);
        Debug.LogWarning(content);
    }

    public static void LogError(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null, object data7 = null, object data8 = null, object data9 = null)
    {
        if (!GameLauncher.Instance.m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6, data7, data8, data9);
        Debug.LogError(content);
    }

    public static void Assert(bool isTrue, object data1 = null, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null, object data7 = null, object data8 = null, object data9 = null)
    {
        if (!GameLauncher.Instance.m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6, data7, data8, data9);
        Debug.Assert(isTrue, content);
    }

}
