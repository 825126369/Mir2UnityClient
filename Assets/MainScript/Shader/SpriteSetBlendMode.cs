using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class SpriteSetBlendMode : MonoBehaviour
{
    private SpriteRenderer mSpriteRenderer;
    private static readonly Dictionary<int, Material> mMatDic = new Dictionary<int, Material>();
    private bool bInit = false;

    public UnityEngine.Rendering.BlendOp mBlendOp = UnityEngine.Rendering.BlendOp.Add;
    public UnityEngine.Rendering.BlendMode srcBlend = UnityEngine.Rendering.BlendMode.One;
    public UnityEngine.Rendering.BlendMode dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;

    private void Start()
    {
        SetBlendMode();
    }

    private void Init()
    {
        if (bInit) return;
        bInit = true;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    static Material GetDefaultMaterial(UnityEngine.Rendering.BlendOp mBlendOp, UnityEngine.Rendering.BlendMode srcBlend, UnityEngine.Rendering.BlendMode dstBlend)
    {
        int Id = (int)mBlendOp << 16 | (int)srcBlend << 8 | (int)dstBlend;
        Material mMat = null;
        if (!mMatDic.TryGetValue(Id, out mMat))
        {
            mMat = new Material(ShaderAutoFind.Find($"Customer/{typeof(SpriteSetBlendMode).Name}"));
            mMat.name = $"BlendMode: {mBlendOp}-{srcBlend}-{dstBlend}";
            mMat.SetInt("_BlendOp", (int)mBlendOp);
            mMat.SetInt("_SrcBlend", (int)srcBlend);
            mMat.SetInt("_DstBlend", (int)dstBlend);
           // GameObject.DontDestroyOnLoad(mMat);
            mMatDic[Id] = mMat;
        }
        return mMat;
    }

    public void SetBlendMode()
    {
        Init();
        SetBlendMode(mBlendOp, srcBlend, dstBlend);
    }

    public void SetBlendMode(UnityEngine.Rendering.BlendOp mBlendOp, UnityEngine.Rendering.BlendMode _SrcBlend, UnityEngine.Rendering.BlendMode _DstBlend)
    {
        Init();
        this.mBlendOp = mBlendOp;
        this.srcBlend = _SrcBlend;
        this.dstBlend = _DstBlend;
        Material mMat = GetDefaultMaterial(mBlendOp, _SrcBlend, _DstBlend);
        mSpriteRenderer.sharedMaterial = mMat;
    }

    public void SetBlendMode(Mir2.BlendMode mBlendMode)
    {
        Init();
        if (mBlendMode == Mir2.BlendMode.INVLIGHT)
        {
            SetBlendMode(UnityEngine.Rendering.BlendOp.Add, UnityEngine.Rendering.BlendMode.One, UnityEngine.Rendering.BlendMode.OneMinusSrcColor);
        }
        else
        {
            SetBlendMode(UnityEngine.Rendering.BlendOp.Add, UnityEngine.Rendering.BlendMode.SrcAlpha, UnityEngine.Rendering.BlendMode.One);
        }
    }

}
