using System.Collections.Generic;
using UnityEngine;

public class SelectServerView : MonoBehaviour
{
    public SelectServerItem mItemPrefab; 

    private List<SelectServerItem> mItemList = new List<SelectServerItem>();
    private bool bInit = false;
    private void Init()
    {
        if (bInit) return;
        bInit = true;
    }

    public void Show()
    {
        Init();
        gameObject.SetActive(true);
        this.RefreshUI();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void RefreshUI()
    {
        mItemPrefab.gameObject.SetActive(false);
        var mServerItemDataList = DataCenter.Instance.mServerItemDataList;
        for (int i = 0; i < mServerItemDataList.Count; i++)
        {
            SelectServerItem mItem = null;
            if (i >= mItemList.Count)
            {
                var go = Instantiate(mItemPrefab.gameObject) as GameObject;
                go.transform.SetParent(mItemPrefab.transform.parent);
                go.transform.localScale = Vector3.one;
                mItem = go.GetComponent<SelectServerItem>();
                mItemList.Add(mItem);
            }
            else
            {
                mItem = mItemList[i];
            }

            mItem.gameObject.SetActive(true);
            var mData = mServerItemDataList[i];
            mItem.Refresh(mData);
        }

        for (int i = mServerItemDataList.Count; i < mItemList.Count; i++)
        {
            mItemList[i].gameObject.SetActive(false);
        }
    }

}
