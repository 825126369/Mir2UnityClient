using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class GameConst
{
    public const bool bHotUpdate = false;
    public const string ResRootDir = "Assets/GameAssets/";
    public static readonly string ResRootDirLower = ResRootDir.ToLower();
    public const string feishuURL = "https://open.feishu.cn/open-apis/bot/v2/hook/e6665d6e-eea6-42b4-948b-01a4b1c8c631";
    public const string PlayFabId = "D706A";

    public const string spriteAtlasExtention = ".spriteatlasv2";
    //´ò°ü
    public const string buildResPath = "/../APK/Android";
    public const string versionFileName = "version.json";
    public const string StreamingAsset_CacheBundleDir = "CustomLocalCache/";
    public const string StreamingAsset_CacheBundleJsonFileName = "CustomLocalCache.json";
    public const string versionUpdateTimeCheckFileName = "versionUpdateTimeCheck.json";
    public const string remoteResUrlPrefix = "https://youxi.blob.core.windows.net/funofpoker-android";

    public static string GetRemoteResUrl()
    {
        return $"{remoteResUrlPrefix}/{VersionTool.GetBigVersionNumber(Application.version)}";
    }

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

    public static string getStreamingAssetsPathUrl(string subFilePath)
    {
        string url = "";
        string path = Path.Combine(Application.streamingAssetsPath, subFilePath);
        if (Application.platform == RuntimePlatform.Android)
        {
            url = path;
        }
        else
        {
            url = "file://" + Path.GetFullPath(path);
        }

        return url;
    }

#if UNITY_EDITOR
    public const bool PLATFORM_EDITOR = true;
#else
	public const bool PLATFORM_EDITOR = false;
#endif

#if UNITY_ANDROID
	public const bool PLATFORM_ANDROID = true;
#else
    public const bool PLATFORM_ANDROID = false;
#endif

#if UNITY_IOS
	public const bool PLATFORM_IOS = true;
#else
    public const bool PLATFORM_IOS = false;
#endif
}