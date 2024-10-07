using UnityEngine;

public class UIMgr : SingleTonMonoBehaviour<UIMgr>
{
    public CommonTipPoolView CommonTipPoolView;
    public CommonDialogView CommonDialogView;
    public CommonWindowLoading CommonWindowLoading;


    public GameBeginUI mGameBeginUI;
    public GameFailUI mGameFailUI;
    public GameFail_Resurrection mGameFail_Resurrection;
    public GameWinUI mGameWinUI;

    public void Init()
    {
        Init_Global_WindowLoading();
        Init_Global_CommonDialogView();
        Init_Global_CommonTipPoolView();
    }

    public void Init_Global_WindowLoading()
    {
        if (this.CommonWindowLoading == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("CommonWindowLoading");
            GameObject go = Instantiate<GameObject>(goPrefab);
            this.CommonWindowLoading = go.GetComponent<CommonWindowLoading>();
            this.CommonWindowLoading.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Tip, false);
            this.CommonWindowLoading.gameObject.SetActive(false);
        }
    }

    public void Init_Global_CommonDialogView()
    {
        if (this.CommonDialogView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("CommonDialogView");
            GameObject go = Instantiate<GameObject>(goPrefab);
            this.CommonDialogView = go.GetComponent<CommonDialogView>();
            this.CommonDialogView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Tip, false);
            this.CommonDialogView.gameObject.SetActive(false);
        }
    }

    public void Init_Global_CommonTipPoolView()
    {
        if (this.CommonTipPoolView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("CommonTipPoolView");
            GameObject go = Instantiate<GameObject>(goPrefab);
            this.CommonTipPoolView = go.GetComponent<CommonTipPoolView>();
            this.CommonTipPoolView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Tip, false);
            this.CommonTipPoolView.gameObject.SetActive(false);
        }
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
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("GameFail_Resurrection.prefab") as GameObject;
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






















