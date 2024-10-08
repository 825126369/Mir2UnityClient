using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class AnimationImage : MonoBehaviour
{
    public SpriteAtlas mSpriteAtlas = null;
    public string PrefixName = "";
    public int nBeginIndex = 940;
    public int nCount = 10;
    public float fInternalTime = 0.03f;

    private int nAniIndex = 0;
    private float mLastAniDrawTime;
    private Image mImage;

    private void Start()
    {
        mImage = GetComponent<Image>();
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
        mImage.sprite = mSpriteAtlas.GetSprite(PrefixName + nAniIndex);
        nAniIndex++;
        if (nAniIndex >= nCount)
        {
            nAniIndex = nBeginIndex;
        }
    }
}
