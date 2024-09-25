using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFail_Resurrection : MonoBehaviour
{
    public Text textCoin;

    public Button mAdsBtn;
    public Button mCoinBtn;
    public Button mCloseBtn;
    public Button mNoThanksBtn;

    private bool bInit = false;

    private void Awake()
    {
        Init();
    }
        
    public void Init()
    {
        if (bInit) return;
        bInit = true;
        mCoinBtn.onClick.AddListener(() =>
        {
            this.Hide();
        });

        mAdsBtn.onClick.AddListener(() =>
        {
            this.Hide();
        });

        mCloseBtn.onClick.AddListener(() =>
        {
            this.Hide();
            UIMgr.readOnlyInstance.Show_GameFailUI();
        });

        mNoThanksBtn.onClick.AddListener(() =>
        {
            this.Hide();
            UIMgr.readOnlyInstance.Show_GameFailUI();
        });
    }
    
    public void Show()
    {
        ViewAniTools.PlayShowScaleAni(gameObject, true);
        Init();
        textCoin.text = "100";
    }

    public void Hide()
    {
        ViewAniTools.PlayShowScaleAni(gameObject, false);
    }
    
}






















