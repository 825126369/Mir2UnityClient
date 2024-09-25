using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableBuildContentEditor : MonoBehaviour
{
    [MenuItem("热更新/Clear Addressables/确定? ")]
    private static void ClearAddressableAA()
    {
        AddressableAssetSettings.CleanPlayerContent();
    }

    public static bool bLocalPackage = false;
    [MenuItem("热更新/一键打包-不热更/确定? ")]
    public static void Do1()
    {
        bLocalPackage = true;
        LuaCopyEditor.Do();
        AddressableCreateGroupEditor.Do();
        BuildContent();
    }

    [MenuItem("热更新/一键打包-热更")]
    public static void Do()
    {
        bLocalPackage = false;
        LuaCopyEditor.Do();
        AddressableCreateGroupEditor.Do();
        
        CheckBuildError();
        ClearOutFolder();
        if (orExistBinFile())
        {
            BuildUpdate();
        }
        else
        {
            BuildContent();
        }

        MoveToOutFolder();
    }

    public static string GetBuildRooFolder()
    {
        var RemoteBuildPath = settings.profileSettings.GetValueByName(settings.activeProfileId, AddressableProfileEditor.kRemoteBuildPath);
        RemoteBuildPath = settings.profileSettings.EvaluateString(settings.activeProfileId, RemoteBuildPath);
        return RemoteBuildPath;
    }

    private static void ClearOutFolder()
    {
        string RemoteBuildPath = GetBuildRooFolder();
        var outDirRoot = FileToolEditor.GetDirParentDir(RemoteBuildPath);
        
        FileToolEditor.DeleteFolder(RemoteBuildPath);
        FileToolEditor.DeleteFolder(outDirRoot + "/" + PlayerSettings.bundleVersion + "/");
    }

    private static void MoveToOutFolder()
    {
        string RemoteBuildPath = GetBuildRooFolder();
        var outDirRoot = FileToolEditor.GetDirParentDir(RemoteBuildPath);
        FileToolEditor.CopyFolder(RemoteBuildPath, outDirRoot + "/" + "AllUpdate_GameRes/");
        FileToolEditor.CopyFolder(RemoteBuildPath, outDirRoot + "/" + PlayerSettings.bundleVersion + "/");

        StreamingAssetCopyEditor.DoCopy(RemoteBuildPath);
        VersionUpdateTimeCheckEditor.CopyToOutDir(outDirRoot);
    }

    private static void CheckBuildError()
    {
        Debug.Log("Application.version: " + Application.version);
        Debug.Log("RemoteUrl: " + GameConst.GetEditorRemoteResUrl());

        // 有时候打包会发现 这个文件丢失的情况
        var mBuildScriptPackedMode = BuildScriptPackedMode.CreateInstance<BuildScriptPackedMode>();
        AssetDatabase.CreateAsset(mBuildScriptPackedMode, settings.DataBuilderFolder + "/BuildScriptPackedMode.asset");
        settings.SetDataBuilderAtIndex(3, mBuildScriptPackedMode, true);
    }

    private static bool orExistBinFile()
    {
        var path = ContentUpdateScript.GetContentStateDataPath(false);
        Debug.Log("orExistBinFile: " + path);
        return File.Exists(path);
    }

   // [MenuItem("AddressableEditor/自动打包Addressable 母包")]
    public static void BuildContent()
    {
        AddressablesPlayerBuildResult result;
        AddressableAssetSettings.BuildPlayerContent(out result);
        if (result != null && string.IsNullOrWhiteSpace(result.Error))
        {
            Debug.Log("Build 母包 完成 :" + result.Duration);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

   // [MenuItem("AddressableEditor/自动打包Addressable 更新包")]
    public static void BuildUpdate()
    {
        var path = ContentUpdateScript.GetContentStateDataPath(false);
        AddressablesPlayerBuildResult result = ContentUpdateScript.BuildContentUpdate(settings, path);
        if (result != null && string.IsNullOrWhiteSpace(result.Error))
        {
            Debug.Log("Build 更新包 完成 :" + result.Duration);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    //[MenuItem("AddressableEditor/Prepare Update Content")]
    public static void CheckForUpdateContent()
    {
        //与上次打包做资源对比
        string buildPath = ContentUpdateScript.GetContentStateDataPath(false);
        List<AddressableAssetEntry> entrys = ContentUpdateScript.GatherModifiedEntries(settings, buildPath);
        if (entrys.Count == 0) return;
        //将被修改过的资源单独分组
        var groupName = string.Format("UpdateGroup_{0}", DateTime.Now.ToString("yyyyMMdd"));
        ContentUpdateScript.CreateContentUpdateGroup(settings, entrys, groupName);
    }



    private static AddressableAssetSettings settings
    {
        get { return AddressableAssetSettingsDefaultObject.Settings; }
    }
}
