using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class AnimationImage : MonoBehaviour
{
    public SpriteAtlas mSpriteAtlas = null;
    public string PrefixName = string.Empty;
    public int nBeginIndex = 940;
    public int nCount = 10;
    public float fInternalTime = 0.03f;

    private int nEndIndex = 0;
    private int nAniIndex = 0;
    private float mLastAniDrawTime;
    private Image mImage;

    private void Start()
    {
        InitAni();
    }

    public void SetAniParam(string atlasName, int nBeginIndex, int nCount, float fInternalTime)
    {
        this.mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas(atlasName);
        if(this.mSpriteAtlas == null)
        {
            this.mSpriteAtlas = AssetsLoader.Instance.GetAsset($"{atlasName}{GameConst.spriteAtlasExtention}") as SpriteAtlas;
        }

        this.nBeginIndex = nBeginIndex;
        this.nCount = nCount;
        this.fInternalTime = fInternalTime;
        InitAni();
    }

    public void SetAniParam(SpriteAtlas mSpriteAtlas, int nBeginIndex, int nCount, float fInternalTime)
    {
        this.mSpriteAtlas = mSpriteAtlas;
        this.nBeginIndex = nBeginIndex;
        this.nCount = nCount;
        this.fInternalTime = fInternalTime;

        InitAni();
    }

    private void InitAni()
    {
        mImage = GetComponent<Image>();
        nEndIndex = nBeginIndex + nCount - 1;
        nAniIndex = nBeginIndex;
        mLastAniDrawTime = Time.time;
        DrawNext();
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
        mImage.SetNativeSize();
        nAniIndex++;
        if (nAniIndex > nEndIndex)
        {
            nAniIndex = nBeginIndex;
        }
    }
}
