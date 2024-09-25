using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTools
{
    public static T DeepClone<T>(T t)
    {
        var json = JsonTool.ToJson(t);
        return JsonTool.FromJson<T>(json);
    }
    
    public static Vector3 WorldToUILocalPos(Vector3 worldPos, RectTransform mRectTransform, Camera camera = null)
    {
        if(camera == null)
        {
            camera = Camera.main;
        }

        Vector2 localPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mRectTransform, localPoint, camera, out localPoint))
        {
            return localPoint;
        }

        return Vector2.zero;
    }

    public static Vector3 ScreenToUILocalPos(Vector3 screenPos, RectTransform mRectTransform, Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        Vector2 localPoint = Vector3.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mRectTransform, screenPos, camera, out localPoint))
        {
            return localPoint;
        }

        return Vector2.zero;
    }

    public static string GetDownLoadSizeStr(long nSumSize)
    {
        if (nSumSize >= 1024 * 1024 * 1024)
        {
            return (nSumSize / 1024f / 1024f / 1024f).ToString("N1") + "Gb";
        }
        else if (nSumSize >= 1024 * 1024)
        {
            return (nSumSize / 1024f / 1024f).ToString("N1") + "Mb";
        }
        else if (nSumSize >= 1024)
        {
            return (nSumSize / 1024f).ToString("N1") + "Kb";
        }
        else
        {
            return nSumSize.ToString("N1") + "B";
        }
    }
}
