using UnityEngine;

public class UIMgr : SingleTonMonoBehaviour<UIMgr>
{
    public static CommonTipPoolView CommonTipPoolView;
    public static CommonDialogView CommonDialogView;
    public static CommonWindowLoading CommonWindowLoading;

    public LoginView LoginView;
    public RegisterView RegisterView;
    public SafeView SafeView;
    public ChangePasswordView ChangePasswordView;

    public SelectServerView SelectServerView;
    public SelectRoleView SelectRoleView;
    public CreateRoleView CreateRoleView;

    public MainUI MainUI;
    public void Init()
    {
        Init_Global_WindowLoading();
        Init_Global_CommonDialogView();
        Init_Global_CommonTipPoolView();
    }

    public static void Init_Global_WindowLoading()
    {
        if (CommonWindowLoading == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("CommonWindowLoading");
            GameObject go = Instantiate<GameObject>(goPrefab);
            CommonWindowLoading = go.GetComponent<CommonWindowLoading>();
            CommonWindowLoading.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Tip, false);
            CommonWindowLoading.gameObject.SetActive(false);
        }
    }

    public static void Init_Global_CommonDialogView()
    {
        if (CommonDialogView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("CommonDialogView");
            GameObject go = Instantiate<GameObject>(goPrefab);
            CommonDialogView = go.GetComponent<CommonDialogView>();
            CommonDialogView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Tip, false);
            CommonDialogView.gameObject.SetActive(false);
        }
    }

    public void Init_Global_CommonTipPoolView()
    {
        if (CommonTipPoolView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("CommonTipPoolView");
            GameObject go = Instantiate<GameObject>(goPrefab);
            CommonTipPoolView = go.GetComponent<CommonTipPoolView>();
            CommonTipPoolView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Tip, false);
            CommonTipPoolView.gameObject.SetActive(false);
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

    public void Show_CreateRoleView()
    {
        if (CreateRoleView == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("CreateRoleView");
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            CreateRoleView = goView.GetComponent<CreateRoleView>();
            CreateRoleView.Show();
        }
        else
        {
            CreateRoleView.Show();
        }
    }

    public void Show_MainUI()
    {
        if (MainUI == null)
        {
            GameObject goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("MainUI");
            GameObject goView = Instantiate<GameObject>(goPrefab);
            goView.transform.SetParent(GameLauncher.Instance.mUIRoot.mCanvas_Pop, false);
            MainUI = goView.GetComponent<MainUI>();
            MainUI.Show();
        }
        else
        {
            MainUI.Show();
        }
    }

}






















