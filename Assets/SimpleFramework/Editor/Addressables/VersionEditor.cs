using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class VersionEditor
{
    const string filePath = "Assets/MyScripts/Resources/" + GameConst.versionFileName;

    [MenuItem("热更新/生成 Version文件/确定? ")]
    public static void CreateFile()
    {
        if (File.Exists(filePath))
        {

        }
        else
        {
            VersionConfig mVersionConfig = new VersionConfig();
            mVersionConfig.minVersion = Application.version;
            mVersionConfig.maxVersion = Application.version;
            mVersionConfig.testUserId.Add("testUser1_Id");
            mVersionConfig.testUserId.Add("testUser2_Id");

            var content = JsonTool.ToJson(mVersionConfig);
            File.WriteAllText(filePath, content);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log("Version文件: " + filePath);
    }
}
