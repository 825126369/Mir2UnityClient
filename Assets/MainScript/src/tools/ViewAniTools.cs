using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ViewAniTools
{
    public static void PlayShowRightToLeftAni(GameObject viewNode, bool bShow, Action finishFunc = null)
    {
        float width = Screen.width;
        GameObject mAniObj = viewNode.transform.FindDeepChild("n_root").gameObject;

        if (bShow)
        {
            viewNode.SetActive(true);
            mAniObj.transform.localPosition = new Vector3(width, 0, 0);
            LeanTween.moveLocalX(mAniObj, 0f, 0.45f).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
            {
                finishFunc?.Invoke();
            });
        }
        else
        {
            mAniObj.transform.localPosition = new Vector3(0, 0, 0);
            LeanTween.moveLocalX(mAniObj, width+200, 0.45f).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
            {
                viewNode.SetActive(false);
                finishFunc?.Invoke();
            });
        }
    }

    public static void PlayShowScaleAni(GameObject viewNode, bool bShow, Action finishFunc = null)
    {
        GameObject mAniObj = viewNode.transform.FindDeepChild("n_root").gameObject;

        if (bShow)
        {
            viewNode.SetActive(true);
            mAniObj.transform.localScale = Vector3.zero;
            LeanTween.scale(mAniObj, Vector3.one, 0.45f).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
            {
                finishFunc?.Invoke();
            });
        }
        else
        {
            LeanTween.scale(mAniObj, Vector3.zero, 0.45f).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
            {
                viewNode.SetActive(false);
                finishFunc?.Invoke();
            });
        }
    }

    public static void PlayShowAlphaAni(GameObject viewNode, bool bShow, Action finishFunc = null)
    {
        GameObject mAniObj = viewNode.transform.FindDeepChild("n_root").gameObject;
        var mUIOpacity = mAniObj.GetComponent<CanvasGroup>();
        if (bShow)
        {
            viewNode.SetActive(true);
            mUIOpacity.alpha = 0f;
            LeanTween.alphaCanvas(mUIOpacity, 1.0f, 0.25f).setOnComplete(() =>
            {
                finishFunc?.Invoke();
            });
        }
        else
        {
            mUIOpacity.alpha = 1f;
            LeanTween.alphaCanvas(mUIOpacity, 0f, 0.25f).setOnComplete(() =>
            {
                viewNode.SetActive(false);
                finishFunc?.Invoke();
            });
        }
    }

    public static void PlayShowDownToUpAni(GameObject viewNode, bool bShow, Action finishFunc = null)
    {
        float height = Screen.height + 300;
        GameObject mAniObj = viewNode.transform.FindDeepChild("n_root").gameObject;
        CanvasGroup mCanvasGroup = mAniObj.AddMissComponent<CanvasGroup>();
        if (bShow)
        {
            viewNode.SetActive(true);
            mAniObj.transform.localPosition = new Vector3(0, -height, 0);
            mCanvasGroup.alpha = 0;

            var mSeq = LeanTween.sequence();
            mSeq.append(LeanTween.moveLocalY(mAniObj, 0f, 0.45f).setEase(LeanTweenType.easeOutSine));
            mSeq.append(() =>
            {
                finishFunc?.Invoke();
            });
            LeanTween.alphaCanvas(mAniObj.GetComponent<CanvasGroup>(), 1, 0.3f).setEase(LeanTweenType.easeInSine);
        }
        else
        {
            viewNode.SetActive(true);
            mAniObj.transform.localPosition = Vector3.zero;
            LeanTween.moveLocalY(mAniObj, -height, 0.65f).setEase(LeanTweenType.easeInSine).setOnComplete(() =>
            {
                viewNode.SetActive(false);
                finishFunc?.Invoke();
            });

            LeanTween.alphaCanvas(mAniObj.GetComponent<CanvasGroup>(), 0, 0.5f).setEase(LeanTweenType.easeInSine);
        }
    }
}


