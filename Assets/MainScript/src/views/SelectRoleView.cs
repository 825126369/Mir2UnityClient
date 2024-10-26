using NetProtocols.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using XKNet.Common;

public class SelectRoleView : MonoBehaviour
{
    public SelectRoleItem mItemPrefab;
    public Image roleDisplay;
    public Text lastRefreshTime;
    public Button createBtn;
    public Button deleteBtn;
    public Button startBtn;

    private uint nSelectRoleId = 0;
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

        DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.addDataBind(RefreshView);

        createBtn.onClick.AddListener(() =>
        {
            UIMgr.Instance.Show_CreateRoleView();
        });

        deleteBtn.onClick.AddListener(() =>
        {
            if(nSelectRoleId > 0)
            {
                var mSendMsg = IMessagePool<packet_cs_request_DeleteRole>.Pop();
                mSendMsg.NPlayerId = nSelectRoleId;
                NetClientGameMgr.Instance.mNetClient.SendNetData(NetProtocolCommand.CS_REQUEST_SELECTROLE_DELETE_ROLE, mSendMsg);
                IMessagePool<packet_cs_request_DeleteRole>.recycle(mSendMsg);
            }
        });

        startBtn.onClick.AddListener(() =>
        {
            if (nSelectRoleId > 0)
            {
                
            }
        });
    }

    private void OnDestroy()
    {
        DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.removeDataBind(RefreshView);
    }

    public void Show()
    {
        Init();
        gameObject.SetActive(true);

        this.nSelectRoleId = 0;
        var mRoleList = DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.bindData;
        RefreshView(mRoleList);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnSelectRoldId(uint Id)
    {
        this.nSelectRoleId = Id;
        var mRoleList = DataCenter.Instance.mDataBind_packet_data_SelectRole_RoleInfo.bindData;
        RefreshView(mRoleList);
    }

    public void RefreshView(List<packet_data_SelectRole_RoleInfo> mRoleList)
    {
        if (nSelectRoleId == 0)
        {
            ulong nMaxLoginTime = 0;
            foreach (var v in mRoleList)
            {
                if (nMaxLoginTime < v.NLastLoginTime)
                {
                    nMaxLoginTime = v.NLastLoginTime;
                    nSelectRoleId = v.NRoleId;
                }
            }
        }

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
            mItem.OnSelect(this.nSelectRoleId);
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
            packet_data_SelectRole_RoleInfo mSelectRoleInfo = mRoleList.Find((x) => x.NRoleId == nSelectRoleId);
            lastRefreshTime.text = TimeTool.GetLocalTimeFromTimeStamp(mSelectRoleInfo.NLastLoginTime).ToLongTimeString();
            if (mSelectRoleInfo.Class == (uint)MirClass.Warrior)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("boy_zhanshi_ani");
                }
                else
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("girl_zhanshi_ani");
                }
            }
            else if (mSelectRoleInfo.Class == (uint)MirClass.Wizard)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("boy_fashi_ani");
                }
                else
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("girl_fashi_ani");
                }
            }
            else if (mSelectRoleInfo.Class == (uint)MirClass.Taoist)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("boy_daoshi_ani");
                }
                else
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("girl_daoshi_ani");
                }
            }
            else if (mSelectRoleInfo.Class == (uint)MirClass.Assassin)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("boy_cike_ani");
                }
                else
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("girl_cike_ani");
                }
            }
            else if (mSelectRoleInfo.Class == (uint)MirClass.Archer)
            {
                if (mSelectRoleInfo.Gender == (uint)MirGender.Male)
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("boy_sheshou_ani");
                }
                else
                {
                    roleDisplay.GetComponent<AnimationImage>().mSpriteAtlas = ResCenter.Instance.mBundleGameAllRes.GetAtlas("girl_sheshou_ani");
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
