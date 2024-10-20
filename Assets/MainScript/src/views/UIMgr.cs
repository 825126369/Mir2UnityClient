using UnityEngine;

public class UIMgr : SingleTonMonoBehaviour<UIMgr>
{
    public CommonTipPoolView CommonTipPoolView;
    public CommonDialogView CommonDialogView;
    public CommonWindowLoading CommonWindowLoading;

    public LoginView LoginView;
    public RegisterView RegisterView;
    public SafeView SafeView;
    public ChangePasswordView ChangePasswordView;

    public SelectServerView SelectServerView;

    public SelectRoleView SelectRoleView;
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

    public void Show_LoginView()
    {
        if (LoginView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("LoginView");
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            LoginView = goView.GetComponent<LoginView>();
            LoginView.Show();
        }
        else
        {
            LoginView.Show();
        }
    }

    public void Show_RegisterView()
    {
        if (RegisterView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("RegisterView");
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            RegisterView = goView.GetComponent<RegisterView>();
            RegisterView.Show();
        }
        else
        {
            RegisterView.Show();
        }
    }

    public void Show_ChangePasswordView()
    {
        if (ChangePasswordView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("ChangePasswordView");
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            ChangePasswordView = goView.GetComponent<ChangePasswordView>();
            ChangePasswordView.Show();
        }
        else
        {
            ChangePasswordView.Show();
        }
    }

    public void Show_SafeView()
    {
        if (SafeView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("SafeView");
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            SafeView = goView.GetComponent<SafeView>();
            SafeView.Show();
        }
        else
        {
            SafeView.Show();
        }
    }

    public void Show_SelectServerView()
    {
        if (SelectServerView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("SelectServerView");
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            SelectServerView = goView.GetComponent<SelectServerView>();
            SelectServerView.Show();
        }
        else
        {
            SelectServerView.Show();
        }
    }

    public void Show_SelectRoleView()
    {
        if (SelectRoleView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("SelectRoleView");
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            SelectRoleView = goView.GetComponent<SelectRoleView>();
            SelectRoleView.Show();
        }
        else
        {
            SelectRoleView.Show();
        }
    }

}






















