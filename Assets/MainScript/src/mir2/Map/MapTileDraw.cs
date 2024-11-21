using UnityEngine;

namespace Mir2
{
    [RequireComponent(typeof(SpriteSetBlendMode))]
    [DisallowMultipleComponent]
    public class MapTileDraw : MonoBehaviour
    {
        SpriteRenderer mSpriteRenderer;
        SpriteSetBlendMode mSpriteSetBlendMode;
        bool bInit = false;
        MImage mMImage;

        private void Init()
        {
            if (bInit) return;
            bInit = true;
            mSpriteRenderer = GetComponent<SpriteRenderer>();
            mSpriteSetBlendMode = GetComponent<SpriteSetBlendMode>();
        }

        public void Clear()
        {
            mSpriteRenderer.sprite = null;
            this.mMImage = null;
        }

        public void SetData(Sprite mSprite, MImage mImage)
        {
            Init();
            mSpriteRenderer.sprite = mSprite;
            this.mMImage = mImage;
        }

        public void DrawBlend(Vector3 point, Color colour, bool offSet = false)
        {
            transform.position = point;
            if (offSet)
            {
                SetOffset();
            }
            mSpriteSetBlendMode.SetBlendMode(BlendMode.NORMAL);
        }

        public void DrawUpBlend(Vector3 point)
        {
            point += new Vector3(0, mMImage.Height);
            transform.position = point;
            mSpriteSetBlendMode.SetBlendMode(BlendMode.NORMAL);
        }

        public void DrawUp(Vector3 point)
        {
            point += new Vector3(0, mMImage.Height);
            transform.position = point;
        }

        public void Draw(Vector3 point)
        {
            transform.position = point;
        }

        private void SetOffset()
        {
            transform.localPosition = Vector3.zero + new Vector3(mMImage.X, -mMImage.Y);
        }
    }
}
