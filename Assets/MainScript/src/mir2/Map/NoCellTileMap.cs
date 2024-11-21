using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NoCellTileMap : MonoBehaviour
{
    private bool bInit = false;
    private SortingGroup mSortingGroup;
    public SpriteRenderer mItemPrefab;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (!bInit) return;
        bInit = true;
        mSortingGroup = GetComponent<SortingGroup>();
    }


}
