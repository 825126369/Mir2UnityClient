using NetProtocols.Login;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RegisterView : MonoBehaviour
{
    public Button OkBtn;
    public Button CancelBtn;

    public InputField AccountInputField;
    public InputField PasswordInputField;
    public InputField ConfirmPasswordInputField;
    public InputField SecretQuestionInputField;
    public InputField AnswerInputField;
    public InputField EmailInputField;

    private bool bInit = false;
    void Init()
    {
        if(bInit) return;
        bInit = true;

        OkBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrWhiteSpace(AccountInputField.text) || string.IsNullOrWhiteSpace(PasswordInputField.text))
            {
                UIMgr.Instance.CommonDialogView.ShowOk("��ʾ", "�˺��������벻��ȷ");
                return;
            }
            else if (PasswordInputField.text != ConfirmPasswordInputField.text)
            {
                UIMgr.Instance.CommonDialogView.ShowOk("��ʾ", "������������벻һ��");
                return;
            }
            else if (!DataCenter.Instance.mAccountRegex.IsMatch(AccountInputField.text))
            {
                UIMgr.Instance.CommonDialogView.ShowOk("��ʾ", "������������벻һ��");
                return;
            }
            else if (!DataCenter.Instance.mPasswordRegex.IsMatch(PasswordInputField.text))
            {
                UIMgr.Instance.CommonDialogView.ShowOk("��ʾ", "������������벻һ��");
                return;
            }
            if (string.IsNullOrWhiteSpace(SecretQuestionInputField.text) || string.IsNullOrWhiteSpace(AnswerInputField.text))
            {
                UIMgr.Instance.CommonDialogView.ShowOk("��ʾ", "��ȫ����ش���ȷ");
                return;
            }

            var mData = new packet_cs_Register();
            mData.Account = AccountInputField.text;
            mData.Password = PasswordInputField.text;
            mData.SecretQuestion = SecretQuestionInputField.text;
            mData.SecretAnswer = AnswerInputField.text;
            mData.EMailAddress = EmailInputField.text;
            NetClientMgr.Instance.LoginServer_NetClient.SendNetData(NetProtocolCommand.CS_REQUEST_REGISTER, mData);
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
