using UnityEngine;
using UnityEngine.UI;

public class SelectServerItem : MonoBehaviour
{
    public Button mClickBtn;
    public Text textServerName;
    public GameObject goState;

    private ServerItemData mData;
    private bool bInit =false;
    private void Init()
    {
        if (bInit) return;
        bInit = true;
        mClickBtn.onClick.AddListener(() =>
        {
            DataCenter.Instance.currentSelectServerItemData = mData;
            NetClientGameMgr.Instance.Init();
        });
    }

    public void Refresh(ServerItemData mData)
    {
        this.mData = mData;
        textServerName.text = mData.ServerName;
    }

}
