using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FruitStallHelper
{
    public static int GetKuangSymbolCount(E_KUANG_TYPE nType)
    {
        if (nType == E_KUANG_TYPE.Length1)
        {
            return 1;
        }
        else if (nType == E_KUANG_TYPE.Length2)
        {
            return 2;
        }
        else if (nType == E_KUANG_TYPE.Length3)
        {
            return 3;
        }
        else if (nType == E_KUANG_TYPE.Length4)
        {
            return 4;
        }
        else if (nType == E_KUANG_TYPE.Length6)
        {
            return 5;
        }
        else
        {
            Debug.Assert(false);
        }

        return 0;
    }

    public static E_KUANG_TYPE GetKuangTypeByXLength(int nXLength)
    {
        if (nXLength == 3)
        {
            return E_KUANG_TYPE.Length1;
        }
        else if (nXLength == 4)
        {
            return E_KUANG_TYPE.Length2;
        }
        else if (nXLength == 6)
        {
            return E_KUANG_TYPE.Length3;
        }
        else if (nXLength == 8)
        {
            return E_KUANG_TYPE.Length4;
        }
        else if (nXLength == 12)
        {
            return E_KUANG_TYPE.Length6;
        }
        else
        {
            Debug.Assert(false, nXLength);
        }

        return E_KUANG_TYPE.None;
    }

    public static int GetKuangXLength(E_KUANG_TYPE nType)
    {
        if (nType == E_KUANG_TYPE.Length1)
        {
            return 3;
        }
        else if (nType == E_KUANG_TYPE.Length2)
        {
            return 4;
        }
        else if (nType == E_KUANG_TYPE.Length3)
        {
            return 6;
        }
        else if (nType == E_KUANG_TYPE.Length4)
        {
            return 8;
        }
        else if (nType == E_KUANG_TYPE.Length6)
        {
            return 12;
        }
        else
        {
            Debug.Assert(false);
        }

        return 0;
    }

    public static bool orTwoKuangOverlap(KuangItemData mData1, KuangItemData mData2)
    {
        for (int i = mData1.nPosX; i < mData1.nPosX + mData1.nLength; i++)
        {
            for (int j = mData2.nPosX; j < mData2.nPosX + mData2.nLength; j++)
            {
                if (i == j)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
