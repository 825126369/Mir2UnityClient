using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_KUANG_TYPE
{
    None = 0,
    Length1 = 1,
    Length2 = 2,
    Length3 = 3,
    Length4 = 4,
    Length6 = 5,
}

public class KuangItemData
{
    public E_KUANG_TYPE nType;
    public int nPosY;
    public int nPosX;
    public int nLength;
    public int nSymbolCount;

    public KuangItemData()
    {

    }

    public KuangItemData(KuangItemData other)
    {
        this.Init(other.nType, other.nPosX, other.nPosY);
    }

    public KuangItemData(E_KUANG_TYPE nType, int nPosX, int nPosY)
    {
        this.Init(nType, nPosX, nPosY);
    }

    public KuangItemData(int nLength, int nPosX, int nPosY)
    {
        this.Init(FruitStallHelper.GetKuangTypeByXLength(nLength), nPosX, nPosY);
    }

    public void Init(E_KUANG_TYPE nType, int nPosX, int nPosY)
    {
        this.nType = nType;
        this.nPosY = nPosY;
        this.nPosX = nPosX;
        this.nLength = FruitStallHelper.GetKuangXLength(nType);
        this.nSymbolCount = FruitStallHelper.GetKuangSymbolCount(nType);
    }
}

public class SymbolItemData
{
    public int nIconIndex;
    public int id;
    public void Init(int nIconIndex)
    {
        this.nIconIndex = nIconIndex;
        id = MainGame.readOnlyInstance.nSymbolId++;
    }
}
