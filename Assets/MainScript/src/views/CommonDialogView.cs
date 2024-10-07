using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonDialogView :MonoBehaviour 
{    
    public Text lbTtile = null;
    public Text lbMessage = null;
    public Button yesBtn = null;
    public Button cancelBtn = null;
    public Button okBtn = null;
    
    Action mYesFunc;
    Action mCancelFunc;
    Action mOkFunc; 
    
    public void ShowYesCancel(string title, string message, Action yesFunc = null, Action cancelFunc = null)
    {
        AudioController.Instance.playSound(Sounds.popup,1);
        ViewAniTools.PlayShowScaleAni(this.transform.gameObject, true);
        this.mYesFunc = yesFunc;
        this.mCancelFunc = cancelFunc;
        this.lbTtile.text = title;
        this.lbMessage.text = message;

        this.yesBtn.gameObject.SetActive(true);
        this.cancelBtn.gameObject.SetActive(true);
        this.okBtn.gameObject.SetActive(false);
        this.transform.SetSiblingIndex(this.transform.childCount - 1);
    }
    
    public void ShowOk(string title, string message, Action okFunc = null)
    {
        AudioController.Instance.PlayAudio(Sounds.popup, 1);
        ViewAniTools.PlayShowScaleAni(this.gameObject, true);
        this.lbTtile.text = title;
        this.lbMessage.text = message;
        this.mOkFunc = okFunc;

        this.yesBtn.gameObject.SetActive(false);
        this.cancelBtn.gameObject.SetActive(false);
        this.okBtn.gameObject.SetActive(true);
        this.transform.SetSiblingIndex(this.transform.childCount - 1);
    }

    public void Hide()
    {
        ViewAniTools.PlayShowScaleAni(this.gameObject, false);
    }

    public void OnClickYes()
    {
        AudioController.Instance.PlayAudio(Sounds.button, 1);
        this.Hide();
        this.mYesFunc?.Invoke();
    }

    public void OnClickCancel()
    {
        AudioController.Instance.PlayAudio(Sounds.button,1);
        this.Hide();
        this.mCancelFunc?.Invoke();
    }

    public void OnClickOk()
    {
        AudioController.Instance.PlayAudio(Sounds.button,1);
        this.Hide();
        this.mOkFunc?.Invoke();
    }
}


