using UnityEngine;
using UnityEngine.UI;

public class SymbolItem : MonoBehaviour
{
    public SymbolItemData mData;
    public Image mIcon;
    public Button mClickBtn;
    public KuangItem mInWhichKuang;

    public void Init(KuangItem mKuangItem, SymbolItemData mData)
    {
        this.mInWhichKuang = mKuangItem;
        this.mData = mData;
        mIcon.sprite = ResCenter.readOnlyInstance.mBundleGameAllRes.FindSprite($"theme{DataCenter.readOnlyInstance.nThemeIndex}_{mData.nIconIndex}");

        mClickBtn.onClick.RemoveAllListeners();
        mClickBtn.onClick.AddListener(() =>
        {
            MainGame.readOnlyInstance.DoClickSymbol(this);
        });
    }
}
