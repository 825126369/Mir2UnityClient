using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinUI : MonoBehaviour
{
    public Text textLevel;

    private bool bInit = false;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (bInit) return;
        bInit = true;
    }

    public void Show()
    {
        ViewAniTools.PlayShowScaleAni(gameObject, true);
        textLevel.text = "Level" + DataCenter.readOnlyInstance.nLevel;

        LeanTween.delayedCall(3.0f, () =>
        {
            this.Hide();
            MainGame.readOnlyInstance.GameBegin();
        });

    }

    public void Hide()
    {
        ViewAniTools.PlayShowScaleAni(gameObject, false);
    }
    
}






















