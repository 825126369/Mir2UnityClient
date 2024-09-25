using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//先随机所有符号的位置
public class RandomKuangPos1 : RandomKuangPosInterface
{
    readonly List<Vector2Int> mSymbolPosList = new List<Vector2Int>();
    readonly List<KuangItemData> mKuangPosList = new List<KuangItemData>();

    public List<KuangItemData> GetKuangItemDataList()
    {
        return mKuangPosList;
    }
    
    private List<int> GetRowRandomPosList(int nPosY)
    {
        List<int> mRandSymbolPosIndex = new List<int> { 0, 1, 2, 3, 4, 5 };
        int nNeedSymbolCount = RandomTool.RandomInt(1, 7);

        List<int> mPosList = new List<int>();
        while(nNeedSymbolCount > 0 && mRandSymbolPosIndex.Count > 0)
        {
            nNeedSymbolCount--;

            int nIndex = RandomTool.RandomInt(0, mRandSymbolPosIndex.Count);
            int nPos = mRandSymbolPosIndex[nIndex];
            mPosList.Add(nPos);
            mRandSymbolPosIndex.RemoveAt(nIndex);
        }

        return mPosList;
    }

    private bool orMeAndAdjacentOk(int nPosX, int nPosY)
    {
        if (nPosY == 0) return true;

        for (int i = nPosX; i < MainGame.nXCount; i++)
        {
            if (orHaveSymbol(i, nPosY))
            {
                if (orHaveSymbol(i, nPosY - 1))
                {
                    return true;
                }
            }
        }

        for (int i = nPosX; i >= 0; i--)
        {
            if (orHaveSymbol(i, nPosY))
            {
                if (orHaveSymbol(i, nPosY - 1))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    private List<Vector2Int> GetCanFillSymbolPosList()
    {
        List<Vector2Int> mPosList = new List<Vector2Int>();
        for (int j = 0; j < MainGame.nYCount; j++)
        {
            for (int i = 0; i < MainGame.nXCount; i++)
            {
                if (!orHaveSymbol(i, j))
                {
                    if (i - 1 >= 0 && orHaveSymbol(i - 1, j) || i + 1 < MainGame.nXCount && orHaveSymbol(i + 1, j) || orHaveSymbol(i, j - 1))
                    {
                        mPosList.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        return mPosList;
    }

    private List<Vector2Int> GetCanDeleteSymbolPosList()
    {
        List<Vector2Int> mPosList = new List<Vector2Int>();
        for (int j = 0; j < MainGame.nYCount; j++)
        {
            for (int i = 0; i < MainGame.nXCount; i++)
            {
                if (orHaveSymbol(i, j) && !orHaveSymbol(i, j - 1))
                {
                    if (i - 1 >= 0 && !orHaveSymbol(i - 1, j) || i + 1 < MainGame.nXCount && !orHaveSymbol(i + 1, j))
                    {
                        mPosList.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        return mPosList;
    }

    public void DoRandomAll()
    {
        mSymbolPosList.Clear();
        mKuangPosList.Clear();

        for (int j = 0; j < MainGame.nYCount; j++)
        {
            var mPosList = GetRowRandomPosList(j);
            for (int i = 0; i < MainGame.nXCount; i++)
            {
                if (mPosList.Contains(i))
                {
                    mSymbolPosList.Add(new Vector2Int(i, j));
                }
            }
        }

        for (int j = MainGame.nYCount - 1; j >= 0; j--)
        {
            for (int i = 0; i < MainGame.nXCount; i++)
            {
                if (!orMeAndAdjacentOk(i, j))
                {
                    mSymbolPosList.RemoveAll((x) => x.y == j && x.x == i);
                }
            }
        }

        //------凑够48个符号以及3的整数倍
        int mNeedExraKengCount = 0;

        if (mSymbolPosList.Count < MainGame.nMinSymbolCount)
        {
            mNeedExraKengCount += RandomTool.RandomInt(MainGame.nMinSymbolCount / 3, MainGame.nMaxSymbolCount / 3) * 3 - mSymbolPosList.Count;
        }
        else if(mSymbolPosList.Count % 3 != 0)
        {
            mNeedExraKengCount = 3 - mSymbolPosList.Count % 3;
            if (mSymbolPosList.Count >= MainGame.nMaxSymbolCount - 3 * 5)
            {
                mNeedExraKengCount = -mSymbolPosList.Count % 3;
            }
        }

        Debug.Log("mNeedExraKengCount: " + mNeedExraKengCount);
        if (mNeedExraKengCount > 0)
        {
            while (mNeedExraKengCount > 0)
            {
                mNeedExraKengCount--;

                List<Vector2Int> mNoSymbolPosList = GetCanFillSymbolPosList();
                int nIndex = RandomTool.RandomInt(0, mNoSymbolPosList.Count);
                mSymbolPosList.Add(mNoSymbolPosList[nIndex]);
            }
        }
        else if(mNeedExraKengCount < 0)
        {
            while (mNeedExraKengCount < 0)
            {
                mNeedExraKengCount++;

                List<Vector2Int> mNoSymbolPosList = GetCanDeleteSymbolPosList();
                int nIndex = RandomTool.RandomInt(0, mNoSymbolPosList.Count);
                mSymbolPosList.Remove(mNoSymbolPosList[nIndex]);
            }
        }

        Debug.Assert(mSymbolPosList.Count % 3 == 0, mSymbolPosList.Count);

        // 产生合格的框
        for (int j = 0; j < MainGame.nYCount; j++)
        {
            bool bHaveKuang = false;
            int nBeginPosX = -1;
            int nEndPosX = -1;
            nRowKuangCount = 0;
            for (int i = 0; i < MainGame.nXCount; i++)
            {
                if (bHaveKuang == false)
                {
                    bHaveKuang = orHaveKuangBySymbol(i, j);
                    if (bHaveKuang)
                    {
                        nBeginPosX = i;
                    }
                }
                else
                {
                    bHaveKuang = orHaveKuangBySymbol(i, j);
                    if (bHaveKuang == false || i == MainGame.nXCount - 1)
                    {
                        nEndPosX = i - 1;
                        int nLength = nEndPosX - nBeginPosX + 1;
                        CreateKuangBySymbolCount(nLength, nBeginPosX, j);
                        nBeginPosX = -1;
                        nEndPosX = -1;

                    }
                }
            }

            if (nRowKuangCount == 0)
            {
                Debug.Log("nTestCount: " + ++nTestCount);
                var nCount = FindRowSymbolPosList(j).Count;
                mSymbolPosList.RemoveAll((x) => x.y == j);

                int nLength = nCount + 3 > 6 ? nCount : nCount + 3;

                List<int> mRandomPosX = new List<int> { 0, 1, 2, 3, 4, 5 };
                int nSelectPosX = -1;
                while(true)
                {
                    int nIndex2 = RandomTool.RandomInt(0, mRandomPosX.Count);
                    Debug.Assert(nIndex2 >= 0 && nIndex2 < mRandomPosX.Count, FindRowSymbolPosList(j - 1).Count + " | " + nLength);

                    if(nIndex2 == 0)
                    {
                        Debug.LogError(string.Concat(FindRowSymbolPosList(j - 1)));
                        Debug.LogError("orKuangOk: " + orKuangOk(new KuangItemData(0, j, nLength)));
                    }

                    int nPosX = mRandomPosX[nIndex2];
                    mRandomPosX.RemoveAt(nIndex2);

                    if(nPosX + nLength <= MainGame.nXCount && orKuangOk(new KuangItemData(nPosX,j,nLength)))
                    {
                        nSelectPosX = nPosX;
                        break;
                    }
                }
                
                for (int i = nSelectPosX; i < nSelectPosX + nLength; i++)
                {
                    mSymbolPosList.Add(new Vector2Int(i, j));
                }
                Debug.Assert(mSymbolPosList.Count % 3 == 0);
                CreateKuangBySymbolCount(nLength, nSelectPosX, j);
            }

        }

        foreach(var v in mKuangPosList)
        {
            if(!orKuangOk(v))
            {
                Debug.LogError("框没修好： " + v.ToString());
            }
        }
    }
    private int nTestCount = 0;

    private void CreateKuangBySymbolCount(int nSymbolCount, int nBeginPosX, int nPosY)
    {
        int nLength = nSymbolCount;
        int j = nPosY;
        if (nLength == 6)
        {
            List<int> mRandomList = new List<int> { 1, 2, 3, 4 };
            while (mRandomList.Count > 0)
            {
                int nRandomIndex = RandomTool.RandomInt(0, mRandomList.Count);
                int nRandomType = mRandomList[nRandomIndex];
                mRandomList.RemoveAt(nRandomIndex);

                if (nRandomType == 1)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 3), new KuangItemData(nBeginPosX + 3, j, 3)))
                    {
                        break;
                    }
                }
                else if (nRandomType == 2)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 2), new KuangItemData(nBeginPosX + 2, j, 2), new KuangItemData(nBeginPosX + 4, j, 2)))
                    {
                        break;
                    }
                }
                else if (nRandomType == 3)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 2), new KuangItemData(nBeginPosX + 2, j, 4)))
                    {
                        break;
                    }
                }
                else if (nRandomType == 4)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 4), new KuangItemData(nBeginPosX + 4, j, 2)))
                    {
                        break;
                    }
                }
            }
        }
        else if (nLength == 5)
        {
            List<int> mRandomList = new List<int> { 1, 2, 3 };
            while (mRandomList.Count > 0)
            {
                int nRandomIndex = RandomTool.RandomInt(0, mRandomList.Count);
                int nRandomType = mRandomList[nRandomIndex];
                mRandomList.RemoveAt(nRandomIndex);

                if (nRandomType == 1)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 6)))
                    {
                        break;
                    }
                }
                else if (nRandomType == 2)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 3), new KuangItemData(nBeginPosX + 3, j, 2)))
                    {
                        break;
                    }
                }
                else if (nRandomType == 3)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 2), new KuangItemData(nBeginPosX + 2, j, 3)))
                    {
                        break;
                    }
                }
            }
        }
        else if (nLength == 4)
        {
            List<int> mRandomList = new List<int> { 1, 2 };
            while (mRandomList.Count > 0)
            {
                int nRandomIndex = RandomTool.RandomInt(0, mRandomList.Count);
                int nRandomType = mRandomList[nRandomIndex];
                mRandomList.RemoveAt(nRandomIndex);

                if (nRandomType == 1)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 4)))
                    {
                        break;
                    }
                }
                else if (nRandomType == 2)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 2), new KuangItemData(nBeginPosX + 2, j, 2)))
                    {
                        break;
                    }
                }
            }
        }
        else if (nLength == 3)
        {
            List<int> mRandomList = new List<int> { 1 };
            while (mRandomList.Count > 0)
            {
                int nRandomIndex = RandomTool.RandomInt(0, mRandomList.Count);
                int nRandomType = mRandomList[nRandomIndex];
                mRandomList.RemoveAt(nRandomIndex);

                if (nRandomType == 1)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX, j, 3)))
                    {
                        break;
                    }
                }
            }
        }
        else if (nLength == 2)
        {
            List<int> mRandomList = new List<int> { 1 };
            while (mRandomList.Count > 0)
            {
                int nRandomIndex = RandomTool.RandomInt(0, mRandomList.Count);
                int nRandomType = mRandomList[nRandomIndex];
                mRandomList.RemoveAt(nRandomIndex);

                if (nRandomType == 1)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX + 0, j, 2)))
                    {
                        break;
                    }
                }
            }
        }
        else if (nLength == 1)
        {
            List<int> mRandomList = new List<int> { 1 };
            while (mRandomList.Count > 0)
            {
                int nRandomIndex = RandomTool.RandomInt(0, mRandomList.Count);
                int nRandomType = mRandomList[nRandomIndex];
                mRandomList.RemoveAt(nRandomIndex);

                if (nRandomType == 1)
                {
                    if (JudgeKuangOkAndAdd(new KuangItemData(nBeginPosX + 0, j, 1)))
                    {
                        break;
                    }
                }
            }
        }
    }

    private int nRowKuangCount = 0;
    private bool JudgeKuangOkAndAdd(params KuangItemData[] mList)
    {
        bool Ok = true;
        foreach(var v  in mList)
        {
            if(orKuangOk(v) == false)
            {
                Ok = false;
                break;
            }
        }

        if(Ok == true)
        {
            foreach (var v in mList)
            {
                mKuangPosList.Add(v);
                nRowKuangCount++;
            }
        }

        return Ok;
    }

    private List<Vector2Int> FindRowSymbolPosList(int nPosY)
    {
        return mSymbolPosList.FindAll((mItem) =>
        {
            return mItem.y == nPosY;
        });
    }
    
    private bool orKuangOk(KuangItemData mKuangData)
    {
        if (mKuangData.nPosY > 0)
        {
            var mKuangDownList = mKuangPosList.FindAll((mItem) =>
            {
                return mItem.nPosY == mKuangData.nPosY - 1;
            });
            
            foreach (var v in mKuangDownList)
            {
                if (FruitStallHelper.orTwoKuangOverlap(mKuangData, v))
                {
                    return true;
                }
            }
               
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool orHaveKuangBySymbol(int nX, int nY)
    {
        return mSymbolPosList.FindIndex((x) => x.x == nX && x.y == nY) >= 0;
    }

    private bool orHaveSymbol(int nX, int nY)
    {
        return mSymbolPosList.FindIndex((x) => x.x == nX && x.y == nY) >= 0;
    }
}
