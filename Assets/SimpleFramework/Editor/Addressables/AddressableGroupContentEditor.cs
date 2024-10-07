using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AddressableGroupContentEditor
{
    [MenuItem("热更新/Gen BundleAllRes/确定？")]
    public static void ListentScriptEvent()
    {
        Debug.Log("Create BundleResPreafb: --------------------------------");
        List<string> mBundleFolderNameList = new List<string>();
        mBundleFolderNameList.Add("Assets/ResourceABs/InitScene/");
        mBundleFolderNameList.Add("Assets/ResourceABs/Lua/");
        mBundleFolderNameList.Add("Assets/ResourceABs/ThemeSolitaire/");

        foreach (var dirPath in mBundleFolderNameList)
        {
            CreateBundleResPreafb(dirPath);
        }
    }

    public static void Do()
    {
        AddressableGroupStructEditor.DoSettingStruct();
        CreateGroupContent1();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    //第2种打包方式，按文件夹打包
    private static void CreateGroupContent1()
    {
        List<string> mBundleFolderNameList = new List<string>();
        mBundleFolderNameList.Add("Assets/ResourceABs/InitScene/");
        mBundleFolderNameList.Add("Assets/ResourceABs/Lua/");
        mBundleFolderNameList.Add("Assets/ResourceABs/ThemeSolitaire/");

        foreach (var dirPath in mBundleFolderNameList)
        {
            CreateBundleResPreafb(dirPath);
            string bundleName = FileToolEditor.GetDirName(dirPath);
            AddressableAssetGroup group = AddressableGroupStructEditor.CreateGroup(bundleName);
            string[] allFiles = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
            foreach (string filePath in allFiles)
            {
                Debug.Assert(filePath.StartsWith("Assets/"));
                if (Path.GetExtension(filePath) != ".meta" && !filePath.Contains("/Editor/"))
                {
                    AddressableGroupStructEditor.SetGroupEntry(group, filePath);
                }
            }
        }
    }

    private static void CreateBundleResPreafb(string outDir)
    {
        GameObject go = new GameObject();
        CommonResSerialization2 mScript = go.AddComponent<CommonResSerialization2>();
        mScript.mResFolder = outDir;
        mScript.mResSuffix = ".prefab;.txt;.json";
        CommonResSerialization2Editor.DoAddAssetFromFolder(mScript);
        PrefabUtility.SaveAsPrefabAsset(go, Path.Combine(outDir, "BundleAllRes.prefab"));
        UnityEngine.Object.DestroyImmediate(go);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static AddressableAssetSettings settings
    {
        get { return AddressableAssetSettingsDefaultObject.Settings; }
    }

}