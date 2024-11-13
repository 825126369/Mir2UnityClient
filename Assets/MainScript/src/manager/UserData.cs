using Mir2;
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

    public readonly List<ItemData> mEquipList = new List<ItemData>();
    public readonly List<ItemData> mBagList = new List<ItemData>();

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
    public Vector3Int MapLocation;
    public MirDirection Direction;

    public uint HP;
    public uint MP;

    //---------------------------------------------------------------------------------------
    public int Weapon = -1;
    public int WeaponEffect = 0;
    public int Armour = 0;
    public int WingEffect = 0;

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
        this.CurrentMapIndex = netUserInfo.NMapIndex;
        this.Direction = (MirDirection)netUserInfo.Direction;
    }

    private void RefreshEquipmentStats()
    {
        Weapon = -1;
        WeaponEffect = 0;
        Armour = 0;
        WingEffect = 0;
      

        //for (int i = 0; i < mEquipList.Length; i++)
        //{
        //    ItemData temp = mEquipList[i];
        //    if (temp == null) continue;

        //    ItemInfo realItem = Functions.GetRealItem(temp.Info, Level, Class, GameScene.ItemInfoList);

        //    if (realItem.Type == ItemType.Weapon || realItem.Type == ItemType.Torch)
        //        CurrentHandWeight = (int)Math.Min(int.MaxValue, CurrentHandWeight + temp.Weight);
        //    else
        //        CurrentWearWeight = (int)Math.Min(int.MaxValue, CurrentWearWeight + temp.Weight);

        //    if (temp.CurrentDura == 0 && realItem.Durability > 0) continue;

        //    if (realItem.Type == ItemType.Armour)
        //    {
        //        Armour = realItem.Shape;
        //        WingEffect = realItem.Effect;
        //    }
        //    if (realItem.Type == ItemType.Weapon)
        //    {
        //        Weapon = realItem.Shape;
        //        WeaponEffect = realItem.Effect;
        //    }

        //    if (realItem.Type == ItemType.Mount)
        //    {
        //        MountType = realItem.Shape;
        //    }

        //    if (temp.Info.IsFishingRod) continue;

        //    Stats.Add(realItem.Stats);
        //    Stats.Add(temp.AddedStats);

        //    Stats[Stat.MinAC] += temp.Awake.GetAC();
        //    Stats[Stat.MaxAC] += temp.Awake.GetAC();
        //    Stats[Stat.MinMAC] += temp.Awake.GetMAC();
        //    Stats[Stat.MaxMAC] += temp.Awake.GetMAC();

        //    Stats[Stat.MinDC] += temp.Awake.GetDC();
        //    Stats[Stat.MaxDC] += temp.Awake.GetDC();
        //    Stats[Stat.MinMC] += temp.Awake.GetMC();
        //    Stats[Stat.MaxMC] += temp.Awake.GetMC();
        //    Stats[Stat.MinSC] += temp.Awake.GetSC();
        //    Stats[Stat.MaxSC] += temp.Awake.GetSC();

        //    Stats[Stat.HP] += temp.Awake.GetHPMP();
        //    Stats[Stat.MP] += temp.Awake.GetHPMP();

        //    if (realItem.Light > Light) Light = realItem.Light;
        //    if (realItem.Unique != SpecialItemMode.None)
        //    {
        //        ItemMode |= realItem.Unique;
        //    }

        //    if (realItem.CanFastRun)
        //    {
        //        FastRun = true;
        //    }

        //    RefreshSocketStats(temp);

        //    if (realItem.Set == ItemSet.None) continue;

        //    ItemSets itemSet = ItemSets.Where(set => set.Set == realItem.Set && !set.Type.Contains(realItem.Type) && !set.SetComplete).FirstOrDefault();

        //    if (itemSet != null)
        //    {
        //        itemSet.Type.Add(realItem.Type);
        //        itemSet.Count++;
        //    }
        //    else
        //    {
        //        ItemSets.Add(new ItemSets { Count = 1, Set = realItem.Set, Type = new List<ItemType> { realItem.Type } });
        //    }

        //    //Mir Set
        //    if (realItem.Set == ItemSet.Mir)
        //    {
        //        if (!MirSet.Contains((EquipmentSlot)i))
        //            MirSet.Add((EquipmentSlot)i);
        //    }
        //}

        //if (ItemMode.HasFlag(SpecialItemMode.Muscle))
        //{
        //    Stats[Stat.BagWeight] = Stats[Stat.BagWeight] * 2;
        //    Stats[Stat.WearWeight] = Stats[Stat.WearWeight] * 2;
        //    Stats[Stat.HandWeight] = Stats[Stat.HandWeight] * 2;
        //}
    }

}