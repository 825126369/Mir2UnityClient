using Mir2;
using NetProto.ShareData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using C = NetProto.CSPacket;
using S = NetProto.SCPacket;

public class SelectRoleView : MonoBehaviour
{
    public SelectRoleItem mItemPrefab;
    public Image roleDisplay;
    public Text lastRefreshTime;
    public Button createBtn;
    public Button deleteBtn;
    public Button startBtn;

    private int nSelectRoleIndex = -1;
    private readonly List<SelectRoleItem> mItemList = new List<SelectRoleItem>();
    private bool bInit = false;

    private void Init()
    {
        if (bInit) return;
        bInit = true;

        for (int i = 0; i < 5; i++)
        {
            var go = Instantiate(mItemPrefab.gameObject) as GameObject;
            go.transform.SetParent(mItemPrefab.transform.parent);
            go.transform.localScale = Vector3.one;
            var mItem = go.GetComponent<SelectRoleItem>();
            mItem.Init(this);
            mItemList.Add(mItem);
            mItem.gameObject.SetActive(false);
        }

        createBtn.onClick.AddListener(() =>
        {
            UIMgr.Instance.Show_CreateRoleView();
        });

        deleteBtn.onClick.AddListener(() =>
        {
            if (nSelectRoleIndex >= 0)
            {
                UIMgr.CommonWindowLoading.Show();
                var mSendMsg = new C.packet_cs_DeleteCharacter();
                mSendMsg.CharacterIndex = (uint)nSelectRoleIndex;
                NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_SELECTROLE_DELETE_ROLE, mSendMsg);
            }
        });

        startBtn.onClick.AddListener(() =>
        {
            if (nSelectRoleIndex >= 0)
            {
                UIMgr.CommonWindowLoading.Show();
                var mSendMsg = new C.packet_cs_StartGame();
                mSendMsg.CharacterIndex = (uint)nSelectRoleIndex;
                NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_STARTGAME, mSendMsg);
            }
        });

        EventMgr.Instance.AddListener(GameEvent.packet_sc_DeleteCharacter, OnEvent_DeleteCharacter);
        EventMgr.Instance.AddListener(GameEvent.packet_sc_NewCharacter, OnEvent_NewCharacter);
        EventMgr.Instance.AddListener(GameEvent.packet_sc_request_AllRoleInfo, OnEvent_request_AllRoleInfo);
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(GameEvent.packet_sc_DeleteCharacter, OnEvent_DeleteCharacter);
        EventMgr.Instance.RemoveListener(GameEvent.packet_sc_NewCharacter, OnEvent_NewCharacter);
        EventMgr.Instance.RemoveListener(GameEvent.packet_sc_request_AllRoleInfo, OnEvent_request_AllRoleInfo);
    }

    public void Show()
    {
        this.Init();
        this.gameObject.SetActive(true);
        this.nSelectRoleIndex = 0;
        this.RefreshView();
    }
    
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEvent_DeleteCharacter(object data)
    {
        UIMgr.CommonWindowLoading.Hide();
        List<packet_data_SelectInfo> mList = SelectRoleModel.Instance.m_packet_data_SelectInfo_List;
        mList.RemoveAll((packet_data_SelectInfo x)=>
        {
            return x.Index == nSelectRoleIndex;
        });

        mList.Sort((x, y) =>
        {
            return (int)x.LastAccess - (int)y.LastAccess;
        });
    }

    private void OnEvent_NewCharacter(object data)
    {
        this.RefreshView();
    }

    private void OnEvent_request_AllRoleInfo(object data)
    {
        this.RefreshView();
    }

    public void OnSelectRoldId(int nIndex)
    {
        PrintTool.Log("OnSelectRoldId: " + nIndex);
        this.nSelectRoleIndex = nIndex;
        this.RefreshView();
    }

    public void RefreshView()
    {
        List<packet_data_SelectInfo> mRoleList = SelectRoleModel.Instance.m_packet_data_SelectInfo_List;
        packet_data_SelectInfo mSelectRoleInfo = mRoleList.Find((x) => x.Index == nSelectRoleIndex);
        if (mSelectRoleInfo == null)
        {
            nSelectRoleIndex = -1;
            if (nSelectRoleIndex == 0)
            {
                if (mRoleList.Count > 0)
                {
                    nSelectRoleIndex = (int)mRoleList[0].Index;
                }
            }
        }
        mSelectRoleInfo = mRoleList.Find((x) => x.Index == nSelectRoleIndex);

        mItemPrefab.gameObject.SetActive(false);
        for (int i = 0; i < mRoleList.Count; i++)
        {
            SelectRoleItem mItem = null;
            if (i >= mItemList.Count)
            {
                var go = Instantiate(mItemPrefab.gameObject) as GameObject;
                go.transform.SetParent(mItemPrefab.transform.parent);
                go.transform.localScale = Vector3.one;
                mItem = go.GetComponent<SelectRoleItem>();
                mItem.Init(this);
                mItemList.Add(mItem);
            }
            else
            {
                mItem = mItemList[i];
            }

            mItem.gameObject.SetActive(true);
            var mData = mRoleList[i];
            mItem.Refresh(mData);
            mItem.OnSelect(this.nSelectRoleIndex);
        }

        for (int i = mRoleList.Count; i < mItemList.Count; i++)
        {
            if (i < 5)
            {
                mItemList[i].gameObject.SetActive(true);
                mItemList[i].Refresh(null);
            }
            else
            {
                mItemList[i].gameObject.SetActive(false);
            }
        }

        if (mRoleList.Count > 0)
        {
            roleDisplay.gameObject.SetActive(true);
            lastRefreshTime.text = TimeTool.GetLocalTimeFromTimeStamp(mSelectRoleInfo.LastAccess).ToString("yyyy/MM/dd HH:mm:ss");

            var mAnimationImage = roleDisplay.GetComponent<AnimationImage>();
            var mClass = mSelectRoleInfo.Class;
            if (mClass == (uint)MirClass.Warrior)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    mAnimationImage.SetAniParam("boy_zhanshi_ani", 20, 16, 0.1f);
                }
                else
                {
                    mAnimationImage.SetAniParam("girl_zhanshi_ani", 300, 16, 0.1f);
                }
            }
            else if (mClass == (uint)MirClass.Wizard)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    mAnimationImage.SetAniParam("boy_fashi_ani", 40, 16, 0.1f);
                }
                else
                {
                    mAnimationImage.SetAniParam("girl_fashi_ani", 320, 16, 0.1f);
                }
            }
            else if (mClass == (uint)MirClass.Taoist)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    mAnimationImage.SetAniParam("boy_daoshi_ani", 60, 16, 0.1f);
                }
                else
                {
                    mAnimationImage.SetAniParam("girl_daoshi_ani", 340, 16, 0.1f);
                }
            }
            else if (mClass == (uint)MirClass.Assassin)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    mAnimationImage.SetAniParam("boy_cike_ani", 80, 16, 0.1f);
                }
                else
                {
                    mAnimationImage.SetAniParam("girl_cike_ani", 360, 16, 0.1f);
                }
            }
            else if (mClass == (uint)MirClass.Archer)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
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
        else
        {
            roleDisplay.gameObject.SetActive(false);
            lastRefreshTime.text = string.Empty;
        }
    }

}
