using Client.MirObjects;
using UnityEngine;

public class MapCellItem_Front : MonoBehaviour
{
    public SpriteRenderer mSpriteRenderer;
    public CellInfo mData;
    private Map mMap;

    public void Init(Map mMap)
    {
        this.mMap = mMap;
    }

    public void Refresh(CellInfo mData, int x, int y)
    {
        this.mData = mData;
        mSpriteRenderer.sortingOrder = -9;
        if (mData.FrontImage >= 0)
        {
            Mir2Res.Instance.SetMapSprite(mSpriteRenderer, mData.FrontIndex, mData.FrontImage);
        }
        else
        {
            mSpriteRenderer.sprite = null;
        }

    }

}
