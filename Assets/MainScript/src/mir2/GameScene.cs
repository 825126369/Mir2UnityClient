namespace Mir2
{
    public sealed class GameScene
    {
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
    }
}