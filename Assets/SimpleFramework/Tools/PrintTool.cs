using System.Text;
using UnityEngine;

public static class PrintTool
{
    public static bool m_PrintToolLog = true;
    private static readonly StringBuilder mStringBuilder = new StringBuilder();

    public static void LogWithColor(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null)
    {
        if (!m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6);
        Debug.Log($"<color=yellow>{content}</color>");
    }

    public static void LogJsonObj(object data)
    {
        if (!m_PrintToolLog) return;
        Debug.Log(JsonTool.ToJson(data));
    }

    private static string GetStr(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null)
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
        return mStringBuilder.ToString();
    }

    public static void Log(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null)
    {
        if (!m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6);
        Debug.Log(content);
    }

    public static void LogWarning(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null)
    {
        if (!m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6);
        Debug.LogWarning(content);
    }

    public static void LogError(object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null)
    {
        if (!m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6);
        Debug.LogError(content);
    }

    public static void Assert(bool isTrue, object data1 = null, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null)
    {
        if (!m_PrintToolLog) return;
        string content = GetStr(data1, data2, data3, data4, data5, data6);
        Debug.Assert(isTrue, content);
    }

}
