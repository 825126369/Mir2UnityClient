using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum CoinStyle
{
    X0 = 1,
    X1 = 2,
    X2 = 2,
    X3 = 3,
    X4 = 4,
    X5 = 5,
}

public static class CoinStyleValue
{
    public const float X0 = 0.5f;
    public const float X1 = 1f;
    public const float X2 = 2;
    public const float X3 = 3;
    public const float X4 = 4;
    public const float X5 = 5;
}

public enum FlyType
{
   Coin = 1,
   Dianmond = 2,
   BP = 3    
}

public class FlyingCoinsView : MonoBehaviour 
{
    public GameObject coinpre = null;
    FlyType flyType =  FlyType.Coin;
    int coinscount = 20;
    public CoinStyle coinStyle = CoinStyle.X4;

    /**
    * 随机范围(random1~random2之间)
    */
    //  X
    private float random1_x = -200;
    private float random2_x = 200;
    private float random1_y = -30;
    private float random2_y = 30;
    private float createTime = 0.15f;
    private float standingTime = 0.2f;
    private float coinSpeed = 1200f;

    private void Awake()
    {
        this.coinpre.SetActive(false);
    }

    public void onPlayCoinAni(GameObject showNode,Vector3 fromPtWp, Vector3 toPtWP, int coinscount, CoinStyle coinStyle,bool hide, Action callback)
    {

   var UIT = showNode.GetComponent<RectTransform>();
   var fromPt = GameTools.WorldToUILocalPos(fromPtWp, UIT);
   var toPt = GameTools.WorldToUILocalPos(toPtWP, UIT);

   this.coinStyle = coinStyle;

    float coinStyleValue = 0;
    if(coinStyle == CoinStyle.X0)
    {
        coinStyleValue = CoinStyleValue.X0;
    }
    else if(coinStyle == CoinStyle.X1)
    {
        coinStyleValue = CoinStyleValue.X1;
    }
    else if(coinStyle == CoinStyle.X2)
    {
        coinStyleValue = CoinStyleValue.X2;
    }
    else if(coinStyle == CoinStyle.X3)
    {
        coinStyleValue = CoinStyleValue.X3;
    }
    else if(coinStyle == CoinStyle.X4)
    {
        coinStyleValue = CoinStyleValue.X4;
    }
    else if(coinStyle == CoinStyle.X5)
    {
        coinStyleValue = CoinStyleValue.X5;
    }else
    {
        PrintTool.Assert(false, coinStyle);
    }

   this.random1_x = -1*coinStyleValue*50;
   this.random2_x = coinStyleValue*50;

   this.coinscount = coinscount;
   AudioController.Instance.playSound(Sounds.coin_get, 1);

   Vector3 tempPlayer = fromPt;
    for (int i = 0; i < this.coinscount; i++)
    {
        GameObject pre = Instantiate(this.coinpre);
        pre.SetActive(true);
        pre.transform.SetParent(showNode.transform, false);       
        float rannumx = Mathf.Floor(UnityEngine.Random.Range(0, 1f) * (this.random2_x - this.random1_x + 1) + this.random1_x);
        float rannumy = Mathf.Floor(UnityEngine.Random.Range(0, 1f) * (this.random2_y - this.random1_y + 1) + this.random1_y);

        pre.transform.localPosition = tempPlayer;
        Vector3 tmpPt = new Vector3(tempPlayer.x + rannumx, tempPlayer.y + rannumy, 0); 
        LeanTween.moveLocal(pre, tmpPt, this.createTime);

        var mTimer = Timer.New(() => 
        {
           LeanTween.cancel(showNode);

           Vector3 pos = pre.transform.localPosition;
           Vector3 coinpos = toPt;

           Vector3 distVec = new Vector3(pos.x-coinpos.x,pos.y-coinpos.y, 0);                   
           float dist = Vector3.Magnitude(distVec);
           float playTime = dist / this.coinSpeed;
             
            Vector3 newPt = new Vector3(coinpos.x, coinpos.y, 0);
            LeanTween.moveLocal(pre, newPt, playTime).setOnComplete(()=>
            {
                if(hide)
                {
                   Destroy(pre); 
               }
               callback?.Invoke();
            });
       }, this.standingTime + this.createTime, 1);
       mTimer.Start();
    }
}
}