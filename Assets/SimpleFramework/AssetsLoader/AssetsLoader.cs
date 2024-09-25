using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class AssetsLoader : SingleTonMonoBehaviour<AssetsLoader>
{
    private Dictionary<string, UnityEngine.Object> mAssetDic = new Dictionary<string, UnityEngine.Object>();

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

        var mResLocListHandle = Addressables.LoadResourceLocationsAsync(assetPaths, Addressables.MergeMode.UseFirst);
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

    public IEnumerator AsyncDownloadAnLoadAssetsByLabel(string label, Action<float> mUpdateFunc = null, Action mFinishFunc = null)
    {
        if (!GameConst.orUseAssetBundle())
        {
            yield return new WaitForSeconds(1.0f);
            mUpdateFunc(1f);
            mFinishFunc?.Invoke();
            yield break;
        }

        label = label.ToLower();
        var mResLocListHandle = Addressables.LoadResourceLocationsAsync(label, typeof(UnityEngine.Object));
        yield return mResLocListHandle;
        var mResLocList = mResLocListHandle.Result;
        Addressables.Release(mResLocListHandle);

        int nAddAssetCount = 0;
        AsyncOperationHandle<IList<UnityEngine.Object>> mAssetListHandle = Addressables.LoadAssetsAsync<UnityEngine.Object>(label, (UnityEngine.Object obj)=>
        {
            nAddAssetCount++;
            float fPercent = nAddAssetCount / (float)mResLocList.Count;
            mUpdateFunc(fPercent);
        });
        yield return mAssetListHandle;

        var mAssetList = mAssetListHandle.Result;
        for (int i = 0; i < mResLocList.Count; i++)
        {
            string assetPath = mResLocList[i].PrimaryKey;
            var obj = mAssetList[i];
            AddAsset(assetPath, obj);
        }
        mFinishFunc?.Invoke();
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

        var mResLocListHandle = Addressables.LoadResourceLocationsAsync(label, typeof(UnityEngine.Object));
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
            return true;
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

    // 暂时释放资源情况良好
    Dictionary<object, AsyncOperationHandle> m_resultToHandle_Value = null;
    List<object> releaseKeyList = new List<object>();
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
                Debug.Log("RelaseByReflection finalKey: " + v?.GetType().Name + " | " + obj.name);
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
        return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
#endif
        Debug.LogError("错误的使用 Editor方法");
        return null;
    }
}

