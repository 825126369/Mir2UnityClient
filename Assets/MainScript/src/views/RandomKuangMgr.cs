using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RandomKuangPosInterface
{
    List<KuangItemData> GetKuangItemDataList();
}

public class RandomKuangMgr : Singleton<RandomKuangMgr>, RandomKuangPosInterface
{
    RandomKuangPosInterface mRandomInterface;

    public void Init()
    {
        mRandomInterface = new RandomKuangPos2();
        this.Test();
    }

    public List<KuangItemData> GetKuangItemDataList()
    {
        return mRandomInterface.GetKuangItemDataList();
    }

    private void Test()
    {
        for (int i = 0; i < 500; i++)
        {
            mRandomInterface.GetKuangItemDataList();
        }
    }
}
