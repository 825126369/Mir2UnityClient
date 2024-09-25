using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : SingleTonMonoBehaviour<UIMgr>
{
    public GameBeginUI mGameBeginUI;
    public GameFailUI mGameFailUI;
    public GameFail_Resurrection mGameFail_Resurrection;
    public GameWinUI mGameWinUI;

    public void Init()
    {

    }

    public void Show_GameBeginView()
    {
        if (mGameBeginUI == null)
        {
            GameObject goPrefab = AssetsLoader.Instance.GetAsset("Assets/GameAssets/game/prefabs/GameBegin.prefab") as GameObject;
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            mGameBeginUI = goView.GetComponent<GameBeginUI>();
            mGameBeginUI.Show();
        }
        else
        {
            mGameBeginUI.Show();
        }
    }

    public void Show_GameFail_Resurrection()
    {
        if (mGameFail_Resurrection == null)
        {
            GameObject goPrefab = AssetsLoader.Instance.GetAsset("Assets/GameAssets/game/prefabs/GameFail_Resurrection.prefab") as GameObject;
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            mGameFail_Resurrection = goView.GetComponent<GameFail_Resurrection>();
            mGameFail_Resurrection.Show();
        }
        else
        {
            mGameFail_Resurrection.Show();
        }
    }

    public void Show_GameFailUI()
    {
        if (mGameFailUI == null)
        {
            GameObject goPrefab = AssetsLoader.Instance.GetAsset("Assets/GameAssets/game/prefabs/GameFailEnd.prefab") as GameObject;
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            mGameFailUI = goView.GetComponent<GameFailUI>();
            mGameFailUI.Show();
        }
        else
        {
            mGameFailUI.Show();
        }
    }

    public void Show_GameWinUI()
    {
        if (mGameWinUI == null)
        {
            GameObject goPrefab = AssetsLoader.Instance.GetAsset("Assets/GameAssets/game/prefabs/GameWinEnd.prefab") as GameObject;
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            mGameWinUI = goView.GetComponent<GameWinUI>();
            mGameWinUI.Show();
        }
        else
        {
            mGameWinUI.Show();
        }
    }

}






















