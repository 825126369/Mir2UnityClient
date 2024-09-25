using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Schema;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.GraphicsBuffer;

public class AddressableCreateGroupEditor
{
    //[MenuItem("AddressableEditor/自动创建Addressable目录")]
    public static void Do()
    {
        SetSetting();
        CreateGroupContent2();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateGroupContent2()
    {
        List<string> mBundleFolderNameList = new List<string>();
        foreach (var v in Directory.GetDirectories(GameConst.ResRootDir))
        {
            mBundleFolderNameList.Add(v);
        }
        
        foreach (var dirPath in mBundleFolderNameList)
        {
            string bundleName = dirPath.Substring(dirPath.LastIndexOf("/") + 1).ToLower();
            AddressableAssetGroup group = CreateGroup(bundleName);
            string[] allFiles = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
            foreach (string filePath in allFiles)
            {
                Debug.Assert(filePath.StartsWith("Assets/"));
                if (Path.GetExtension(filePath) == ".prefab")
                {
                    string guid = AssetDatabase.AssetPathToGUID(filePath);  //要打包的资产条目   将路径转成guid
                    AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group, false, true);//要打包的资产条目   会将要打包的路径移动到group节点下
                    if (entry != null)
                    {
                        entry.SetLabel(bundleName, true, true, true);//第一个参数是创建这个标签  第二个是 是否开启标签
                        entry.SetAddress(entry.AssetPath.ToLower());
                    }
                }
            }
        }
    }

    private static void SetSetting()
    {
        if (AddressableBuildContentEditor.bLocalPackage)
        {
            settings.BuildRemoteCatalog = false;
            settings.CertificateHandlerType = typeof(WebRequestCertificateHandler);
        }
        else
        {
            settings.BuildRemoteCatalog = true;
            settings.DisableCatalogUpdateOnStartup = true;
            settings.CertificateHandlerType = typeof(WebRequestCertificateHandler);
            AddressableProfileEditor.SetProfile();
            settings.RemoteCatalogBuildPath.SetVariableByName(settings, AddressableProfileEditor.kRemoteBuildPath);
            settings.RemoteCatalogLoadPath.SetVariableByName(settings, AddressableProfileEditor.kRemoteLoadPath);
        }
    }
    
    private static AddressableAssetGroup CreateGroup(string bundleName)
    {
        AddressableAssetGroup group = FindGroup(bundleName);
        if (group == null)
        {
            group = settings.CreateGroup(bundleName, false, false, true, null, typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
        }

        var mSchema = group.GetSchema<BundledAssetGroupSchema>();
        mSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.AppendHash;

        if (AddressableBuildContentEditor.bLocalPackage)
        {
            mSchema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kLocalBuildPath);
            mSchema.LoadPath.SetVariableByName(settings, AddressableAssetSettings.kLocalLoadPath);
        }
        else
        {
            mSchema.BuildPath.SetVariableByName(settings, AddressableProfileEditor.kRemoteBuildPath);
            mSchema.LoadPath.SetVariableByName(settings, AddressableProfileEditor.kRemoteLoadPath);
        }

        return group;
    }
    
    private static AddressableAssetGroup FindGroup(string groupName)
    {
        for (int i = 0; i < settings.groups.Count; ++i)
        {
            AddressableAssetGroup group = settings.groups[i];
            if (group != null)
            {
                if (groupName == group.Name)
                {
                    return group;
                }
            }
        }
        return null;
    }

    private static void setCustomValue(ProfileValueReference mProfileValueReference, string value)
    {
        var m_Id = mProfileValueReference.GetType().GetField("m_Id", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
        Debug.Assert(m_Id != null, "m_Id == null");
        m_Id.SetValue(mProfileValueReference, value);
        mProfileValueReference.OnValueChanged?.Invoke(mProfileValueReference);
    }

    private static AddressableAssetSettings settings
    {
        get { return AddressableAssetSettingsDefaultObject.GetSettings(true); }
    }

}