using NetProtocols.Login;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour
{
    public Button LoginBtn;
    public Button RegisterBtn;
    public InputField AccountInputField;
    public InputField PasswordInputField;

    void Start()
    {
        LoginBtn.onClick.AddListener(() =>
        {
            if (!string.IsNullOrWhiteSpace(AccountInputField.text) && !string.IsNullOrWhiteSpace(PasswordInputField.text))
            {
                packet_cs_Login mData = new packet_cs_Login();
                mData.Account = AccountInputField.text;
                mData.Password = PasswordInputField.text;
                NetClientMgr.Instance.LoginServer_NetClient.SendNetData(LoginServer_NetCommand.CS_REQUEST_LOGIN, mData);
            }
            else
            {
                UIMgr.Instance.CommonDialogView.ShowOk("提示", "账号密码输入不正确");
            }
        });
    }
    
    void Update()
    {
        
    }
}
