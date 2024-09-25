using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : SingleTonMonoBehaviour<MainGame>
{
    public GameObject goKuangPosOri;
    public RectTransform tranKuangParent;
    public RectTransform tranBottomSymbolParent;
    public List<Transform> mTranFlyEndList;

    public Text mCoinText;

    public GameObject goGameBeginMoveObj;
    public GameObject goCanvas_BottomObj;
    public GameObject goCanvas_TopObj;

    public const int nSymbolTypeCount = 16;
    public const int nXCount = 6;
    public const int nYCount = 30;
    public const float fItemHeight = 225f;
    public const float fItemWidth = 75f;
    public const int nXMaxLength = 12;
    public const int nXMaxSymbolCount = 6;

    public const int nMinSymbolCount = nSymbolTypeCount * 3;
    public const int nMaxSymbolCount = 6 * nYCount;
    public int nNowRandomIconIndex = 1;
    public int nSymbolId = 0;
    public bool bInSymbolFlyAni = false;

    private Vector3 mKuangLocalPosOri;
    private readonly List<KuangItem> mKuangItemList = new List<KuangItem>();
    private readonly List<SymbolItem> mBottomSymbolItemList = new List<SymbolItem>();

    private void Start()
    {
        GameBegin();
    }

    public void GameBegin()
    {
        InitParam();
        //随机框
        DoRefreshKuang();
        //随机水果

        mCoinText.text = DataCenter.Instance.nCoinCount.ToString();
        goCanvas_BottomObj.SetActive(false);
        goCanvas_TopObj.SetActive(false);
        goGameBeginMoveObj.transform.localPosition = new Vector3(0, -1000);
        UIMgr.readOnlyInstance.Show_GameBeginView();
    }

    public void PlayGameBeginMoveAni()
    {
        goCanvas_BottomObj.SetActive(false);
        goCanvas_TopObj.SetActive(false);
        goGameBeginMoveObj.transform.localPosition = new Vector3(0, -1000);
        LeanTween.moveLocalY(goGameBeginMoveObj, 0, 1f).setOnComplete(() =>
        {
            goCanvas_BottomObj.SetActive(true);
            goCanvas_TopObj.SetActive(true);
        });
    }

    public void DoGameEnd(bool bWin)
    {

        if (bWin == false)
        {
            if (DataCenter.readOnlyInstance.bShowResurrectionUI == false)
            {
                DataCenter.readOnlyInstance.bShowResurrectionUI = true;
                UIMgr.readOnlyInstance.Show_GameFail_Resurrection();
            }
            else
            {
                UIMgr.readOnlyInstance.Show_GameFailUI();
            }
        }
        else
        {
            UIMgr.readOnlyInstance.Show_GameWinUI();
        }
    }

    private void InitParam()
    {
        goKuangPosOri.SetActive(false);
        mKuangLocalPosOri = GameTools.WorldToUILocalPos(goKuangPosOri.transform.position, tranKuangParent) - new Vector3(fItemWidth * nXMaxLength / 2f, 0);

        foreach (var v1 in mKuangItemList)
        {
            foreach (var v2 in v1.mSymbolItemList)
            {
                ResCenter.readOnlyInstance.mSymbolItemPool.recycleObj(v2);
            }
            v1.mSymbolItemList.Clear();
            ResCenter.readOnlyInstance.mKuangItemPool.recycleObj(v1);
        }
        mKuangItemList.Clear();

        foreach (var v in mBottomSymbolItemList)
        {
            ResCenter.readOnlyInstance.mSymbolItemPool.recycleObj(v);
        }
        mBottomSymbolItemList.Clear();

        nNowRandomIconIndex = 1;
        bInSymbolFlyAni = false;
        DataCenter.readOnlyInstance.bShowResurrectionUI = false;

        RandomKuangMgr.Instance.Init();
    }

    private void DoRefreshKuang()
    {
        var mKuangList = RandomKuangMgr.Instance.GetKuangItemDataList();
        foreach (var v in mKuangList)
        {
            var mData = v;
            KuangItem mKuangItem = ResCenter.readOnlyInstance.mKuangItemPool.popObj();
            mKuangItem.transform.SetParent(tranKuangParent);
            mKuangItem.transform.localScale = Vector3.one;
            mKuangItem.transform.localPosition = GetKuangPos(mData);
            mKuangItem.name = $"Kuang_{mData.nPosY}_{mData.nPosX}_{mData.nLength}";

            mKuangItem.Init(mData);
            mKuangItemList.Add(mKuangItem);
        }
    }
    
    public Vector3 GetKuangPos(KuangItemData mData)
    {
        return mKuangLocalPosOri + new Vector3(fItemWidth * mData.nPosX, fItemHeight * mData.nPosY);
    }

    public int GetRandomSymbolIconIndex()
    {
        nNowRandomIconIndex++;
        if (nNowRandomIconIndex > nSymbolTypeCount)
        {
            nNowRandomIconIndex = 1;
        }
        return nNowRandomIconIndex;
    }

    public void DoClickSymbol(SymbolItem mItem)
    {
        if (orGameEnd() || bInSymbolFlyAni) return;

        Debug.Log(mItem.mData.id);

        int nTargetIndex = mBottomSymbolItemList.Count;
        mBottomSymbolItemList.Add(mItem);
        mItem.mInWhichKuang.RemoveSymbolItem(mItem);
        mItem.mInWhichKuang = null;
        Vector3 oriWorldPos = mItem.transform.position;

        ClearUpKuangItem();

        mItem.transform.SetParent(tranBottomSymbolParent, true);
        mItem.transform.position = oriWorldPos;
        mItem.transform.localScale = Vector3.one;
        mItem.gameObject.SetActive(true);

        Transform tranFlyEnd = mTranFlyEndList[nTargetIndex];
        Vector3 targetPos = GameTools.WorldToUILocalPos(tranFlyEnd.position, tranBottomSymbolParent);

        bInSymbolFlyAni = true;
        LeanTween.moveLocal(mItem.gameObject, targetPos, 0.3f).setOnComplete(() =>
        {
            bInSymbolFlyAni = false;

            int nIconIndex = GetNeedClearUpSymbol();
            bool bClearUp = nIconIndex > 0;
            if (bClearUp)
            {
                DoClearUpSymbol(nIconIndex);

                if (orWin())
                {
                    DoGameEnd(true);
                }
            }
            else
            {
                if (mBottomSymbolItemList.Count >= 8)
                {
                    DoGameEnd(false);
                }
            }
        });
    }

    private bool orGameEnd()
    {
        return orWin() || orFail();
    }

    private bool orFail()
    {
        return mBottomSymbolItemList.Count >= 8;
    }

    private bool orWin()
    {
        return mKuangItemList.Count == 0 && mBottomSymbolItemList.Count == 0;
    }

    int GetNeedClearUpSymbol()
    {
        int nContinueCount = 0;
        for (int i = 0; i < mBottomSymbolItemList.Count; i++)
        {
            SymbolItem mSeeSymbolItem = mBottomSymbolItemList[i];
            if (mSeeSymbolItem != null)
            {
                for (int j = i + 1; j < mBottomSymbolItemList.Count; j++)
                {
                    SymbolItem tempItem = mBottomSymbolItemList[j];
                    if (tempItem != null)
                    {
                        if (tempItem.mData.nIconIndex == mSeeSymbolItem.mData.nIconIndex)
                        {
                            nContinueCount++;
                            if (nContinueCount >= 3)
                            {
                                return mSeeSymbolItem.mData.nIconIndex;
                            }
                        }
                    }
                }
            }
        }

        return -1;
    }

    private void DoClearUpSymbol(int nIconIndex)
    {
        Debug.Log("当前消除符号：" + nIconIndex);
        for (int i = mBottomSymbolItemList.Count - 1; i >= 0; i--)
        {
            SymbolItem mSeeSymbolItem = mBottomSymbolItemList[i];
            if (mSeeSymbolItem != null && mSeeSymbolItem.mData.nIconIndex == nIconIndex)
            {
                mBottomSymbolItemList.RemoveAt(i);
                ResCenter.readOnlyInstance.mSymbolItemPool.recycleObj(mSeeSymbolItem);
            }
        }

        RefreshBottomSymbolPos();
    }

    private void RefreshBottomSymbolPos()
    {
        for (int i = mBottomSymbolItemList.Count - 1; i >= 0; i--)
        {
            SymbolItem mSeeSymbolItem = mBottomSymbolItemList[i];
            if (mSeeSymbolItem != null)
            {
                Transform tranFlyEnd = mTranFlyEndList[i];
                Vector3 targetPos = GameTools.WorldToUILocalPos(tranFlyEnd.position, tranBottomSymbolParent);
                mSeeSymbolItem.transform.localPosition = targetPos;
            }
        }
    }

    private void ClearUpKuangItem()
    {
        for (int i = 0; i < nYCount; i++)
        {
            List<KuangItem> mKuangList = mKuangItemList.FindAll((x) => i == x.mData.nPosY);
            foreach (var v1 in mKuangList)
            {
                if (v1.mSymbolItemList.Count == 0)
                {
                    mKuangItemList.Remove(v1);
                    ResCenter.readOnlyInstance.mKuangItemPool.recycleObj(v1);
                }
            }
        }

        DoKuangItemDown();
        PlayKuangItemDownAni();
    }

    private void DoKuangItemDown()
    {
        bool bExistDown = false;
        for (int i = 1; i < nYCount; i++)
        {
            List<KuangItem> mKuangList1 = mKuangItemList.FindAll((x) => i == x.mData.nPosY);
            List<KuangItem> mKuangList2 = mKuangItemList.FindAll((x) => i - 1 == x.mData.nPosY);

            foreach (var v1 in mKuangList1)
            {
                bool bDoDown = true;
                foreach (var v2 in mKuangList2)
                {
                    if (orKuangAndKuangOverlap(v1, v2))
                    {
                        bDoDown = false;
                        break;
                    }
                }

                if (bDoDown)
                {
                    bExistDown = true;
                    v1.mData.nPosY--;
                }
            }
        }

        if (bExistDown)
        {
            DoKuangItemDown();
        }
    }

    private void PlayKuangItemDownAni()
    {
        // 做动画
        foreach (var v in mKuangItemList)
        {
            Vector3 pos = GetKuangPos(v.mData);
            LeanTween.moveLocal(v.gameObject, pos, 0.3f);
        }
    }

    private bool orKuangAndKuangOverlap(KuangItem mKuangItemDown, KuangItem mKuangItemUp)
    {
        return FruitStallHelper.orTwoKuangOverlap(mKuangItemDown.mData, mKuangItemUp.mData);
    }
}






















