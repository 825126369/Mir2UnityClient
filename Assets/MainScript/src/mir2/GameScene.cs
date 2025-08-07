using System.Collections.Generic;

namespace Mir2
{
    public sealed class GameScene
    {
        public class UserId
        {
            public long Id = 0;
            public string UserName = "";
        }

        public class BigMapRecord
        {
            public int Index;
            public ClientMapInfo MapInfo;
            //public Dictionary<ClientMovementInfo, MirButton> MovementButtons = new Dictionary<ClientMovementInfo, MirButton>();
            //public List<BigMapNPCRow> NPCButtons = new List<BigMapNPCRow>();

            public BigMapRecord() { }
        }

        public static bool Observing;
        public static bool AllowObserve;
        public static long MoveTime, AttackTime, NextRunTime, LogTime, 
            LastRunTime, ChangePModeTime, ChangeAModeTime, HeroSpellTime, IntelligentCreaturePickupTime;
        public static bool CanMove, CanRun;

        public static UserObject User
        {
            get { return MapObject.User; }
            set { MapObject.User = value; }
        }

        public AttackMode AMode;
        public PetMode PMode;
        public LightSetting Lights;
        public static long NPCTime;
        public static uint NPCID;
        public static float NPCRate;
        public static PanelType NPCPanelType;
        public static uint DefaultNPCID;
        public static bool HideAddedStoreStats;
        public static long SpellTime;
        public static long UseItemTime, PickUpTime, DropViewTime, TargetDeadTime;

        public static List<ItemInfo> ItemInfoList = new List<ItemInfo>();
        public static List<UserId> UserIdList = new List<UserId>();
        public static List<UserItem> ChatItemList = new List<UserItem>();
        public static List<ClientQuestInfo> QuestInfoList = new List<ClientQuestInfo>();
        public static List<GameShopItem> GameShopInfoList = new List<GameShopItem>();
        public static List<ClientRecipeInfo> RecipeInfoList = new List<ClientRecipeInfo>();
        public static Dictionary<int, BigMapRecord> MapInfoList = new Dictionary<int, BigMapRecord>();
        public static List<ClientHeroInformation> HeroInfoList = new List<ClientHeroInformation>();
        public static ClientHeroInformation[] HeroStorage = new ClientHeroInformation[8];
        public static Dictionary<long, RankCharacterInfo> RankingList = new Dictionary<long, RankCharacterInfo>();
    }
}