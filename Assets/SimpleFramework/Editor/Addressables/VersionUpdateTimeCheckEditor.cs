using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class VersionUpdateTimeCheckEditor
{
    const string filePath = "Assets/MyScripts/Resources/" + GameConst.versionUpdateTimeCheckFileName;
    private static void CreateFile()
    {
        if (!File.Exists(filePath))
        {
            VersionUpdateTimeCheckConfig mVersionConfig = new VersionUpdateTimeCheckConfig();
            var content = JsonTool.ToJson(mVersionConfig);
            File.WriteAllText(filePath, content);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log($"{GameConst.versionUpdateTimeCheckFileName}: {filePath}");
    }

    [MenuItem("热更新/生成 VersionUpdateTimeCheck 文件/确定? ")]
    private static void SetDaBaoTime()
    {
        CreateFile();
        var versionFileContent = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
        VersionUpdateTimeCheckConfig mVersionConfig = JsonTool.FromJson<VersionUpdateTimeCheckConfig>(versionFileContent.text);

        if (mVersionConfig.mUpdateTimeDic.ContainsKey(Application.version))
        {
            mVersionConfig.mUpdateTimeDic[Application.version] = TimeTool.GetNowTimeStamp();
        }
        else
        {
            mVersionConfig.mUpdateTimeDic.Add(Application.version, TimeTool.GetNowTimeStamp());
        }

        var content = JsonTool.ToJson(mVersionConfig);
        File.WriteAllText(filePath, content);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void CopyToOutDir(string dir)
    {
        SetDaBaoTime();
        File.Copy(filePath, Path.Combine(dir, Path.GetFileName(filePath)), true);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

