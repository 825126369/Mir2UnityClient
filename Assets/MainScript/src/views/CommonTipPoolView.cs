using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonTipPoolView : MonoBehaviour 
{
    public CommonTipPoolViewItem n_ItemPrefab = null;
    NodeComponentPool<CommonTipPoolViewItem> mItemPoolList = null;
    bool m_bInit = false;
    public int nItemId = 1;

    private void Init()
    {
        if(this.m_bInit) return;
        this.m_bInit = true;
        this.gameObject.SetActive(true);
        this.mItemPoolList = new NodeComponentPool<CommonTipPoolViewItem>();
        this.mItemPoolList.Init(this.n_ItemPrefab.gameObject);
    }

    public void Show(string des)
    {
        this.Init();
        var mItem = this.mItemPoolList.popObj();
        this.nItemId++;
        mItem.Show(des, this.nItemId);
    }

    public void RecycleItem(CommonTipPoolViewItem mItem)
    {
        this.mItemPoolList.recycleObj(mItem);
    }
}

