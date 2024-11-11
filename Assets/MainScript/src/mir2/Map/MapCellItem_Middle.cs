using CrystalMir2;
using UnityEngine;

namespace Mir2
{
    public class MapCellItem_Middle : MonoBehaviour
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
            mSpriteRenderer.sortingOrder = -10;
            if (mData.MiddleImage >= 0)
            {
                Mir2Res.Instance.SetMapSprite(mSpriteRenderer, mData.MiddleIndex, mData.MiddleImage);
            }
            else
            {
                mSpriteRenderer.sprite = null;
            }
        }
    }
}
