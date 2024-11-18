using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public uint nId;
    public uint nAccountId;

    public readonly List<ItemData> mItemList = new List<ItemData>();
    public string Name;
    public uint nLevel; //等级
    public ulong nLevelExp; //等级
    public uint Class; //职业
    public uint Gender;//性别
    public byte Hair; //头发

    public uint nCoinCount;

    public uint Index; //创建的索引
    public uint GuildIndex; //工会
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
    public ulong MarriedDate; //结婚

    //Mentor
    public uint Mentor; //拜师
    public ulong MentorDate;
    public bool IsMentor;
    public ulong MentorExp;
    //Location
    public uint CurrentMapIndex; //当前地图Index
    public Vector3Int CurrentLocation;
    public uint Direction;

    public uint HP;
    public uint MP;
}