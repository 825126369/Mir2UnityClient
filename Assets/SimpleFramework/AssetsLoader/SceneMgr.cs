using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneMgr : SingleTonMonoBehaviour<SceneMgr>
{
    Dictionary<string, SceneInstance> mAssetDic = new Dictionary<string, SceneInstance>();
    Dictionary<string, AsyncOperation> mAssetDicInEditor = new Dictionary<string, AsyncOperation>();

    private Action mActiveFunc;
    public void LoadSceneAsync(string sceneName, Action<float> mUpdateEvent = null, Action mFinishEvent = null)
    {
        StartCoroutine(LoadSceneAsync1(sceneName,  LoadSceneMode.Single, true, mUpdateEvent, mFinishEvent));
    }

    public void LoadSceneAsyncInBackGroud(string sceneName, Action<float> mUpdateEvent = null, Action mFinishEvent = null)
    {
        StartCoroutine(LoadSceneAsync1(sceneName, LoadSceneMode.Single, false, mUpdateEvent, mFinishEvent));
    }

    private IEnumerator LoadSceneAsync1(string sceneName, LoadSceneMode mLoadSceneMode, bool activateOnLoad, Action<float> mUpdateEvent = null, Action mFinishEvent = null)
    {
        if (GameConst.orUseAssetBundle())
        {
            var realSceneName = "Assets/Scenes/" + sceneName + ".unity";
            realSceneName = realSceneName.ToLower();
            var mInstanceTask = Addressables.LoadSceneAsync(realSceneName, mLoadSceneMode, activateOnLoad);
            mAssetDic[sceneName] = mInstanceTask.WaitForCompletion();

            mUpdateEvent?.Invoke(mInstanceTask.GetDownloadStatus().Percent);
            while (!mInstanceTask.IsDone)
            {
                mUpdateEvent?.Invoke(mInstanceTask.GetDownloadStatus().Percent);
                yield return null;
            }

            mAssetDic[sceneName] = mInstanceTask.Result;
            mUpdateEvent?.Invoke(mInstanceTask.GetDownloadStatus().Percent);
        }
        else
        {
            AsyncOperation mInstanceTask = SceneManager.LoadSceneAsync(sceneName, mLoadSceneMode);
            mAssetDicInEditor[sceneName] = mInstanceTask;
            mInstanceTask.allowSceneActivation = activateOnLoad; //如果设置为false，有两种情况1:另外一个场景加载后把它干掉，那么直接isDone==true，要么他就是一直处于0.9进度
            mUpdateEvent?.Invoke(mInstanceTask.progress);
            while (!mInstanceTask.isDone)
            {
                mUpdateEvent?.Invoke(mInstanceTask.progress);
                yield return null;
            }
            mUpdateEvent?.Invoke(mInstanceTask.progress);
        }
        mFinishEvent?.Invoke();
        mActiveFunc?.Invoke();

        mFinishEvent = null;
        mUpdateEvent = null;
        mActiveFunc = null;
    }

    public bool orScenePerpareOk_ForActiveScene(string sceneName)
    {
        if (GameConst.orUseAssetBundle())
        {
            return mAssetDic.ContainsKey(sceneName) && mAssetDic[sceneName].Scene.IsValid();
        }
        else
        {
            return mAssetDicInEditor.ContainsKey(sceneName)  && mAssetDicInEditor[sceneName] != null;
        }
    }

    public void ActiveScene(string sceneName, Action mFinishEvent = null)
    {
        StartCoroutine(ActiveScene1(sceneName, mFinishEvent));
    }

    private IEnumerator ActiveScene1(string sceneName, Action mFinishEvent)
    {
        if (GameConst.orUseAssetBundle())
        {
            yield return mAssetDic[sceneName].ActivateAsync();
            mFinishEvent?.Invoke();
        }
        else
        {
            mActiveFunc = mFinishEvent;
            mAssetDicInEditor[sceneName].allowSceneActivation = true;
        }
    }

    public void RemoveAsset(string sceneName)
    {
        if (GameConst.orUseAssetBundle())
        {
            SceneInstance asset;
            if (mAssetDic.TryGetValue(sceneName, out asset))
            {
                mAssetDic.Remove(sceneName);
                Addressables.Release(asset);//切换场景的时候，Addressable内部会自动释放资源的，这里只是走个形式，反正没问题
            }
        }
        else
        {
            mAssetDicInEditor.Remove(sceneName);
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                StartCoroutine(RemoveAsset1(sceneName));
            }
        }
    }

    private IEnumerator RemoveAsset1(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);
    }

}
