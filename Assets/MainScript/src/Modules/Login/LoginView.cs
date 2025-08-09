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
            if (string.IsNullOrWhiteSpace(AccountInputField.text))
            {
                UIMgr.CommonDialogView.ShowOk("提示", "账号不能为空");
                return;
            }
            else if (string.IsNullOrWhiteSpace(PasswordInputField.text))
            {
                UIMgr.CommonDialogView.ShowOk("提示", "密码不能为空");
                return;
            }
            
            UIMgr.CommonWindowLoading.Show();
            packet_cs_Login mData = new packet_cs_Login();
            mData.Account = AccountInputField.text;
            mData.Password = PasswordInputField.text;
            NetClientLoginMgr.SendNetData(NetProtocolCommand.CS_REQUEST_LOGIN, mData);
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
