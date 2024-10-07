using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class InitSceneHotUpdateManager : MonoBehaviour
{
    public Action<DownloadStatus> UpdateProgressFunc;
    public Action UpdateFinishFunc;
    public Action UpdateErrorFunc;
    public Action UpdateVersionFunc;

    private DownloadStatus mDownloadStatus;

    public IEnumerator CheckHotUpdate()
    {
        //VersionUpdateTimeCheck.Instance.Do();
        //yield return InitSceneVersionCheck.CheckCSharpVersionConfig();
        //if (InitSceneVersionCheck.orHaveError())
        //{
        //    DoUpdateError();
        //    yield break;
        //}
        
        //if (InitSceneVersionCheck.orNeedUpdateInLauncher())
        //{
        //    DoUpdateInLauncher();
        //    yield break;
        //}

        //TestUserManager.Print();
        yield return CheckHotUpdateRes();
    }

    private readonly List<object> _updateKeys = new List<object>();
    private IEnumerator CheckHotUpdateRes()
    {
        AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates(false);// 这里是检查更新的目录，会返回要更新的目录列表
        yield return checkHandle;
        if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError(checkHandle.OperationException.ToString());
            Addressables.Release(checkHandle);
            DoUpdateError();
            yield break;
        }
        
        List<string> catalogs = checkHandle.Result;
        Addressables.Release(checkHandle);

        Debug.Log("检查更新的目录数量: " + catalogs.Count);

        IEnumerable<IResourceLocator> locators = null;
        if (catalogs != null && catalogs.Count > 0)
        {
            foreach (var v in catalogs)
            {
                Debug.Log("Update CataLogs: " + v);
            }

            var updateHandle = Addressables.UpdateCatalogs(catalogs, false); // 这里是更新目录，会返回更新了目录里的哪些资源
            yield return updateHandle;
            if (updateHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError(updateHandle.OperationException.ToString());
                Addressables.Release(updateHandle);
                DoUpdateError();
                yield break;
            }

            locators = updateHandle.Result;
            Addressables.Release(updateHandle);
        }
        else
        {
            locators = Addressables.ResourceLocators;
        }

        _updateKeys.Clear();
        foreach (var locator in locators)
        {
            PrintTool.LogWithColor("locator: " + locator.GetType().Name + " | " + locator.ToString());
            if (locator is ResourceLocationMap)
            {
                ResourceLocationMap a = locator as ResourceLocationMap;
                foreach (var v in a.Locations)
                {
                    foreach (var v1 in v.Value)
                    {
                        if (v1.Data is AssetBundleRequestOptions)
                        {
                            var mM = v1.Data as AssetBundleRequestOptions;
                            //DebugUtility.LogWithColor("IResourceLocation Value: " + v.Key + " | " + v1.GetType().Name + " | " + v1.ToString());
                           // DebugUtility.LogWithColor("AssetBundleRequestOptions: " + mM.BundleName + " | " + mM.BundleSize / 1024 / 1024);
                        }
                    }
                }
            }

            foreach (var v in locator.Keys)
            {
                if (v.ToString().EndsWith(".bundle"))
                {
                    PrintTool.LogWithColor("AssetBundleRequestOptions: " + v);
                    _updateKeys.Add(v);
                }
            }
        }

        AssetsLoader.Instance.OnlyDownloadAssetList(new List<string>() { "InitScene" });

        List<string> mNeedInitLable = new List<string>()
        {
            "Lua",
            "ThemeSolitaire"
        };

        DoUpdateRealProgress();
        yield return AssetsLoader.Instance.AsyncDownloadAndLoadAssets(mNeedInitLable, (DownloadStatus mDownloadStatus) =>
        {
            this.mDownloadStatus = mDownloadStatus;
            DoUpdateRealProgress();
        }, (bSuccess) =>
        {
            if (bSuccess)
            {
                DoUpdateRealProgress();
                DoUpdateFinish();
                Debug.Log("CheckHotUpdate Finish, Prepare Enter Game!");
            }
            else
            {
                DoUpdateError();
            }
        });
    }

    private void DoUpdateRealProgress()
    {
        //DebugUtility.LogWithColor("DoUpdateRealProgress: " + mDownloadStatus.DownloadedBytes + " | " + mDownloadStatus.TotalBytes + " | " + mDownloadStatus.Percent);
        UpdateProgressFunc?.Invoke(mDownloadStatus);
    }

    private void DoUpdateFinish()
    {
        UpdateFinishFunc?.Invoke();
    }

    private void DoUpdateError()
    {
        UpdateErrorFunc?.Invoke();
    }

    private void DoUpdateInLauncher()
    {
        UpdateVersionFunc?.Invoke();
    }

}
