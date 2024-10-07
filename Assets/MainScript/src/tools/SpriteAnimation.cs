using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public string AssetPathDir;
    public string PrefixName = "";
    public int nBeginFrameIndex = 0;
    public int nCount = 0;
    public float fInternalTime = 0.03f;

    [SerializeField] List<Sprite> mSpriteList = new List<Sprite>();

    private int nAniIndex = 0;
    private SpriteRenderer mSpriteRenderer;
    private float mLastAniDrawTime;
    private void Start()
    {
        InitSpriteList();
        nAniIndex = 0;
        DrawNext();
        mLastAniDrawTime = Time.time;
    }

    private void InitSpriteList()
    {
        if (mSpriteList.Count != nCount)
        {
            mSpriteList.Clear();
            for (int i = 0; i < nCount; i++)
            {
                Sprite mSprite = AssetsLoader.Instance.GetAsset(AssetPathDir + PrefixName + i + nBeginFrameIndex) as Sprite;
                mSpriteList.Add(mSprite);
            }
        }
    }

    private void Update()
    {
        if (Time.time - mLastAniDrawTime > fInternalTime)
        {
            mLastAniDrawTime = Time.time;
            DrawNext();
        }
    }

    private void DrawNext()
    {
        mSpriteRenderer.sprite = sprites[nAniIndex];
        nAniIndex++;
        if (nAniIndex >= nCount)
        {
            nAniIndex = nBeginFrameIndex;
        }
    }
}
