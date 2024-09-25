using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResCenter : SingleTonMonoBehaviour<ResCenter>
{
    public CommonResSerialization mBundleGameAllRes;
    public NodeComponentPool<SymbolItem> mSymbolItemPool;
    public NodeComponentPool<KuangItem> mKuangItemPool;

    public IEnumerator AsyncInit()
    {
        var goPreafb = AssetsLoader.Instance.GetAsset("Assets/GameAssets/game/BundleAllRes.prefab") as GameObject;
        mBundleGameAllRes = goPreafb.GetComponent<CommonResSerialization>();

        GameObject goSymbolItem = mBundleGameAllRes.FindPrefab(PrefabNames.symbolPrefab);
        mSymbolItemPool = new NodeComponentPool<SymbolItem>();
        mSymbolItemPool.Init(goSymbolItem, 52);

        GameObject goKuangItem = mBundleGameAllRes.FindPrefab(PrefabNames.kuangPrefab);
        mKuangItemPool = new NodeComponentPool<KuangItem>();
        mKuangItemPool.Init(goKuangItem, 52);
        
        yield return null;

        StartCoroutine(LobbyAfter_LoadCardAnimationItem());
    }

    public IEnumerator LobbyAfter_LoadCardAnimationItem()
    {
        yield return null;
        mSymbolItemPool.preLoadObj(10, 100);
    }

}
