using AKNet.Common;
using AKNet.Extentions.Protobuf;
using Mir2;
using NetProtocols.Game;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoleView : MonoBehaviour
{
    public Image roleDisplay;
    public Text textRoleDes;
    public InputField mInputName;
    public Button mBoyBtn;
    public Button mGirlBtn;
    public Button mZhanShiBtn;
    public Button mFaShiBtn;
    public Button mDaoShiBtn;
    public Button mCiKeShiBtn;
    public Button mSheShouBtn;

    public Button createBtn;
    public Button cancelBtn;

    private MirClass mClass;
    private MirGender mGender;

    private bool bInit = false;
    private void Init()
    {
        if (bInit) return;
        bInit = true;

        mBoyBtn.onClick.AddListener(() =>
        {
            mGender = MirGender.Male;
            Refresh();
        });

        mGirlBtn.onClick.AddListener(() =>
        {
            mGender = MirGender.Female;
            Refresh();
        });

        mZhanShiBtn.onClick.AddListener(() =>
        {
            mClass = MirClass.Warrior;
            Refresh();
        });

        mFaShiBtn.onClick.AddListener(() =>
        {
            mClass = MirClass.Wizard;
            Refresh();
        });

        mDaoShiBtn.onClick.AddListener(() =>
        {
            mClass = MirClass.Taoist;
            Refresh();
        });

        mCiKeShiBtn.onClick.AddListener(() =>
        {
            mClass = MirClass.Assassin;
            Refresh();
        });

        mSheShouBtn.onClick.AddListener(() =>
        {
            mClass = MirClass.Archer;
            Refresh();
        });

        cancelBtn.onClick.AddListener(() =>
        {
            this.Hide();
        });

        createBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrWhiteSpace(mInputName.text))
            {
                UIMgr.CommonDialogView.ShowOk("提示", "角色名不能为空");
                return;
            }

            UIMgr.CommonWindowLoading.Show();
            packet_cs_request_CreateRole mSendMsg = IMessagePool<packet_cs_request_CreateRole>.Pop();
            mSendMsg.NAccountId = DataCenter.Instance.nAccountId;
            mSendMsg.Class = (uint)mClass;
            mSendMsg.Gender = (uint)mGender;
            mSendMsg.Name = mInputName.text;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_SELECTROLE_CREATE_ROLE, mSendMsg);
            IMessagePool<packet_cs_request_CreateRole>.recycle(mSendMsg);
        });
    }

    public void Show()
    {
        Init();
        gameObject.SetActive(true);
        this.mClass = MirClass.Warrior;
        this.mGender = MirGender.Male;
        this.mInputName.text = string.Empty;
        this.Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        var mAnimationImage = roleDisplay.GetComponent<AnimationImage>();
        if (mClass == MirClass.Warrior)
        {
            textRoleDes.text = DataCenter.WarriorDescription;
            if (mGender == MirGender.Male)
            {
                mAnimationImage.SetAniParam("boy_zhanshi_ani", 20, 16, 0.1f);
            }
            else
            {
                mAnimationImage.SetAniParam("girl_zhanshi_ani", 300, 16, 0.1f);
            }
        }
        else if (mClass == MirClass.Wizard)
        {
            textRoleDes.text = DataCenter.WizardDescription;
            if (mGender == MirGender.Male)
            {
                mAnimationImage.SetAniParam("boy_fashi_ani", 40, 16, 0.1f);
            }
            else
            {
                mAnimationImage.SetAniParam("girl_fashi_ani", 320, 16, 0.1f);
            }
        }
        else if (mClass == MirClass.Taoist)
        {
            textRoleDes.text = DataCenter.TaoistDescription;
            if (mGender == MirGender.Male)
            {
                mAnimationImage.SetAniParam("boy_daoshi_ani", 60, 16, 0.1f);
            }
            else
            {
                mAnimationImage.SetAniParam("girl_daoshi_ani", 340, 16, 0.1f);
            }
        }
        else if (mClass == MirClass.Assassin)
        {
            textRoleDes.text = DataCenter.AssassinDescription;
            if (mGender == MirGender.Male)
            {
                mAnimationImage.SetAniParam("boy_cike_ani", 80, 16, 0.1f);
            }
            else
            {
                mAnimationImage.SetAniParam("girl_cike_ani", 360, 16, 0.1f);
            }
        }
        else if (mClass == MirClass.Archer)
        {
            textRoleDes.text = DataCenter.ArcherDescription;
            if (mGender == MirGender.Male)
            {
                mAnimationImage.SetAniParam("boy_sheshou_ani", 100, 16, 0.1f);
            }
            else
            {
                mAnimationImage.SetAniParam("girl_sheshou_ani", 140, 16, 0.1f);
            }
        }
        else
        {
            PrintTool.Assert(false);
        }
    }

}
