using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBeginUI : MonoBehaviour
{
    public Text textLevel;

    public void Init()
    {
        
    }
    
    public void Show()
    {
        ViewAniTools.PlayShowScaleAni(gameObject, true);
        textLevel.text = "Level" + DataCenter.readOnlyInstance.nLevel;

        LeanTween.delayedCall(3.0f, () =>
        {
            this.Hide();
            MainGame.readOnlyInstance.PlayGameBeginMoveAni();
        });
    }

    public void Hide()
    {
        ViewAniTools.PlayShowScaleAni(gameObject, false);
    }
    
}






















