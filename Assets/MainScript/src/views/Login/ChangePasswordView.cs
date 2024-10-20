using NetProtocols.Login;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ChangePasswordView : MonoBehaviour
{
    public Button OkBtn;
    public Button CancelBtn;

    public InputField AccountInputField;
    public InputField CurrentPasswordInputField;
    public InputField NewPasswordInputField;
    public InputField ConfirmPasswordInputField;

    private bool bInit = false;
    private void Init()
    {
        if (bInit) return;
        bInit = true;

        OkBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrWhiteSpace(AccountInputField.text) ||
                string.IsNullOrWhiteSpace(CurrentPasswordInputField.text) ||
                string.IsNullOrWhiteSpace(NewPasswordInputField.text) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordInputField.text))
            {
                UIMgr.Instance.CommonDialogView.ShowOk("提示", "账号密码不能为空");
                return;
            }
            else if (NewPasswordInputField.text != ConfirmPasswordInputField.text)
            {
                UIMgr.Instance.CommonDialogView.ShowOk("提示", "输入的两次密码不一样");
                return;
            }
            else if (!DataCenter.Instance.mAccountRegex.IsMatch(AccountInputField.text))
            {
                UIMgr.Instance.CommonDialogView.ShowOk("提示", "账户名必须是字母和数字的组合，且长度在8~20个字符之间");
                return;
            }
            else if (!DataCenter.Instance.mPasswordRegex.IsMatch(NewPasswordInputField.text))
            {
                UIMgr.Instance.CommonDialogView.ShowOk("提示", "密码必须是字母和数字的组合，且长度在8~20个字符之间");
                return;
            }

            var mData = new packet_cs_ChangePassword();
            mData.Account = AccountInputField.text;
            mData.CurrentPassword = CurrentPasswordInputField.text;
            mData.NewPassword = NewPasswordInputField.text;
            NetClientLoginMgr.Instance.LoginServer_NetClient.SendNetData(NetProtocolCommand.CS_REQUEST_CHANGE_PASSWORD, mData);
        });

        CancelBtn.onClick.AddListener(() =>
        {
            this.Hide();
            UIMgr.Instance.Show_LoginView();
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
        UIMgr.Instance.Show_LoginView();
    }
}
