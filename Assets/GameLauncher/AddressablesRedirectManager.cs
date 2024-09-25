using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

public static class AddressablesRedirectManager
{
    static List<string> _bundleCacheList = null;
    public static IEnumerator Do()
    {
        if (_bundleCacheList == null)
        {
            var bundleCacheFilePath = GetJsonFilePath();
            var url = getUrl(bundleCacheFilePath);
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                string netErrorDes = "www Load Error:" + www.responseCode + " | " + url + " | " + www.error;
                Debug.LogError(netErrorDes);
                www.Dispose();
                yield break;
            }

            string jsonStr = www.downloadHandler.text;
            www.Dispose();

            _bundleCacheList = JsonTool.FromJson<List<string>>(jsonStr);
            if (_bundleCacheList == null)
            {
                Debug.LogError("_bundleCacheList Local Error: " + url);
                
                
            }
        }

        Do1();
    }

    static string GetLocalCacheOutDir()
    {
        return $"{Application.streamingAssetsPath}/{GameConst.StreamingAsset_CacheBundleDir}";
    }

    static string GetJsonFilePath()
    {
        return $"{GetLocalCacheOutDir()}{GameConst.StreamingAsset_CacheBundleJsonFileName}";
    }

    private static string getUrl(string bundleCacheFilePath)
    {
        string url = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            url = bundleCacheFilePath;
        }
        else
        {
            url = "file://" + Path.GetFullPath(bundleCacheFilePath);
        }

        return url;
    }

    private static void Do1()
    {
        Addressables.InternalIdTransformFunc = (IResourceLocation location) =>
        {
            string InternalId = location.InternalId;
            string PrimaryKey = location.PrimaryKey;

            if (location.Data is AssetBundleRequestOptions)
            {
                if (InternalId.StartsWith("http://") || InternalId.StartsWith("https://"))
                {
                    if (InitSceneVersionCheck.orTestUser())
                    {
                        string oriInternalId = InternalId;
                        InternalId = $"{GameConst.GetTestUserRemoteResUrl()}/{PrimaryKey}";
                        Debug.Log("[Test User] Internal Redirect Url: " + oriInternalId + " | " + InternalId);
                    }

                    if (_bundleCacheList != null && _bundleCacheList.Contains(PrimaryKey))
                    {
                        string oriInternalId = InternalId;
                        InternalId = $"{GetLocalCacheOutDir()}{PrimaryKey}";
                        Debug.Log("[Local Cache] Internal Redirect Url: " + oriInternalId + " | " + InternalId);
                    }
                }
            }

            return InternalId;
        };
    }
}
