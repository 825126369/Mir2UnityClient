using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;


public class SceneMgr : SingleTonMonoBehaviour<SceneMgr>
{
    public void LoadSceneAsync(string sceneName, Action<float> mUpdateEvent, Action mFinishEvent)
    {
        StartCoroutine(LoadSceneAsync1(sceneName, mUpdateEvent, mFinishEvent));
    }

    private IEnumerator LoadSceneAsync1(string sceneName, Action<float> mUpdateEvent, Action mFinishEvent)
    {
        if (GameConst.orUseAssetBundle())
        {
            //Assets/GameAssets/Scenes/gameScene.unity
            var realSceneName = GameConst.ResRootDir + "Scenes/" + sceneName + ".unity";
            realSceneName = realSceneName.ToLower();
            var mInstanceTask = Addressables.LoadSceneAsync(realSceneName);
            while (!mInstanceTask.IsDone)
            {
                float fPercent = mInstanceTask.GetDownloadStatus().Percent;
                mUpdateEvent(fPercent);
                yield return null;
            }
        }
        else
        {
            var mInstanceTask = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!mInstanceTask.isDone)
            {
                float fPercent = mInstanceTask.progress;
                mUpdateEvent(fPercent);
                yield return null;
            }
        }

        mUpdateEvent(1.0f);
        mFinishEvent();
    }

    private void Release()
    {

    }
}
