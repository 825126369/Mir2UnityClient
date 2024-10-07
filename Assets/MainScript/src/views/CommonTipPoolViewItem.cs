using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonTipPoolViewItem : MonoBehaviour
{
    public Text lb_Des = null;
    private CanvasGroup mUIOpacity;
    public int nMyId;
    public int nOtherId;
    public float fWaitTime;
    
    private void Awake()
    {
        this.mUIOpacity = this.gameObject.GetComponent<CanvasGroup>();
    }

    private void Update() 
    {
        float dt = Time.deltaTime;
        if (this.fWaitTime > 0)
        {
            this.fWaitTime -= dt;
            if(this.fWaitTime <= 0)
            {
                this.Hide();
            }
            else
            {
                if(UIMgr.Instance.CommonTipPoolView.nItemId != this.nOtherId)
                {
                    this.nOtherId = UIMgr.Instance.CommonTipPoolView.nItemId;
                    this.DoMoveUpAni();
                }
            }
        }
    }

    public void Show(string des, int nId)
    {
        this.nMyId = nId;
        this.nOtherId = nId;
        this.fWaitTime = 2.3f;

        this.gameObject.SetActive(true);
        this.lb_Des.text = des;
        LeanTween.alphaCanvas(this.mUIOpacity, 1.0f, 0.2f).setEase(LeanTweenType.easeOutSine);
        float fromY = -Screen.height / 4f;
        this.transform.localPosition = new Vector3(0, fromY, 0);
        this.DoMoveUpAni();
    }  

    public void DoMoveUpAni()
    {
        float toY = 0;
        toY = 120 * (UIMgr.Instance.CommonTipPoolView.nItemId - this.nMyId);
        LeanTween.moveLocalY(this.gameObject, toY, 0.3f).setEase(LeanTweenType.easeOutSine);
    }

    public void Hide()
    {   
        LeanTween.cancel(this.gameObject);
        LeanTween.alphaCanvas(this.mUIOpacity, 0f, 0.3f).setEase(LeanTweenType.easeOutSine);
        float toY = Screen.height / 4f;
        toY = Mathf.Max(toY, this.transform.localPosition.y);
        LeanTween.moveLocalY(this.gameObject, toY, 0.3f).setEase(LeanTweenType.easeOutSine).setOnComplete(()=>{
            UIMgr.Instance.CommonTipPoolView.RecycleItem(this);
        });
    }

}

