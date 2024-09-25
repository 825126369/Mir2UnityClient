using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UIButtonLimitedType
{
    enabled = 0,
    raycastTarget = 1,
    interactable = 2,
}

public class UIButtonClickLimited : MonoBehaviour, IPointerClickHandler
{
    public UIButtonLimitedType m_LimitedType;
    public float m_Time = 1.0f;
    private bool bCanClick = true;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!this.bCanClick) return;
        this.OnClick();
    }

    private void OnClick()
    {
        Button mButton = gameObject.GetComponent<Button>();
        if(mButton != null)
        {
            if(m_LimitedType == UIButtonLimitedType.enabled)
            {
                mButton.enabled = false;
                this.bCanClick = false;
                LeanTween.delayedCall(gameObject, m_Time, ()=>
                {
                    mButton.enabled = true;
                    this.bCanClick = true;
                });
            }
            else if(m_LimitedType == UIButtonLimitedType.raycastTarget)
            {
                mButton.image.raycastTarget = false;
                this.bCanClick = false;
                LeanTween.delayedCall(gameObject, m_Time, ()=>
                {
                    mButton.image.raycastTarget = true;
                    this.bCanClick = true;
                });
            }
            else if(m_LimitedType == UIButtonLimitedType.interactable)
            {
                mButton.interactable = false;
                this.bCanClick = false;
                LeanTween.delayedCall(gameObject, m_Time, ()=>
                {
                    mButton.interactable = true;
                    this.bCanClick = true;
                });
            }
        }
        else
        {
            Image mImage = gameObject.GetComponent<Image>();
            if(mImage != null)
            {
                if(m_LimitedType == UIButtonLimitedType.enabled)
                {
                    mImage.enabled = false;
                    this.bCanClick = false;
                    LeanTween.delayedCall(gameObject, m_Time, ()=>
                    {
                        mImage.enabled = true;
                        this.bCanClick = true;
                    });
                }
                else if(m_LimitedType == UIButtonLimitedType.raycastTarget)
                {
                    mImage.raycastTarget = false;
                    this.bCanClick = false;
                    LeanTween.delayedCall(gameObject, m_Time, ()=>
                    {
                        mImage.raycastTarget = true;
                        this.bCanClick = true;
                    });
                }
            }
        }
        
    }
}
