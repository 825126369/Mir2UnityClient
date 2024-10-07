using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameConst:Singleton<GameConst>
{
    public const string ResRootDir = "Assets/GameAssets/";
    public static readonly string ResRootDirLower = ResRootDir.ToLower();
    public const string feishuURL = "";
    public const string buildResPath = "/../APK/Android";
    public const string versionFileName = "version.json";
    public const string versionUpdateTimeCheckFileName = "versionUpdateTimeCheck.json";
    public const string StreamingAsset_CacheBundleDir = "CustomLocalCache/";
    public const string StreamingAsset_CacheBundleJsonFileName = "CustomLocalCache.json";
    public const string remoteResUrlPrefix = "https://storage.googleapis.com/sgame/JTest/template_Test";

    public const string AdsAppKey = "9uHgeBwag3NXva9MC23ToO3q11Ve59bF1uwg4qGltdGmCQ7OSByFZ_3b1ZF7krMlkHQo5gXzIokVDsvg1rwbr-";

#if UNITY_EDITOR
    public static string GetEditorRemoteResUrl()
    {
        return $"{remoteResUrlPrefix}/{UnityEditor.PlayerSettings.bundleVersion}";
    }
#endif

    public static string GetTestUserRemoteResUrl()
    {
        return $"{remoteResUrlPrefix}/Test";
    }

    public static string GetVersionConfigUrl()
    {
        return $"{remoteResUrlPrefix}/{versionFileName}";
    }

    public static string GetVersionUpdateTimeCheckUrl()
    {
        return $"{remoteResUrlPrefix}/{versionUpdateTimeCheckFileName}";
    }

    public static bool orUseAssetBundle()
    {
        bool bUseBundle = false;
#if UNITY_EDITOR
        var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            return false;
        }

        var playModeType = settings.ActivePlayModeDataBuilder.GetType();
        if (playModeType == typeof(UnityEditor.AddressableAssets.Build.DataBuilders.BuildScriptPackedPlayMode))
        {
            bUseBundle = true;
        }
#else
            bUseBundle = true;
#endif
        return bUseBundle;
    }
    
    public static bool isMobilePlatform()
    {
        return Application.isMobilePlatform;
    }
}