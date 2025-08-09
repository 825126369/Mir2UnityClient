using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot:MonoBehaviour
{
    public RectTransform mCanvas_Game;
    public RectTransform mCanvas_WinAnimation;
    public RectTransform mCanvas_Pop;
    public RectTransform mCanvas_Tip;
    public RectTransform mCanvas_Loading;
    public RectTransform bannerAdsOffsetNode;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
