using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameConst:Singleton<GameConst>
{
    public const string ResRootDir = "Assets/GameAssets/";
    public const string feishuURL = "";
    //public const string feishuURL = "https://open.feishu.cn/open-apis/bot/v2/hook/5a9911ae-565d-4247-8d11-ea19deb5dbb7";
    public const string ThinkingAnalyticsServerUrl = "https://rtsyx.higgsyx.com/";
    public const string buildResPath = "/../APK/Android";
    public const string versionFileName = "version.json";
    public const string versionUpdateTimeCheckFileName = "versionUpdateTimeCheck.json";
    public const string StreamingAsset_CacheBundleDir = "CustomLocalCache/";
    public const string StreamingAsset_CacheBundleJsonFileName = "CustomLocalCache.json";
    public const string remoteResUrlPrefix = "https://storage.googleapis.com/sgame/JTest/template_Test";

    public const string AdsAppKey = "9uHgeBwag3NXva9MC23ToO3q11Ve59bF1uwg4qGltdGmCQ7OSByFZ_3b1ZF7krMlkHQo5gXzIokVDsvg1rwbr-";    //ironsource
#if UNITY_ANDROID
        public const string AFDevKey = "bRPJywoUsNqpXwuYWpkgz8";
        public const string ThinkingAnalyticsAppId = "d9447828e51d46bf8fe091c93428456f";//witt

        public const string APPID = "";


        public const string RewardvideoID ="77f5105b0754777e";
        public const string InterstitialID ="df686a893fcd49cc";
        public const string BannerID ="bc5005c1a249dd18";
        
        public const string Amazon_AppKey = "2f3fd53a-e8b7-4505-9ea9-87125c622398";
        public const string Amazon_Banner_UID = "8a0a9e1f-22d5-4e8d-b234-39b00fc6d353";
        public const string Amazon_Interstitial_UID = "77e59278-b291-4a72-820a-9d135544a5b7";
        public const string Amazon_Rewarded_UID = "e893de6c-e250-4bab-9668-cac427dcc1fe";
#elif UNITY_IOS
        public const string AFDevKey = "bHg5MgBwAcDRh6GX4toPMi";
        public const string ThinkingAnalyticsAppId = "d9447828e51d46bf8fe091c93428456f"; //witt
        public const string APPID = "6467742649";

        public const string RewardvideoID ="42ec932dbcbefee4";
        public const string InterstitialID ="e6e7b507afcad65f";
        public const string BannerID ="bfd3159471d43f6c";

        public const string Amazon_AppKey = "f8c53595-6b88-4755-92b3-5e66aa3d1e54";
        public const string Amazon_Banner_UID = "80c7d42d-0b49-45e3-8d07-cb1b550c3078";
        public const string Amazon_Interstitial_UID = "a69cceb2-09a6-44a7-bf78-eec384add98d";
        public const string Amazon_Rewarded_UID = "084b363c-bfbb-4d28-a485-d85da2d0efa8";
#endif

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