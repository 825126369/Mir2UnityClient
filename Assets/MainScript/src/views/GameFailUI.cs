using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFailUI : MonoBehaviour
{
    public Text textLevel;
    public Button mReStartBtn;


    private bool bInit = false;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (bInit) return;
        bInit = true;
        mReStartBtn.onClick.AddListener(() =>
        {
            this.Hide();
            //MainGame.readOnlyInstance.GameBegin();
        });
    }

    public void Show()
    {
        ViewAniTools.PlayShowScaleAni(gameObject, true);
        Init();
        textLevel.text = "Level" + DataCenter.readOnlyInstance.nLevel;
    }

    public void Hide()
    {
        ViewAniTools.PlayShowScaleAni(gameObject, false);
    }
    
}






















