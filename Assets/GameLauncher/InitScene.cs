using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class InitScene : SingleTonMonoBehaviour<InitScene>
{
    public InitSceneView mInitSceneView;
    public InitSceneHotUpdateManager mInitSceneHotUpdateManager = null;

    public bool bProgressFull = false;
    public void Init()
    {
        mInitSceneHotUpdateManager = gameObject.AddMissComponent<InitSceneHotUpdateManager>();
        mInitSceneHotUpdateManager.UpdateProgressFunc += UpdateDownloadAssetsProgress;
        mInitSceneHotUpdateManager.UpdateFinishFunc += UpdateDownloadAssetsFinish;
        mInitSceneHotUpdateManager.UpdateErrorFunc += UpdateDownloadAssetsError;
        mInitSceneHotUpdateManager.UpdateVersionFunc += UpdateVersion;
        
        GameObject goPrefab = AssetsLoader.Instance.GetAsset("Assets/GameAssets/InitScene/InitSceneView.prefab") as GameObject;
        GameObject goView = Instantiate<GameObject>(goPrefab);
        goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Loading, false);
        mInitSceneView = goView.GetComponent<InitSceneView>();
        mInitSceneView.Init();
        
        if (GameConst.orUseAssetBundle())
        {
            CheckHotUpdate();
        }
        else
        {
            mInitSceneView.Show(() =>
            {
                bProgressFull = true;
            });
            mInitSceneView.UpdateRealProgress(1.0f);
            UpdateDownloadAssetsFinish();
        }
    }

    private void CheckHotUpdate()
    {
        bProgressFull = false;
        mInitSceneView.Show(() =>
        {
            bProgressFull = true;
        });
        StartCoroutine(mInitSceneHotUpdateManager.CheckHotUpdate());
    }

    public void UpdateDownloadAssetsProgress(DownloadProgressInfo mInfo)
    {
        mInitSceneView.UpdateRealProgress(mInfo);
    }

    public void UpdateDownloadAssetsFinish()
    {
        GameLauncher.Instance.OnHotUpdateFinish();
    }
    
    public void UpdateDownloadAssetsError()
    {
        
    }

    public void UpdateLoadSceneProgress(float fPercent)
    {
        mInitSceneView.UpdateRealProgress(0.6f + 0.4f * fPercent);
    }

    public void UpdateVersion()
    {
        Debug.LogError("需要强制更新版本");
    }
}
