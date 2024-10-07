using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetsLoader : SingleTonMonoBehaviour<AssetsLoader>
{
    private readonly Dictionary<string, UnityEngine.Object> mAssetDic = new Dictionary<string, UnityEngine.Object>();
    private readonly Dictionary<string, List<string>> LabelDic = new Dictionary<string, List<string>>();

    #region 下载特定Label的资源
    public void OnlyDownloadAssetList(List<string> mNeedDownloadKey, Action<DownloadStatus> UpdateEvent = null, Action<bool> mFinishEvent = null)
    {
        StartCoroutine(AsyncOnlyDownloadAssetList(mNeedDownloadKey, UpdateEvent = null, mFinishEvent = null));
    }

    public IEnumerator AsyncOnlyDownloadAssetList(List<string> mNeedDownloadKey, Action<DownloadStatus> UpdateEvent, Action<bool> mFinishEvent)
    {
        //开始下载
        var mKeys = mNeedDownloadKey as List<string>;
        for (int i = 0; i < mKeys.Count; i++)
        {
            mKeys[i] = mKeys[i].ToLower();
        }
        AsyncOperationHandle _downloadHandle = Addressables.DownloadDependenciesAsync(mKeys, Addressables.MergeMode.Union, false);

        UpdateEvent?.Invoke(_downloadHandle.GetDownloadStatus());
        while (!_downloadHandle.IsDone)
        {
            yield return null;
            UpdateEvent?.Invoke(_downloadHandle.GetDownloadStatus());
        }

        UpdateEvent?.Invoke(_downloadHandle.GetDownloadStatus());
        if (_downloadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("总共下载的Bundle大小：" + GameTools.GetDownLoadSizeStr(_downloadHandle.GetDownloadStatus().TotalBytes));
            mFinishEvent?.Invoke(true);
        }
        else
        {
            mFinishEvent?.Invoke(false);
        }

        Addressables.Release(_downloadHandle);
    }
    
    public void DownloadAndLoadAssets(List<string> mNeedLoadLableList, Action<DownloadStatus> UpdateEvent, Action<bool> mFinishEvent)
    {
        foreach(var v in mNeedLoadLableList)
        {
            Debug.Log("DownloadAndLoadAssets: " + v);
        }

        StartCoroutine(AsyncDownloadAndLoadAssets(mNeedLoadLableList, UpdateEvent, mFinishEvent));
    }

    public IEnumerator AsyncDownloadAndLoadAssets(List<string> mNeedLoadLableList, Action<DownloadStatus> UpdateEvent, Action<bool> mFinishEvent)
    {
        if (!GameConst.orUseAssetBundle())
        {
            yield return new WaitForSeconds(0.5f);
            UpdateEvent?.Invoke(new DownloadStatus() { IsDone = true });
            mFinishEvent?.Invoke(true);
            yield break;
        }

        for (int i = 0; i < mNeedLoadLableList.Count; i++)
        {
            mNeedLoadLableList[i] = mNeedLoadLableList[i].ToLower();
        }

        var mResLocListHandle = Addressables.LoadResourceLocationsAsync(mNeedLoadLableList, Addressables.MergeMode.Union);
        yield return mResLocListHandle;
        var mResLocList = mResLocListHandle.Result;
        Addressables.Release(mResLocListHandle);

        AsyncOperationHandle<IList<UnityEngine.Object>> mAssetListHandle = Addressables.LoadAssetsAsync<UnityEngine.Object>(mResLocList, null);

        UpdateEvent?.Invoke(mAssetListHandle.GetDownloadStatus());
        while (!mAssetListHandle.IsDone)
        {
            yield return null;
            UpdateEvent?.Invoke(mAssetListHandle.GetDownloadStatus());
        }
        UpdateEvent?.Invoke(mAssetListHandle.GetDownloadStatus());

        if (mAssetListHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var mAssetList = mAssetListHandle.Result;
            for (int i = 0; i < mResLocList.Count; i++)
            {
                string assetPath = mResLocList[i].PrimaryKey;
                var obj = mAssetList[i];
                AddAsset(assetPath, obj);
            }

            mFinishEvent?.Invoke(true);
        }
        else
        {
            mFinishEvent?.Invoke(false);
            Addressables.Release(mAssetListHandle);
        }
    }
    #endregion


    public void LoadSingleAssetCallBack(string assetPath, Action mFunc = null)
    {
        StartCoroutine(AsyncLoadSingleAsset(assetPath, mFunc));
    }

    public void LoadManyAssetCallBack(List<string> assetPaths, Action mFunc = null)
    {
        StartCoroutine(AsyncLoadManyAssets(assetPaths, mFunc));
    }

    public void LoadManyAssetsByLabelCallBack(string key, Action mFunc = null)
    {
        StartCoroutine(AsyncLoadManyAssetsByLabel(key, mFunc));
    }

    // key:可以是路径，标签，以及简单的名字
    public IEnumerator AsyncLoadSingleAsset(string assetPath, Action mFunc = null)
    {
        Debug.Assert(assetPath != null, "assetPaths == null");
        if (!GameConst.orUseAssetBundle())
        {
            yield return new WaitForSeconds(0.5f);
            mFunc?.Invoke();
            yield break;
        }

        assetPath = assetPath.ToLower();
        UnityEngine.Object asset = null;
        if (!orExistAsset(assetPath))
        {
            var assetHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(assetPath);
            yield return assetHandle;
            asset = assetHandle.Result;
            AddAsset(assetPath, asset);
        }

        mFunc?.Invoke();
    }

    public IEnumerator AsyncLoadManyAssets(List<string> assetPaths, Action mFunc = null)
    {
        Debug.Assert(assetPaths != null, "assetPaths == null");
        if (!GameConst.orUseAssetBundle())
        {
            yield return new WaitForSeconds(1.0f);
            mFunc?.Invoke();
            yield break;
        }

        for (int i = 0; i < assetPaths.Count; i++)
        {
            assetPaths[i] = assetPaths[i].ToLower();
        }

        var mResLocListHandle = Addressables.LoadResourceLocationsAsync(assetPaths, Addressables.MergeMode.Union);
        yield return mResLocListHandle;
        var mResLocList = mResLocListHandle.Result;
        Addressables.Release(mResLocListHandle);
        
        AsyncOperationHandle<IList<UnityEngine.Object>> mAssetListHandle = Addressables.LoadAssetsAsync<UnityEngine.Object>(mResLocList, null);
        yield return mAssetListHandle;
        var mAssetList = mAssetListHandle.Result;

        for (int i = 0; i < mResLocList.Count; i++)
        {
            string assetPath = mResLocList[i].PrimaryKey;
            var obj = mAssetList[i];
            AddAsset(assetPath, obj);
        }

        mFunc?.Invoke();
    }

    public IEnumerator AsyncLoadManyAssetsByLabel(string label, Action mFunc = null)
    {
        if (!GameConst.orUseAssetBundle())
        {
            yield return new WaitForSeconds(1.0f);
            mFunc?.Invoke();
            yield break;
        }

        label = label.ToLower();

        var mResLocListHandle = Addressables.LoadResourceLocationsAsync(label);
        yield return mResLocListHandle;
        var mResLocList = mResLocListHandle.Result;
        Addressables.Release(mResLocListHandle);

        AsyncOperationHandle<IList<UnityEngine.Object>> mAssetListHandle = Addressables.LoadAssetsAsync<UnityEngine.Object>(mResLocList, null);
        yield return mAssetListHandle;
        var mAssetList = mAssetListHandle.Result;

        for (int i = 0; i < mResLocList.Count; i++)
        {
            string assetPath = mResLocList[i].PrimaryKey;
            var obj = mAssetList[i];
            AddAsset(assetPath, obj);
        }
        mFunc?.Invoke();
    }

    private void AddLableToResultDic(string assetPath)
    {
        string[] splitArray = assetPath.Split('/');
        string labelName = null;
        if (splitArray[2] == "theme")
        {
            labelName = splitArray[2] + splitArray[3];
        }
        else
        {
            labelName = splitArray[2];
        }

        List<string> mAssetList = null;
        if (!LabelDic.TryGetValue(labelName, out mAssetList))
        {
            mAssetList = new List<string>();
            LabelDic[labelName] = mAssetList;
        }
        mAssetList.Add(assetPath.ToLower());
    }

    private void AddAsset(string assetPath, UnityEngine.Object asset)
    {
        if (asset == null)
        {
            Debug.LogError("加载的资源为Null： " + assetPath);
            return;
        }

        assetPath = assetPath.ToLower();
        string assetName = asset.name.ToLower();
        if (assetName != Path.GetFileNameWithoutExtension(assetPath))
        {
            Debug.LogError("加载资源不一致： " + assetPath + " | " + assetName);
            return;
        }
        
        if (!orExistAsset(assetPath))
        {
            mAssetDic[assetPath] = asset;
            AddLableToResultDic(assetPath);
        }
    }

    public bool orExistAsset(string assetPath)
    {
        if (GameConst.orUseAssetBundle())
        {
            assetPath = assetPath.ToLower();
            return mAssetDic.ContainsKey(assetPath);
        }
        else
        {
            return EditorLoadAsset(assetPath) != null;
        }
    }

    public UnityEngine.Object GetAsset(string assetPath)
    {
        if (GameConst.orUseAssetBundle())
        {
            assetPath = assetPath.ToLower();
            
            if (!mAssetDic.ContainsKey(assetPath))
            {
                Debug.LogError("你没有 提前加载 这个资源哦: " + assetPath);
                return null;
            }

            if (mAssetDic.ContainsKey(assetPath) && mAssetDic[assetPath] == null)
            {
                Debug.LogError("某些操作导致内存被提前释放了，待检查: " + assetPath);
                return null;
            }
            
            return mAssetDic[assetPath];
        }
        else
        {
            return EditorLoadAsset(assetPath);
        }
    }

    public void RemoveAsset(string assetPath)
    {
        UnityEngine.Object asset = null;
        assetPath = assetPath.ToLower();
        if (mAssetDic.TryGetValue(assetPath, out asset))
        {
            if (!mAssetDic.Remove(assetPath))
            {
                Debug.LogError("mAssetDic Remove Failure: " + assetPath);
            }

            RelaseByReflection(asset);
        }
    }

    public void RemoveAssetByLabel(string label)
    {
        label = label.ToLower();
        if (LabelDic.ContainsKey(label))
        {
            List<string> mAssetList = LabelDic[label];
            foreach (var v in mAssetList)
            {
                RemoveAsset(v);
            }
            LabelDic.Remove(label);
        }
        else
        {
            if (label.StartsWith("theme"))
            {
                label = "theme/" + label.Substring(5);
            }

            string assetPathPrefix = GameConst.ResRootDirLower + label;
            List<string> mAssetList = new List<string>();
            foreach (var v in mAssetDic)
            {
                if (v.Key.StartsWith(assetPathPrefix))
                {
                    mAssetList.Add(v.Key);
                }
            }

            foreach (var v in mAssetList)
            {
                RemoveAsset(v);
            }
        }
    }

    // 暂时释放资源情况良好
    Dictionary<object, AsyncOperationHandle> m_resultToHandle_Value = null;
    readonly List<object> releaseKeyList = new List<object>();
    private void RelaseByReflection(UnityEngine.Object obj)
    {
        if (m_resultToHandle_Value == null)
        {
            m_resultToHandle_Value = GetReflectionRef();
        }

        releaseKeyList.Clear();
        foreach (var keyValue in m_resultToHandle_Value)
        {
            var k = keyValue.Key;
            var v = keyValue.Value;
            if (k is UnityEngine.Object)
            {
                var k1 = k as UnityEngine.Object;
                if (k1 == obj)
                {
                    releaseKeyList.Add(k);
                }
            }
            else if (k is IList<UnityEngine.Object>)
            {
                var k1 = k as IList<UnityEngine.Object>;
                if (k1.Contains(obj))
                {
                    k1.Remove(obj);
                    if (k1.Count == 0)
                    {
                        releaseKeyList.Add(k);
                    }
                }
            }
            else if (k is UnityEngine.ResourceManagement.ResourceProviders.SceneInstance)
            {
                
            }
            else
            {
                Debug.LogError("Error: " + k.GetType().Name);
            }
        }

        if (releaseKeyList.Count > 0)
        {
            foreach (var v in releaseKeyList)
            {
                PrintTool.LogWithColor("RelaseByReflection finalKey: " + v?.GetType().Name + " | " + obj.name);
                Addressables.Release(v);
            }

            releaseKeyList.Clear();
        }
    }
    
    private Dictionary<object, AsyncOperationHandle> GetReflectionRef()
    {
        var m_Addressables = typeof(Addressables).GetProperty("m_Addressables", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        var m_resultToHandle = m_Addressables.PropertyType.GetField("m_resultToHandle", BindingFlags.NonPublic | BindingFlags.Instance);
        var m_Addressables_Value = m_Addressables.GetValue(null);
        var m_resultToHandle_Value = m_resultToHandle.GetValue(m_Addressables_Value) as Dictionary<object, AsyncOperationHandle>;

        Debug.Assert(m_resultToHandle_Value != null, "m_resultToHandle_Value == null");
        return m_resultToHandle_Value;
    }

    private UnityEngine.Object EditorLoadAsset(string assetPath)
    {
#if UNITY_EDITOR
        var mAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
        //Debug.Assert(mAsset != null, assetPath);
        return mAsset;
#endif
        Debug.LogError("错误的使用 Editor方法");
        return null;
    }
}

