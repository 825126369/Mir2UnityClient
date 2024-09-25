using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KuangItem : MonoBehaviour
{
    public Image mKuangIcon;
    public RectTransform mSymbolParent;

    public KuangItemData mData;
    public List<SymbolItem> mSymbolItemList;

    public void Init(KuangItemData mData)
    {
        this.mData = mData;

        Debug.Assert(mData.nLength != 5);
        this.mKuangIcon.rectTransform.sizeDelta = new Vector2(MainGame.fItemWidth * mData.nLength, MainGame.fItemHeight);
        for (int i = 0; i < mData.nSymbolCount; i++)
        {
            CreateSymbolItem(i);
        }
    }

    public void RemoveSymbolItem(SymbolItem mItem)
    {
        mSymbolItemList.Remove(mItem);
    }

    private void CreateSymbolItem(int nPosX)
    {
        int nIconIndex = MainGame.readOnlyInstance.GetRandomSymbolIconIndex();
        SymbolItem mItem = ResCenter.readOnlyInstance.mSymbolItemPool.popObj();
        mItem.transform.SetParent(mSymbolParent);
        mItem.transform.localScale = Vector3.one;
        mItem.transform.localPosition = GetSymbolPos(nPosX);

        SymbolItemData mData = new SymbolItemData();
        mData.Init(nIconIndex);
        mItem.Init(this, mData);
        mSymbolItemList.Add(mItem);
    }

    private Vector3 GetSymbolPos(int nPosX)
    {
        float fSymbolWidth = MainGame.fItemWidth * 2f;
        float fMiddleX = (mData.nSymbolCount - 1) / 2f;
        float fWidth = mData.nLength / 2f / (float)mData.nSymbolCount * fSymbolWidth;
        return new Vector3(fWidth * nPosX + fWidth /2f, 0, 0);
    }
}
