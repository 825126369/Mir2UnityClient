using NetProtocols.Game;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public uint nUniqueId;
    public uint nCount;
    public uint nItemId;

    public uint nSlotIndex;
    public uint nStarCount;
    public uint nStarLevel;
    public uint nDura;
}

public class UserData
{
    public uint nId;
    public uint nWorldObjectId;

    public readonly List<ItemData> mItemList = new List<ItemData>();
    public string Name;
    public uint nLevel; //�ȼ�
    public ulong nLevelExp; //�ȼ�
    public uint Class; //ְҵ
    public uint Gender;//�Ա�
    public byte Hair; //ͷ��

    public uint nCoinCount;

    public uint Index; //����������
    public uint GuildIndex; //����
    public string CreationIP;
    public ulong CreationDate;

    public bool Banned;
    public string BanReason;
    public ulong ExpiryDate;

    public bool ChatBanned;
    public ulong ChatBanExpiryDate;

    public string LastIP;
    public ulong LastLogoutDate;
    public ulong LastLoginDate;

    public bool Deleted;
    public ulong DeleteDate;

    //Marriage
    public uint Married;
    public ulong MarriedDate; //���

    //Mentor
    public uint Mentor; //��ʦ
    public ulong MentorDate;
    public bool IsMentor;
    public ulong MentorExp;
    //Location
    public uint CurrentMapIndex; //��ǰ��ͼIndex
    public Vector3Int MapLocation;
    public uint Direction;

    public uint HP;
    public uint MP;

    public void CopyFrom(packet_data_UserInfo netUserInfo)
    {
        this.nId = netUserInfo.NPlayerId;
        this.nLevel = netUserInfo.NLevel;
        this.nLevelExp = netUserInfo.NLevelExp;
        this.Class = netUserInfo.Class;
        this.Gender = netUserInfo.Gender;
        this.Name = netUserInfo.Name;
        this.nWorldObjectId = netUserInfo.NMapObjectId;
        this.MapLocation = netUserInfo.Location.ToVector3Int();
    }

}