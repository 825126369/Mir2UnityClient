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

public class AddressableGroupStructEditor
{
    public static void DoSettingStruct()
    {
        if (GameConst.bHotUpdate == false)
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

    public static AddressableAssetGroup CreateGroup(string bundleName, bool bLocalPackage = false)
    {
        bundleName = bundleName.ToLower();
        AddressableAssetGroup group = FindGroup(bundleName);
        if (group == null)
        {
            group = settings.CreateGroup(bundleName, false, false, true, null, typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
        }

        var mSchema = group.GetSchema<BundledAssetGroupSchema>();
        mSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.AppendHash;

        if (GameConst.bHotUpdate == false || bLocalPackage)
        {
            mSchema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kLocalBuildPath);
            mSchema.LoadPath.SetVariableByName(settings, AddressableAssetSettings.kLocalLoadPath);
        }
        else
        {
            mSchema.BuildPath.SetVariableByName(settings, AddressableProfileEditor.kRemoteBuildPath);
            mSchema.LoadPath.SetVariableByName(settings, AddressableProfileEditor.kRemoteLoadPath);
        }
        
        bool bRemove = true;
        while(bRemove)
        {   
            bRemove = false;
            foreach(var v in group.entries)
            {
                if(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(v.AssetPath) == null)
                {
                    group.RemoveAssetEntry(v, true);
                    bRemove = true;
                    break;
                }
            }
        }

        return group;
    }

    public static void SetGroupEntry(AddressableAssetGroup group, string filePath)
    {
        var fielName = Path.GetFileName(filePath);
        string guid = AssetDatabase.AssetPathToGUID(filePath);  //要打包的资产条目   将路径转成guid
        AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group, false, true);//要打包的资产条目   会将要打包的路径移动到group节点下
        if (entry != null)
        {
            if (fielName.EndsWith(".prefab") ||
                fielName.EndsWith(".unity"))
            {
                entry.SetLabel(group.Name.ToLower(), true, true);
            }
            else
            {
                entry.SetLabel(group.Name.ToLower(), false, true);
            }
            entry.SetAddress(entry.AssetPath.ToLower());
        }
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