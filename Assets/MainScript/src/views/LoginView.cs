using NetProtocols.Login;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour
{
    public Button LoginBtn;
    public Button RegisterBtn;
    public Button changePasswordBtn;
    public Button SaftBtn;

    public InputField AccountInputField;
    public InputField PasswordInputField;
    
    private bool bInit = false;
    void Init()
    {
        if (bInit) return;
        bInit = true;

        LoginBtn.onClick.AddListener(() =>
        {
            if (!string.IsNullOrWhiteSpace(AccountInputField.text) && !string.IsNullOrWhiteSpace(PasswordInputField.text))
            {
                packet_cs_Login mData = new packet_cs_Login();
                mData.Account = AccountInputField.text;
                mData.Password = PasswordInputField.text;
                NetClientMgr.Instance.LoginServer_NetClient.SendNetData(NetProtocolCommand.CS_REQUEST_LOGIN, mData);
            }
            else
            {
                UIMgr.Instance.CommonDialogView.ShowOk("提示", "账号密码输入不正确");
            }
        });

        RegisterBtn.onClick.AddListener(() =>
        {
            this.Hide();
            UIMgr.Instance.Show_RegisterView();
        });

        changePasswordBtn.onClick.AddListener(() =>
        {
            this.Hide();
            UIMgr.Instance.Show_ChangePasswordView();
        });

    }
    
    public void Show()
    {
        Init();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
