using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1:先随机所有框的位置
public class RandomKuangPos2:RandomKuangPosInterface
{
    private static readonly List<KuangItemData> mKuangItemDataList = new List<KuangItemData>();
    public List<KuangItemData> GetKuangItemDataList()
    {
        DoRandomKuang();
        DoFixKuangOverlap();
        DoFixSymbolCount();
        return mKuangItemDataList;
    }

    private void DoRandomKuang()
    {
        mKuangItemDataList.Clear();
        for (int i = 0; i < MainGame.nYCount; i++)
        {
            int nPosX = 0;
            while (nPosX < MainGame.nXMaxLength)
            {
                if (orEmpty())
                {
                    nPosX++;
                }
                else
                {
                    E_KUANG_TYPE nType = GetRandomKuangType(MainGame.nXMaxLength - nPosX);
                    if (nType != E_KUANG_TYPE.None)
                    {
                        int nLength = FruitStallHelper.GetKuangXLength(nType);
                        KuangItemData mData = new KuangItemData(nType, nPosX, i);
                        mKuangItemDataList.Add(mData);
                        nPosX += nLength;
                    }
                    else
                    {
                        nPosX++;
                    }
                }
            }
        }
    }

    private void DoFixKuangOverlap()
    {
        for (int i = 1; i < MainGame.nYCount; i++)
        {
            if (!CheckOverlap(i))
            {
                int nCheckPosY = i - 1;
                var mList = GetXKuangList(nCheckPosY);
                foreach (KuangItemData v in mList)
                {
                    mKuangItemDataList.Remove(v);
                }

                FillFull(nCheckPosY);
            }

            Debug.Assert(CheckOverlap(i), $"DoFixKuangOverlap Error: {i}, {i - 1}, {orXFillFull(i - 1)}");
        }

        //CheckKuangOverlap("DoFixKuangOverlap");
    }

    private void CheckKuangOverlap(string tag = "")
    {
        for (int i = 1; i < MainGame.nYCount; i++)
        {
            bool bOverlap = CheckOverlap(i);
            Debug.Assert(bOverlap, $"{tag} Error: {i}, {i - 1}, {orXFillFull(i - 1)}");
        }
    }

    private void DoFixSymbolCount()
    {
        int nLimitCount = RandomTool.RandomInt2(2, 4);
        List<int> mRandomYList = new List<int>();
        for (int i = 0; i < MainGame.nYCount; i++)
        {
            if (GetSymbolCount(i) <= nLimitCount)
            {
                mRandomYList.Add(i);
            }
        }

        while (GetSymbolCount() < MainGame.nMinSymbolCount)
        {
            int nRemoveIndex = RandomTool.RandomInt(0, mRandomYList.Count);
            int nPosY = mRandomYList[nRemoveIndex];
            mRandomYList.RemoveAt(nRemoveIndex);

            var mXList = GetXKuangList(nPosY);
            foreach (KuangItemData v in mXList)
            {
                mKuangItemDataList.Remove(v);
            }
            FillFull(nPosY);
        }

        int nNowSymbolCount = GetSymbolCount();
        if (nNowSymbolCount % 3 != 0)
        {
            int nNeedAddSymbolCount = 3 - nNowSymbolCount % 3;
            DoAddSymbolFunc(nNowSymbolCount + nNeedAddSymbolCount);
        }
    }

    private void DoAddSymbolFunc(int nTargetSymbolCount)
    {
        int nNeedAddSymbolCount = nTargetSymbolCount - GetSymbolCount();
        int nLoopCheckCount = 0;
        while (GetSymbolCount() < nTargetSymbolCount && nLoopCheckCount++ < 100)
        {
            List<int> mRandomYList = new List<int>();
            for (int i = 0; i < MainGame.nYCount; i++)
            {
                if (GetSymbolCount(i) < MainGame.nXMaxSymbolCount)
                {
                    mRandomYList.Add(i);
                }
            }

            while (mRandomYList.Count > 0 && nNeedAddSymbolCount > 0)
            {
                int nRemoveIndex = RandomTool.RandomInt(0, mRandomYList.Count);
                int nPosY = mRandomYList[nRemoveIndex];
                mRandomYList.RemoveAt(nRemoveIndex);

                List<KuangItemData> mXList = GetXKuangList(nPosY);

                mXList.Sort((x, y) =>
                {
                    return x.nPosX - y.nPosX;
                });

                if (mXList.Count > 1)
                {
                    Debug.Assert(mXList[0].nPosX < mXList[1].nPosX);
                }

                if (mXList[0].nType == E_KUANG_TYPE.Length6)
                {
                    Debug.Assert(GetSymbolCount(nPosY) == 5);
                    foreach (KuangItemData v in mXList)
                    {
                        mKuangItemDataList.Remove(v);
                    }
                    FillFull(nPosY, 6);
                    Debug.Assert(GetSymbolCount(nPosY) == 6);
                    nNeedAddSymbolCount--;
                }
                else
                {
                    List<KuangItemData> mOldList = new List<KuangItemData>();
                    List<KuangItemData> mNewList = new List<KuangItemData>();
                    for (int i = 0; i < mXList.Count; i++)
                    {
                        if (nNeedAddSymbolCount <= 0)
                        {
                            break;
                        }

                        if (mXList[i].nType == E_KUANG_TYPE.Length1)
                        {
                            if (
                                (i == 0 && mXList[i].nPosX - 1 >= 0) ||
                                (i - 1 >= 0 && mXList[i].nPosX - 1 >= mXList[i - 1].nPosX + mXList[i - 1].nLength)
                               )
                            {
                                mOldList.Add(mXList[i]);
                                int nNewPosX = mXList[i].nPosX - 1;
                                int nNewLength = 4;
                                mNewList.Add(new KuangItemData(nNewLength, nNewPosX, nPosY));
                                nNeedAddSymbolCount--;
                                break;
                            }

                            if (
                                (i == mXList.Count - 1 && mXList[i].nPosX + mXList[i].nLength + 1 <= MainGame.nXMaxLength) ||
                                (i + 1 < mXList.Count && mXList[i].nPosX + mXList[i].nLength + 1 <= mXList[i + 1].nPosX)
                               )
                            {
                                mOldList.Add(mXList[i]);
                                int nNewPosX = mXList[i].nPosX;
                                int nNewLength = 4;
                                mNewList.Add(new KuangItemData(nNewLength, nNewPosX, nPosY));
                                nNeedAddSymbolCount--;
                                break;
                            }
                        }
                        else
                        {
                            int nTempAddCount = RandomTool.RandomInt2(1, nNeedAddSymbolCount);
                            int nAddLength = nTempAddCount * 2;
                            if (
                                (i == 0 && mXList[i].nPosX - nAddLength >= 0) ||
                                (i > 0 && mXList[i].nPosX - nAddLength >= mXList[i - 1].nPosX + mXList[i - 1].nLength)
                               )
                            {
                                mOldList.Add(mXList[i]);
                                int nNewPosX = mXList[i].nPosX - nAddLength;
                                int nNewLength = mXList[i].nLength + nAddLength;
                                if (nNewLength == 10)
                                {
                                    var mList3 = UnPackLength10(nNewLength, nNewPosX, nPosY);
                                    mNewList.AddRange(mList3);
                                    PrintTool.LogWithColor("UnPackLength10: " + nPosY);
                                }
                                else
                                {
                                    mNewList.Add(new KuangItemData(nNewLength, nNewPosX, nPosY));
                                }

                                if (nNewLength == 12)
                                {
                                    nNeedAddSymbolCount--;
                                }
                                else
                                {
                                    nNeedAddSymbolCount -= nTempAddCount;
                                }
                                break;
                            }

                            if (
                                (i == mXList.Count - 1 && mXList[i].nPosX + mXList[i].nLength + nAddLength <= MainGame.nXMaxLength) ||
                                (i + 1 < mXList.Count && mXList[i].nPosX + mXList[i].nLength + nAddLength <= mXList[i + 1].nPosX)
                               )
                            {
                                mOldList.Add(mXList[i]);
                                int nNewPosX = mXList[i].nPosX;
                                int nNewLength = mXList[i].nLength + nAddLength;
                                if (nNewLength == 10)
                                {
                                    var mList3 = UnPackLength10(nNewLength, nNewPosX, nPosY);
                                    mNewList.AddRange(mList3);
                                    PrintTool.LogWithColor("UnPackLength10: " + nPosY);
                                }
                                else
                                {
                                    mNewList.Add(new KuangItemData(nNewLength, nNewPosX, nPosY));
                                }

                                if (nNewLength == 12)
                                {
                                    nNeedAddSymbolCount--;
                                }
                                else
                                {
                                    nNeedAddSymbolCount -= nTempAddCount;
                                }
                                break;
                            }
                        }
                    }

                    foreach (KuangItemData v in mOldList)
                    {
                        mKuangItemDataList.Remove(v);
                    }

                    foreach (KuangItemData v in mNewList)
                    {
                        mKuangItemDataList.Add(v);
                    }
                }
                
                if (!CheckOverlap(nPosY))
                {
                    var mList = GetXKuangList(nPosY - 1);
                    int nSymbolCount = GetSymbolCount(mList);
                    int nOriSymbolCount = nSymbolCount;
                    foreach (KuangItemData v in mList)
                    {
                        mKuangItemDataList.Remove(v);
                    }

                    if (nSymbolCount < 4)
                    {
                        nSymbolCount += 3;
                        nTargetSymbolCount += 3;
                    }
                    Debug.Assert(nSymbolCount >= 4, nOriSymbolCount + ", " + nSymbolCount);
                    FillFull(nPosY - 1, nSymbolCount);
                    Debug.Assert(GetSymbolCount(nPosY - 1) == nSymbolCount, nSymbolCount + ", " + GetSymbolCount(nPosY - 1));
                    PrintTool.LogWithColor("UnPackLength10 CheckOverlap Op: " + nSymbolCount);
                }
            }
        }

        if (nLoopCheckCount >= 100)
        {
            Debug.LogError("loop many!!!  " + nNeedAddSymbolCount);
        }

        Debug.Assert(GetSymbolCount() % 3 == 0);
        if (GetSymbolCount() != nTargetSymbolCount)
        {
            PrintTool.LogError($"Final nTargetSymbolCount: {nTargetSymbolCount}, {GetSymbolCount()}, {nNeedAddSymbolCount}");
        }
        CheckKuangOverlap("DoFixSymbolCount");
    }

    private List<KuangItemData> UnPackLength10(int nLength, int nPosX, int nPosY)
    {
        List<KuangItemData> mList = new List<KuangItemData>();
        Debug.Assert(nLength == 10, nLength);
        if (RandomTool.RandomInt2(1, 2) == 1)
        {
            mList.Add(new KuangItemData(E_KUANG_TYPE.Length2, nPosX, nPosY));
            mList.Add(new KuangItemData(E_KUANG_TYPE.Length3, nPosX + 4, nPosY));
        }
        else
        {
            mList.Add(new KuangItemData(E_KUANG_TYPE.Length3, nPosX, nPosY));
            mList.Add(new KuangItemData(E_KUANG_TYPE.Length2, nPosX + 6, nPosY));
        }
        return mList;
    }

    private void FillFull(int nPosY, int nSumSymbolCount = -1)
    {
        var mList = FindList_For_FillFullAuto(nPosY, nSumSymbolCount);
        if (mList != null)
        {
            foreach (var v in mList)
            {
                mKuangItemDataList.Add(v);
            }
        }
        else if (nSumSymbolCount > 0)
        {
            if (nSumSymbolCount == 6)
            {
                FillFullWith6(nPosY);
            }
            else if (nSumSymbolCount == 5)
            {
                FillFullWith5(nPosY);
            }
            else if (nSumSymbolCount == 4)
            {
                FillFullWith4(nPosY);
            }
            else
            {
                Debug.Assert(false, nSumSymbolCount);
            }
        }
        else
        {
            int nRandom = RandomTool.RandomInt2(1, 3);
            if (nRandom == 1)
            {
                FillFullWith6(nPosY);
            }
            else if (nRandom == 2)
            {
                FillFullWith5(nPosY);
            }
            else if (nRandom == 3)
            {
                FillFullWith4(nPosY);
            }
        }
    }

    private void FillFullWith6(int nPosY)
    {
        int nRandom = RandomTool.RandomInt2(1, 3);
        if (nRandom == 1)
        {
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length3, 0, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length3, 6, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
        else if (nRandom == 2)
        {
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length2, 0, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length4, 4, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
        else if (nRandom == 3)
        {
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length4, 0, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length2, 8, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
    }

    private void FillFullWith5(int nPosY)
    {
        int nRandom = RandomTool.RandomInt2(1, 5);
        if (nRandom == 1)
        {
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length6, 0, nPosY);
            mKuangItemDataList.Add(mData1);
        }
        else if (nRandom == 2)
        {
            int nRandom2 = RandomTool.RandomInt2(0, 2);
            int nRandom3 = RandomTool.RandomInt2(nRandom2 + 4, 12 - 6);
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length2, nRandom2, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length3, nRandom3, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);

        }
        else if (nRandom == 3)
        {
            int nRandom2 = RandomTool.RandomInt2(0, 2);
            int nRandom3 = RandomTool.RandomInt2(nRandom2 + 6, 12 - 4);
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length3, nRandom2, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length2, nRandom3, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
        else if (nRandom == 4)
        {
            int nRandom2 = RandomTool.RandomInt2(0, 1);
            int nRandom3 = RandomTool.RandomInt2(nRandom2 + 8, 12 - 3);
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length4, nRandom2, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length1, nRandom3, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
        else if (nRandom == 5)
        {
            int nRandom2 = RandomTool.RandomInt2(0, 1);
            int nRandom3 = RandomTool.RandomInt2(nRandom2 + 3, 12 - 8);
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length1, nRandom2, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length4, nRandom3, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
    }

    private void FillFullWith4(int nPosY)
    {
        int nRandom = RandomTool.RandomInt2(1, 7);
        if (nRandom == 1)
        {
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length4, 2, nPosY);
            mKuangItemDataList.Add(mData1);
        }
        else if (nRandom == 2)
        {
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length2, 2, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length2, 6, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
        else if (nRandom == 3)
        {
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length1, 0, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length1, 3, nPosY);
            var mData3 = new KuangItemData(E_KUANG_TYPE.Length1, 6, nPosY);
            var mData4 = new KuangItemData(E_KUANG_TYPE.Length1, 9, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
            mKuangItemDataList.Add(mData3);
            mKuangItemDataList.Add(mData4);
        }
        else if (nRandom == 4)
        {
            int nRandom2 = RandomTool.RandomInt2(0, 2);
            int nRandom3 = RandomTool.RandomInt2(nRandom2 + 4, 12 - 6);
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length1, nRandom2, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length3, nRandom3, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
        else if (nRandom == 5)
        {
            int nRandom2 = RandomTool.RandomInt2(0, 2);
            int nRandom3 = RandomTool.RandomInt2(nRandom2 + 7, 12 - 3);
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length3, nRandom2, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length1, nRandom3, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
        else if (nRandom == 6)
        {
            int nOffsetX = RandomTool.RandomInt2(1, 3);
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length1, nOffsetX, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length3, 3 + nOffsetX, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
        else if (nRandom == 7)
        {
            int nOffsetX = RandomTool.RandomInt2(1, 3);
            var mData1 = new KuangItemData(E_KUANG_TYPE.Length3, nOffsetX, nPosY);
            var mData2 = new KuangItemData(E_KUANG_TYPE.Length1, 6 + nOffsetX, nPosY);
            mKuangItemDataList.Add(mData1);
            mKuangItemDataList.Add(mData2);
        }
    }

    private List<KuangItemData> FindList_For_FillFullAuto(int nPosY, int nSumSymbolCount = -1)
    {
        List<KuangItemData> mList = new List<KuangItemData>();
        int nLoopCount = 0;
        while (nLoopCount++ < 100)
        {
            int nPosX = 0;
            int nContinueEmptyCount = 0;
            mList.Clear();
            while (nPosX < MainGame.nXMaxLength)
            {
                if (RandomTool.RandomInt2(1, 2) == 1 && nContinueEmptyCount < 2)
                {
                    nPosX++;
                    nContinueEmptyCount++;
                }
                else
                {
                    int nMaxLength = MainGame.nXMaxLength - nPosX;
                    int nMaxSymbolCount = 6;
                    if (nSumSymbolCount > 0)
                    {
                        nMaxSymbolCount = nSumSymbolCount - GetSymbolCount(mList);
                    }

                    E_KUANG_TYPE nType = GetRandomKuangType(nMaxLength, nMaxSymbolCount);
                    if (nType != E_KUANG_TYPE.None)
                    {
                        int nLength = FruitStallHelper.GetKuangXLength(nType);
                        KuangItemData mData = new KuangItemData(nType, nPosX, nPosY);
                        mList.Add(mData);
                        nPosX += nLength;

                        nContinueEmptyCount = 0;
                    }
                    else
                    {
                        nPosX++;
                        nContinueEmptyCount++;
                    }
                }
            }

            if (orXFillFull(mList, nSumSymbolCount))
            {
                return mList;
            }
        }

        Debug.Log("FindList_For_FillFullAuto == null");
        return null;
    }

    private bool orXFillFull(int nPosY)
    {
        return orXFillFull(GetXKuangList(nPosY));
    }

    private bool orXFillFull(List<KuangItemData> mList, int nSumSymbolCount = -1)
    {
        bool bFillFull = true;
        mList.Sort((x, y) =>
        {
            return x.nPosX - y.nPosX;
        });

        for (int t = 0; t < mList.Count; t++)
        {
            KuangItemData current = mList[t];
            if (t == 0)
            {
                if (current.nPosX - 0 >= 3)
                {
                    bFillFull = false;
                    break;
                }
            }

            if (t == mList.Count - 1)
            {
                if (MainGame.nXMaxLength - (current.nPosX + current.nLength) >= 3)
                {
                    bFillFull = false;
                    break;
                }
            }

            if (t - 1 >= 0 && current.nPosX - (mList[t - 1].nPosX + mList[t - 1].nLength) >= 3)
            {
                bFillFull = false;
                break;
            }

            if (t + 1 < mList.Count && mList[t + 1].nPosX - (current.nPosX + current.nLength) >= 3)
            {
                bFillFull = false;
                break;
            }

        }

        if (nSumSymbolCount > 0)
        {
            return bFillFull && GetSymbolCount(mList) == nSumSymbolCount;
        }
        else
        {
            return bFillFull;
        }
    }

    private bool CheckOverlap(int nPosY_Up)
    {
        if (nPosY_Up == 0) return true;

        var mList_Up = mKuangItemDataList.FindAll((x) => x.nPosY == nPosY_Up);
        var mList_Down = mKuangItemDataList.FindAll((x) => x.nPosY == nPosY_Up - 1);

        foreach (KuangItemData v2 in mList_Up)
        {
            bool bOverlap = false;
            foreach (KuangItemData v1 in mList_Down)
            {
                if (FruitStallHelper.orTwoKuangOverlap(v1, v2))
                {
                    bOverlap = true;
                    break;
                }
            }

            if (!bOverlap)
            {
                return false;
            }
        }
        return true;
    }

    private List<KuangItemData> GetXKuangList(int nPosY)
    {
        var mList = mKuangItemDataList.FindAll((x) => x.nPosY == nPosY);
        return mList;
    }

    private int GetSymbolCount()
    {
        return GetSymbolCount(mKuangItemDataList);
    }

    private int GetSymbolCount(int nPosY)
    {
        var mList = mKuangItemDataList.FindAll((x) => x.nPosY == nPosY);
        return GetSymbolCount(mList);
    }

    private int GetSymbolCount(List<KuangItemData> mList)
    {
        int nSum = 0;
        foreach (var v in mList)
        {
            nSum += v.nSymbolCount;
        }
        return nSum;
    }

    private bool orEmpty()
    {
        return RandomTool.RandomInt2(1, 100) <= 50;
    }

    private E_KUANG_TYPE GetRandomKuangType(int nMaxLength = -1, int nMaxSymbolCount = -1)
    {
        List<E_KUANG_TYPE> mList = new List<E_KUANG_TYPE>() {
            E_KUANG_TYPE.Length1,
            E_KUANG_TYPE.Length2,
            E_KUANG_TYPE.Length3,
            E_KUANG_TYPE.Length4,
            E_KUANG_TYPE.Length6,
        };

        for (int i = mList.Count - 1; i >= 0; i--)
        {
            E_KUANG_TYPE nType = mList[i];
            if (nMaxLength > 0 && FruitStallHelper.GetKuangXLength(nType) > nMaxLength)
            {
                mList.RemoveAt(i);
            }
            else if (nMaxSymbolCount > 0 && FruitStallHelper.GetKuangSymbolCount(nType) > nMaxSymbolCount)
            {
                mList.RemoveAt(i);
            }
        }

        if (mList.Count > 0)
        {
            int nIndex = RandomTool.RandomInt(0, mList.Count);
            return mList[nIndex];
        }

        return E_KUANG_TYPE.None;
    }
}
