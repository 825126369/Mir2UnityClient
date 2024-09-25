using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using static UnityEngine.AddressableAssets.Addressables;

public class InitSceneHotUpdateManager : MonoBehaviour
{
    public long nTotalBytes = -1;
    public long nDownloadBytes = -1;
    public float fPercent;
    public Action<DownloadProgressInfo> UpdateProgressFunc;
    public Action UpdateFinishFunc;
    public Action UpdateErrorFunc;
    public Action UpdateVersionFunc;
    private readonly List<object> _updateKeys = new List<object>();
    
    public IEnumerator CheckHotUpdate()
    {
        // VersionUpdateTimeCheck.Instance.Do();
        // yield return InitSceneVersionCheck.CheckCSharpVersionConfig();
        // if (InitSceneVersionCheck.orHaveError())
        // {
        //     DoUpdateError();
        //     yield break;
        // }
        
        // if (InitSceneVersionCheck.orNeedUpdateInLauncher())
        // {
        //     DoUpdateInLauncher();
        //     yield break;
        // }

        // TestUserManager.Print();
        // yield return AddressablesRedirectManager.Do();
        yield return CheckHotUpdateRes();
    }
    
    private IEnumerator CheckHotUpdateRes()
    {
        fPercent = 0f;
        nTotalBytes = -1;
        nDownloadBytes = -1;
        DoUpdateRealProgress();
        
        //AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates(false);// 这里是检查更新的目录，会返回要更新的目录列表
        //yield return checkHandle;
        //if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        //{
        //    Debug.LogError(checkHandle.OperationException.ToString());
        //    Addressables.Release(checkHandle);
        //    DoUpdateError();
        //    yield break;
        //}
        //_updateKeys.Clear();
        //List<string> catalogs = checkHandle.Result;
        //Addressables.Release(checkHandle);

        //Debug.Log("Update catalogs Count: " + catalogs.Count);
        //if (catalogs != null && catalogs.Count > 0)
        //{
        //    var updateHandle = Addressables.UpdateCatalogs(catalogs, false); // 这里是更新目录，会返回更新了目录里的哪些资源
        //    yield return updateHandle;
        //    if (updateHandle.Status != AsyncOperationStatus.Succeeded)
        //    {
        //        Debug.LogError(updateHandle.OperationException.ToString());
        //        Addressables.Release(updateHandle);
        //        DoUpdateError();
        //        yield break;
        //    }

        //    foreach (var v in updateHandle.Result)
        //    {
        //        _updateKeys.AddRange(v.Keys);
        //    }

        //    Addressables.Release(updateHandle);
        //}
        //else
        //{
        //    IEnumerable<IResourceLocator> locators = Addressables.ResourceLocators;
        //    foreach (var locator in locators)
        //    {
        //        _updateKeys.AddRange(locator.Keys);
        //    }
        //}

        fPercent = 0.1f;
        DoUpdateRealProgress();
        
        // 开始下载
        yield return AssetsLoader.Instance.AsyncDownloadAnLoadAssetsByLabel("Game", (float mStatus)=>
        {
            nTotalBytes = 0;
            nDownloadBytes = 0;
            fPercent = 0.1f + mStatus * 0.9f;
            DoUpdateRealProgress();
        });
        // 结束下载

        fPercent = 1.0f;
        DoUpdateRealProgress();
        DoUpdateFinish();
        
        Debug.Log("CheckHotUpdate Finish, Prepare Enter Game!");
    }

    private void DoUpdateRealProgress()
    {
        DownloadProgressInfo mInfo = new DownloadProgressInfo();
        mInfo.DownloadedBytes = nDownloadBytes;
        mInfo.TotalBytes = nTotalBytes;
        mInfo.Percent = fPercent;
        UpdateProgressFunc?.Invoke(mInfo);
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
