using NetProto.ShareData;

namespace Mir2
{
    public class UserHeroObject : UserObject
    {
        public bool AutoPot;
        public uint AutoHPPercent;
        public uint AutoMPPercent;

        public UserItem[] HPItem = new UserItem[1];
        public UserItem[] MPItem = new UserItem[1];
        // public override BuffDialog GetBuffDialog => GameScene.Scene.HeroBuffsDialog;
        public UserHeroObject(uint objectID):base(objectID)
        {
            ObjectID = objectID;
            Stats = new Stats();
            Frames = FrameSet.Player;
        }

        public override void Load(packet_data_UserInfo info)
        {
            Name = info.Name;
            //NameColour = info.NameColour;
            Class = (MirClass)info.Class;
            Gender = (MirGender)info.Gender;
            Level = (ushort)info.NLevel;
            // Hair = info.Hair;

            HP = (int)info.HP;
            MP = (int)info.MP;

            Experience = (long)info.NLevelExp;
            // MaxExperience = info.MaxExperience;

            //Inventory = info.Inventory;
            //Equipment = info.Equipment;

            //Magics = info.Magics;
            for (int i = 0; i < Magics.Count; i++)
            {
                Magics[i].CastTime += CMain.Time;
            }

            BindAllItems();
        }
    }
}
