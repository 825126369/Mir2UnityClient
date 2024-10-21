using UnityEngine;
using UnityEngine.UI;

public class SelectServerItem : MonoBehaviour
{
    public Button mClickBtn;
    public Text textServerName;
    public Image imageState;

    private ServerItemData mData;
    private bool bInit = false;
    private void Init()
    {
        if (bInit) return;
        bInit = true;
        mClickBtn.onClick.AddListener(() =>
        {
            PrintTool.Log(mData);
            DataCenter.Instance.currentSelectServerItemData = mData;
            NetClientGameMgr.Instance.Init();
        });
    }

    public void Refresh(ServerItemData mData)
    {
        Init();

        this.mData = mData;
        textServerName.text = mData.ServerName;

        if (mData.nState == EServerState.Normal)
        {
            imageState.color = Color.green;
        }
        else if (mData.nState == EServerState.jam)
        {
            imageState.color = Color.red;
        }
        else
        {
            imageState.color = Color.gray;
        }
    }
}
