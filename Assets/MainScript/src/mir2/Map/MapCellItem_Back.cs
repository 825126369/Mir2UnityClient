using Client.MirObjects;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class MapCellItem_Back : MonoBehaviour
{
    public SpriteRenderer mSpriteRenderer;
    private CellInfo mData;
    private Map mMap;

    public void Init(Map mMap)
    {
        this.mMap = mMap;
    }

    public void Refresh(CellInfo mData, int x, int y)
    {
        this.mData = mData;

        mSpriteRenderer.sortingOrder = -11;
        if (x % 2 == 0 && y % 2 == 0 && mData.BackImage >= 0)
        {
            Mir2Res.Instance.SetMapSprite(mSpriteRenderer, mData.BackIndex, mData.BackImage);
        }
        else
        {
            mSpriteRenderer.sprite = null;
        }
    }

}
