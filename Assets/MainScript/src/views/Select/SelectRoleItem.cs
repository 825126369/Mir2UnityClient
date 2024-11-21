using Mir2;
using NetProtocols.Game;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoleItem : MonoBehaviour
{
    public GameObject goHaveRole;
    public Button mClickBtn;

    public Text textName;
    public Text textLevel;
    public Text textClass;

    public GameObject goZhanShi;
    public GameObject goZhanShiSelect;

    public GameObject goFaShi;
    public GameObject goFaShiSelect;

    public GameObject goDaoShi;
    public GameObject goDaoShiSelect;

    public GameObject goCiKe;
    public GameObject goCiKeSelect;

    public GameObject goSheShou;
    public GameObject goSheShouSelect;

    private packet_data_SelectRole_RoleInfo mData;
    private SelectRoleView mManager;
    private bool bInit = false;
    public void Init(SelectRoleView mManager)
    {
        if (bInit) return;
        bInit = true;

        this.mManager = mManager;
        mClickBtn.onClick.AddListener(() =>
        {
            mManager.OnSelectRoldId(mData.NRoleId);
        });
    }

    public void OnSelect(uint nRoleId)
    {
        ShowTip(nRoleId == mData.NRoleId);
    }

    private void ShowTip(bool Show)
    {
        goZhanShiSelect.SetActive(false);
        goFaShiSelect.SetActive(false);
        goDaoShiSelect.SetActive(false);
        goCiKeSelect.SetActive(false);
        goSheShouSelect.SetActive(false);

        if (mData.Class == (uint)MirClass.Warrior)
        {
            goZhanShiSelect.SetActive(Show);
        }
        else if (mData.Class == (uint)MirClass.Wizard)
        {
            goFaShiSelect.SetActive(Show);
        }
        else if (mData.Class == (uint)MirClass.Taoist)
        {
            goDaoShiSelect.SetActive(Show);
        }
        else if (mData.Class == (uint)MirClass.Assassin)
        {
            goCiKeSelect.SetActive(Show);
        }
        else if (mData.Class == (uint)MirClass.Archer)
        {
            goSheShouSelect.SetActive(Show);
        }
        else
        {
            PrintTool.Assert(false);
        }
    }

    public void Refresh(packet_data_SelectRole_RoleInfo mData)
    {
        this.mData = mData;
        if (this.mData != null)
        {
            PrintTool.Log(gameObject.name + " | " + mData.NRoleId);
            goHaveRole.SetActive(true);
            textName.text = mData.Name;
            textLevel.text = mData.Level.ToString();

            goZhanShi.SetActive(false);
            goFaShi.SetActive(false);
            goDaoShi.SetActive(false);
            goCiKe.SetActive(false);
            goSheShou.SetActive(false);
            if (mData.Class == (uint)MirClass.Warrior)
            {
                goZhanShi.SetActive(true);
                textClass.text = "战士";
            }
            else if (mData.Class == (uint)MirClass.Wizard)
            {
                goFaShi.SetActive(true);
                textClass.text = "法师";
            }
            else if (mData.Class == (uint)MirClass.Taoist)
            {
                goDaoShi.SetActive(true);
                textClass.text = "道士";
            }
            else if (mData.Class == (uint)MirClass.Assassin)
            {
                goCiKe.SetActive(true);
                textClass.text = "刺客";
            }
            else if (mData.Class == (uint)MirClass.Archer)
            {
                goSheShou.SetActive(true);
                textClass.text = "射手";
            }
            else
            {
                PrintTool.Assert(false);
            }
        }
        else
        {
            goHaveRole.SetActive(false);
        }
    }
}
