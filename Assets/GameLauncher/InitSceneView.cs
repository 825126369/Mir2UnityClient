using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class InitSceneView : MonoBehaviour
{
    public GameObject n_bg;
    public GameObject uiLoading;
    private Text mText;
    private Scrollbar imageProgressBar;

    private float fCurrentBeginProgress;
    private float fCurrentEndProgress;
    private float fNowMaxProgress;
    private bool bAni;
    private float fCdTime = 0.0f;
    private float fAniMaxTime = 0.0f;

    Action mFinishAction;
    private DownloadStatus mInfo;

    public void Awake()
    {
        //AdaptorCtrl.Instance.adjustUI(uiLoading);
        //AdaptorCtrl.Instance.AdjustBg(this.n_bg);
    }

    public void Init()
    {
        mText = transform.FindDeepChild("textProgress").GetComponent<Text>();
        imageProgressBar = transform.FindDeepChild("Scrollbar").GetComponent<Scrollbar>();
        gameObject.SetActive(false);
    }

    public void Show(Action mFinishAction)
    {
        gameObject.SetActive(true);

        fCdTime = 0.0f;
        bAni = false;
        fCurrentBeginProgress = 0.0f;
        fCurrentEndProgress = 0.0f;
        fNowMaxProgress = 0;

        this.mFinishAction = mFinishAction;
        UpdateProgress(0);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateProgress(float fPercent)
    {
        long nTotalBytes = mInfo.TotalBytes;
        long nDownloadBytes = mInfo.DownloadedBytes;
        
        if (nTotalBytes > 0)
        {
            mText.text = "LOADING " + Mathf.FloorToInt(fPercent * 100) + "%   " + nDownloadBytes + "/" + nTotalBytes;
        }
        else
        {
            mText.text = "LOADING " + Mathf.FloorToInt(fPercent * 100) + "%";
        }
        imageProgressBar.size = fPercent;

        if (fPercent >= 1.0f)
        {
            mFinishAction();
        }
    }

    //private void Update()
    //{
    //    if(this.mInfo.Percent >= 1.0f)
    //    {
    //        fAniMaxTime = 0.5f;
    //    }

    //    if (bAni)
    //    {
    //        fCdTime += Time.deltaTime;
    //        float fPerent = fCdTime / fAniMaxTime;
    //        fPerent = Mathf.Clamp01(fPerent);
    //        float fJinDu = fCurrentBeginProgress * (1 - fPerent) + fCurrentEndProgress * fPerent;
    //        UpdateProgress(fJinDu);
    //        if (fPerent >= 1.0f)
    //        {
    //            bAni = false;
    //            fCdTime = 0.0f;
    //        }
    //    }

    //    if (!bAni)
    //    {
    //        if (fCurrentEndProgress < fNowMaxProgress)
    //        {
    //            fCurrentBeginProgress = fCurrentEndProgress;
    //            fCurrentEndProgress = fNowMaxProgress;
    //            bAni = true;
    //            fCdTime = 0.0f;
    //            fAniMaxTime = (fCurrentEndProgress - fCurrentBeginProgress) * 2.0f;
    //            fAniMaxTime = Mathf.Clamp(fAniMaxTime, 0.5f, 2.0f);
    //        }
    //        else
    //        {
    //            if (fCurrentEndProgress >= 1.0f)
    //            {
    //                mFinishAction();
    //            }
    //        }
    //    }
    //}

    public void UpdateRealProgress(DownloadStatus mInfo)
    {
        this.fNowMaxProgress = mInfo.Percent;
        this.mInfo = mInfo;
        UpdateProgress(fNowMaxProgress);
    }

    public void UpdateRealProgress(float Percent)
    {
        this.fNowMaxProgress = Percent;
        UpdateProgress(fNowMaxProgress);
    }

}
