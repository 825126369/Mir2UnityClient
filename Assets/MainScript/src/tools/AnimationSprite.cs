using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AnimationSprite : MonoBehaviour
{
    public SpriteAtlas mSpriteAtlas = null;
    public string PrefixName = "";
    public int nBeginIndex = 940;
    public int nCount = 10;
    public float fInternalTime = 0.03f;

    private int nAniIndex = 0;
    private float mLastAniDrawTime;
    private SpriteRenderer mSpriteRenderer;

    private void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        nAniIndex = 0;
        DrawNext();
        mLastAniDrawTime = Time.time;
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
        mSpriteRenderer.sprite = mSpriteAtlas.GetSprite(PrefixName + nAniIndex);
        nAniIndex++;
        if (nAniIndex >= nCount)
        {
            nAniIndex = nBeginIndex;
        }
    }
}
