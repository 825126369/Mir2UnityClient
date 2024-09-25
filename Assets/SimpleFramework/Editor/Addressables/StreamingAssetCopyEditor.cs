
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class StreamingAssetCopyEditor
{
    static string GetOutDir()
    {
        return $"{Application.streamingAssetsPath}/{GameConst.StreamingAsset_CacheBundleDir}";
    }

    static string GetJsonFilePath()
    {
        return $"{GetOutDir()}{GameConst.StreamingAsset_CacheBundleJsonFileName}";
    }

    private static List<string> GetNeedCopyFileList(string buildRootDir)
    {
        List<string> mList = new List<string>();
        mList.Add("lua");


        foreach (var filePath in Directory.GetFiles(buildRootDir, "*.bundle", SearchOption.AllDirectories))
        {
            string fileName = Path.GetFileName(filePath);
            mList.Add(fileName);
        }
        return mList;
    }

    private static bool orIsCopyFilet(List<string> NeedCopyFileList, string filePath)
    {
        foreach(var v in NeedCopyFileList)
        {
            if(filePath.Contains(v))
            {
                return true;
            }
        }

        return false;
    }

    [MenuItem("热更新/清理 StreamingAsset 重定向的 Bundle/确定？")]
    public static void ClearCache()
    {
        FileToolEditor.DeleteFolder(GetOutDir());
    }

    /// <summary>
    /// 生成BundleCache文件列表
    /// </summary>
    /// <param name="result"></param>
    public static void DoCopy(string buildRootDir)
    {
        var destDir = GetOutDir();
        if(!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        List<string> NeedCopyFileList = GetNeedCopyFileList(buildRootDir);
        List<string> allBundles = GetBundleList();
        foreach (var filePath in Directory.GetFiles(buildRootDir, "*.bundle", SearchOption.AllDirectories))
        {
            string fileName = Path.GetFileName(filePath);
            if (orIsCopyFilet(NeedCopyFileList, fileName))
            {
                if (!allBundles.Contains(fileName))
                {
                    allBundles.Add(fileName);
                }
                File.Copy(filePath, destDir + fileName, true);
            }
        }

        var json = JsonTool.ToJson(allBundles);
        File.WriteAllText(GetJsonFilePath(), json);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static List<string> GetBundleList()
    {
        string filePath = GetJsonFilePath();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            List<string> allBundles = JsonTool.FromJson<List<string>>(json);
            return allBundles;
        }
        else
        {
            return new List<string>();
        }
    }

}
