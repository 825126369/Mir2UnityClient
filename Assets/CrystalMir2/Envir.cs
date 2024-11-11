using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CrystalMir2
{
    public class Envir
    {
        public static int LoadVersion;
        public static int LoadCustomVersion;

        public const int MinVersion = 60;
        public const int Version = 110;
        public const int CustomVersion = 0;

        public static int MapIndex;
        public static int ItemIndex;
        public static int MonsterIndex;
        public static int NPCIndex;
        public static int QuestIndex;
        public static int GameshopIndex;
        public static int ConquestIndex;
        public static int RespawnIndex;
        public static int ScriptIndex;

        public static List<MapInfo> MapInfoList = new List<MapInfo>();

        const string DatabasePath = "D:\\Me\\MyProject\\CrystalMir2_Chinese\\Server\\Debug\\Server.MirDB";

        public static bool LoadDB()
        {
            using (var stream = File.OpenRead(DatabasePath))
            using (var reader = new BinaryReader(stream))
            {
                LoadVersion = reader.ReadInt32();
                LoadCustomVersion = reader.ReadInt32();

                if (LoadVersion < MinVersion)
                {
                    Debug.LogError($"Cannot load a database version {LoadVersion}. Mininum supported is {MinVersion}.");
                    return false;
                }
                else if (LoadVersion > Version)
                {
                    Debug.LogError($"Cannot load a database version {LoadVersion}. Maximum supported is {Version}.");
                    return false;
                }

                MapIndex = reader.ReadInt32();
                ItemIndex = reader.ReadInt32();
                MonsterIndex = reader.ReadInt32();

                NPCIndex = reader.ReadInt32();
                QuestIndex = reader.ReadInt32();

                if (LoadVersion >= 63)
                {
                    GameshopIndex = reader.ReadInt32();
                }

                if (LoadVersion >= 66)
                {
                    ConquestIndex = reader.ReadInt32();
                }

                if (LoadVersion >= 68)
                    RespawnIndex = reader.ReadInt32();


                var count = reader.ReadInt32();
                MapInfoList.Clear();
                for (var i = 0; i < count; i++)
                    MapInfoList.Add(new MapInfo(reader));

                //count = reader.ReadInt32();
                //ItemInfoList.Clear();
                //for (var i = 0; i < count; i++)
                //{
                //    ItemInfoList.Add(new ItemInfo(reader, LoadVersion, LoadCustomVersion));
                //    if (ItemInfoList[i] != null && ItemInfoList[i].RandomStatsId < Settings.RandomItemStatsList.Count)
                //    {
                //        ItemInfoList[i].RandomStats = Settings.RandomItemStatsList[ItemInfoList[i].RandomStatsId];
                //    }
                //}
                //count = reader.ReadInt32();
                //MonsterInfoList.Clear();
                //for (var i = 0; i < count; i++)
                //    MonsterInfoList.Add(new MonsterInfo(reader));

                //count = reader.ReadInt32();
                //NPCInfoList.Clear();
                //for (var i = 0; i < count; i++)
                //    NPCInfoList.Add(new NPCInfo(reader));

                //count = reader.ReadInt32();
                //QuestInfoList.Clear();
                //for (var i = 0; i < count; i++)
                //    QuestInfoList.Add(new QuestInfo(reader));

                //DragonInfo = new DragonInfo(reader);
                //count = reader.ReadInt32();
                //for (var i = 0; i < count; i++)
                //{
                //    var m = new MagicInfo(reader, LoadVersion, LoadCustomVersion);
                //    if (!MagicExists(m.Spell))
                //        MagicInfoList.Add(m);
                //}

                //FillMagicInfoList();
                //if (LoadVersion <= 70)
                //    UpdateMagicInfo();

                //if (LoadVersion >= 63)
                //{
                //    count = reader.ReadInt32();
                //    GameShopList.Clear();
                //    for (var i = 0; i < count; i++)
                //    {
                //        var item = new GameShopItem(reader, LoadVersion, LoadCustomVersion);
                //        if (Main.BindGameShop(item))
                //        {
                //            GameShopList.Add(item);
                //        }
                //    }
                //}

                //if (LoadVersion >= 66)
                //{
                //    ConquestInfoList.Clear();
                //    count = reader.ReadInt32();
                //    for (var i = 0; i < count; i++)
                //    {
                //        ConquestInfoList.Add(new ConquestInfo(reader));
                //    }
                //}

                //if (LoadVersion > 67)
                //    RespawnTick = new RespawnTimer(reader);

            }
            return true;
        }
    }
}