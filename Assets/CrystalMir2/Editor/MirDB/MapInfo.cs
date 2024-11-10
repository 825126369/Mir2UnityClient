using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mir2Editor
{
    public class SafeZoneInfo
    {
        public Vector3Int Location;
        public ushort Size;
        public bool StartPoint;

        public SafeZoneInfo(BinaryReader reader)
        {
            Location = new Vector3Int(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            StartPoint = reader.ReadBoolean();
        }

        public override string ToString()
        {
            return string.Format("Map: {0}- {1}", Location, StartPoint);
        }
    }

    public class RespawnInfo
    {
        public int MonsterIndex;
        public Vector3Int Location;
        public ushort Count;
        public ushort Spread;
        public ushort Delay;
        public ushort RandomDelay;
        public byte Direction;

        public string RoutePath = string.Empty;
        public int RespawnIndex;
        public bool SaveRespawnTime = false;
        public ushort RespawnTicks;

        public RespawnInfo(BinaryReader reader, int Version, int Customversion)
        {
            MonsterIndex = reader.ReadInt32();

            Location = new Vector3Int(reader.ReadInt32(), reader.ReadInt32());

            Count = reader.ReadUInt16();
            Spread = reader.ReadUInt16();

            Delay = reader.ReadUInt16();
            Direction = reader.ReadByte();

            RoutePath = reader.ReadString();

            if (Version > 67)
            {
                RandomDelay = reader.ReadUInt16();
                RespawnIndex = reader.ReadInt32();
                SaveRespawnTime = reader.ReadBoolean();
                RespawnTicks = reader.ReadUInt16();
            }
            else
            {
                RespawnIndex = ++Envir.RespawnIndex;
            }
        }

        public override string ToString()
        {
            return string.Format("Monster: {0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9}", MonsterIndex, Location, Count, Spread, Delay, Direction, RandomDelay, RespawnIndex, SaveRespawnTime, RespawnTicks);
        }
    }

    public class MovementInfo
    {
        public int MapIndex;
        public Vector3Int Source;
        public Vector3Int Destination;
        public bool NeedHole;
        public bool NeedMove;
        public bool ShowOnBigMap;
        public int ConquestIndex;
        public int Icon;

        public MovementInfo(BinaryReader reader)
        {
            MapIndex = reader.ReadInt32();
            Source = new Vector3Int(reader.ReadInt32(), reader.ReadInt32());
            Destination = new Vector3Int(reader.ReadInt32(), reader.ReadInt32());

            NeedHole = reader.ReadBoolean();
            NeedMove = reader.ReadBoolean();

            if (Envir.LoadVersion < 69) return;
            ConquestIndex = reader.ReadInt32();

            if (Envir.LoadVersion < 95) return;
            ShowOnBigMap = reader.ReadBoolean();
            Icon = reader.ReadInt32();
        }

        public override string ToString()
        {
            return string.Format("{0} -> Map :{1} - {2}", Source, MapIndex, Destination);
        }
    }

    public class MineZone
    {
        public byte Mine;
        public Vector3Int Location;
        public ushort Size;

        public MineZone(BinaryReader reader)
        {
            Location = new Vector3Int(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            Mine = reader.ReadByte();
        }

        public override string ToString()
        {
            return string.Format("Mine: {0}- {1}", Location, Mine);
        }
    }

    public class MapInfo
    {
        public int Index;
        public string FileName = string.Empty;
        public string Title = string.Empty;
        public ushort MiniMap;
        public ushort BigMap;
        public ushort Music;
        public LightSetting Light;
        public byte MapDarkLight = 0;
        public byte MineIndex = 0;

        public bool NoTeleport;
        public bool NoReconnect;
        public bool NoRandom;
        public bool NoEscape;
        public bool NoRecall;
        public bool NoDrug;
        public bool NoPosition;
        public bool NoFight;
        public bool NoThrowItem;
        public bool NoDropPlayer;
        public bool NoDropMonster;
        public bool NoNames;
        public bool NoMount;
        public bool NeedBridle;
        public bool Fight;
        public bool NeedHole;
        public bool Fire;
        public bool Lightning;
        public bool NoTownTeleport;
        public bool NoReincarnation;

        public string NoReconnectMap = string.Empty;
        public int FireDamage;
        public int LightningDamage;

        public List<SafeZoneInfo> SafeZones = new List<SafeZoneInfo>();
        public List<MovementInfo> Movements = new List<MovementInfo>();
        public List<RespawnInfo> Respawns = new List<RespawnInfo>();
        public List<MineZone> MineZones = new List<MineZone>();
        public WeatherSetting WeatherParticles = WeatherSetting.None;

        public override string ToString()
        {
            return string.Format("{0}: {1}", Index, Title);
        }

        public MapInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            FileName = reader.ReadString();
            Title = reader.ReadString();
            MiniMap = reader.ReadUInt16();
            Light = (LightSetting)reader.ReadByte();

            BigMap = reader.ReadUInt16();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                SafeZones.Add(new SafeZoneInfo(reader) { });

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Respawns.Add(new RespawnInfo(reader, Envir.LoadVersion, Envir.LoadCustomVersion));

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Movements.Add(new MovementInfo(reader));

            NoTeleport = reader.ReadBoolean();
            NoReconnect = reader.ReadBoolean();
            NoReconnectMap = reader.ReadString();

            NoRandom = reader.ReadBoolean();
            NoEscape = reader.ReadBoolean();
            NoRecall = reader.ReadBoolean();
            NoDrug = reader.ReadBoolean();
            NoPosition = reader.ReadBoolean();
            NoThrowItem = reader.ReadBoolean();
            NoDropPlayer = reader.ReadBoolean();
            NoDropMonster = reader.ReadBoolean();
            NoNames = reader.ReadBoolean();
            Fight = reader.ReadBoolean();
            Fire = reader.ReadBoolean();
            FireDamage = reader.ReadInt32();
            Lightning = reader.ReadBoolean();
            LightningDamage = reader.ReadInt32();
            MapDarkLight = reader.ReadByte();
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                MineZones.Add(new MineZone(reader));
            MineIndex = reader.ReadByte();
            NoMount = reader.ReadBoolean();
            NeedBridle = reader.ReadBoolean();
            NoFight = reader.ReadBoolean();
            Music = reader.ReadUInt16();

            if (Envir.LoadVersion < 78) return;
            NoTownTeleport = reader.ReadBoolean();
            if (Envir.LoadVersion < 79) return;
            NoReincarnation = reader.ReadBoolean();

            if (Envir.LoadVersion >= 110)
            {
                WeatherParticles = (WeatherSetting)reader.ReadUInt16();
            }
        }
    }
}
