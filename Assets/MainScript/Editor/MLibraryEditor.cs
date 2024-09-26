using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEditor;
using UnityEngine;
using static UnityEditor.AddressableAssets.Build.BuildPipelineTasks.GenerateLocationListsTask;

public static class MLibraryEditor
{
    public const string OutDir = "D:/Me/MyProject/CrystalMir2/Client2/";
    public const string RootDir = "D:/Me/MyProject/CrystalMir2/Client/";
    public const string DataPath = RootDir + "Data/",
                    MapPath = RootDir + "Map/",
                    SoundPath = RootDir + "Sound/",
                    ExtraDataPath = RootDir + "Data/Extra/",
                    ShadersPath = RootDir + "Data/Shaders/",
                    MonsterPath = RootDir + "Data/Monster/",
                    GatePath = RootDir + "Data/Gate/",
                    FlagPath = RootDir + "Data/Flag/",
                    SiegePath = RootDir + "Data/Siege/",
                    NPCPath = RootDir + "Data/NPC/",
                    CArmourPath = RootDir + "Data/CArmour/",
                    CWeaponPath = RootDir + "Data/CWeapon/",
                    CWeaponEffectPath = RootDir + "Data/CWeaponEffect/",
                    CHairPath = RootDir + "Data/CHair/",
                    AArmourPath = RootDir + "Data/AArmour/",
                    AWeaponPath = RootDir + "Data/AWeapon/",
                    AHairPath = RootDir + "Data/AHair/",
                    ARArmourPath = RootDir + "Data/ARArmour/",
                    ARWeaponPath = RootDir + "Data/ARWeapon/",
                    ARHairPath = RootDir + "Data/ARHair/",
                    CHumEffectPath = RootDir + "Data/CHumEffect/",
                    AHumEffectPath = RootDir + "Data/AHumEffect/",
                    ARHumEffectPath = RootDir + "Data/ARHumEffect/",
                    MountPath = RootDir + "Data/Mount/",
                    FishingPath = RootDir + "Data/Fishing/",
                    PetsPath = RootDir + "Data/Pet/",
                    TransformPath = RootDir + "Data/Transform/",
                    TransformMountsPath = RootDir + "Data/TransformRide2/",
                    TransformEffectPath = RootDir + "Data/TransformEffect/",
                    TransformWeaponEffectPath = RootDir + "Data/TransformWeaponEffect/",
                    MouseCursorPath = RootDir + "Data/Cursors/",
                    ResourcePath = RootDir + "DirectX/",
                    UserDataPath = RootDir + "Data/UserData/";


    public static readonly MLibrary ChrSel = new MLibrary(DataPath + "ChrSel");
    public static readonly MLibrary Prguse = new MLibrary(DataPath + "Prguse");
    public static readonly MLibrary Prguse2 = new MLibrary(DataPath + "Prguse2");
    public static readonly MLibrary Prguse3 = new MLibrary(DataPath + "Prguse3");
    public static readonly MLibrary BuffIcon = new MLibrary(DataPath + "BuffIcon");
    public static readonly MLibrary Help = new MLibrary(DataPath + "Help");
    public static readonly MLibrary MiniMap = new MLibrary(DataPath + "MMap");
    public static readonly MLibrary MapLinkIcon = new MLibrary(DataPath + "MapLinkIcon");
    public static readonly MLibrary Title = new MLibrary(DataPath + "Title");
    public static readonly MLibrary MagIcon = new MLibrary(DataPath + "MagIcon");
    public static readonly MLibrary MagIcon2 = new MLibrary(DataPath + "MagIcon2");
    public static readonly MLibrary Magic = new MLibrary(DataPath + "Magic");
    public static readonly MLibrary Magic2 = new MLibrary(DataPath + "Magic2");
    public static readonly MLibrary Magic3 = new MLibrary(DataPath + "Magic3");
    public static readonly MLibrary Effect = new MLibrary(DataPath + "Effect");
    public static readonly MLibrary MagicC = new MLibrary(DataPath + "MagicC");
    public static readonly MLibrary GuildSkill = new MLibrary(DataPath + "GuildSkill");
    public static readonly MLibrary Weather = new MLibrary(DataPath + "Weather");

    public static readonly MLibrary Background = new MLibrary(DataPath + "Background");
    public static readonly MLibrary Dragon = new MLibrary(DataPath + "Dragon");
    //Map
    public static readonly MLibrary[] MapLibs = new MLibrary[400];

    //Items
    public static readonly MLibrary Items = new MLibrary(DataPath + "Items");
    public static readonly MLibrary StateItems = new MLibrary(DataPath + "StateItem");
    public static readonly MLibrary FloorItems = new MLibrary(DataPath + "DNItems");

    //Deco
    public static readonly MLibrary Deco = new MLibrary(DataPath + "Deco");

    public static MLibrary[] CArmours;
    public static MLibrary[] CWeapons;
    public static MLibrary[] CWeaponEffect;
    public static MLibrary[] CHair;
    public static MLibrary[] CHumEffect;
    public static MLibrary[] AArmours;
    public static MLibrary[] AWeaponsL;
    public static MLibrary[] AWeaponsR;
    public static MLibrary[] AHair;
    public static MLibrary[] AHumEffect;
    public static MLibrary[] ARArmours;
    public static MLibrary[] ARWeapons;
    public static MLibrary[] ARWeaponsS;
    public static MLibrary[] ARHair;
    public static MLibrary[] ARHumEffect;
    public static MLibrary[] Monsters;
    public static MLibrary[] Gates;
    public static MLibrary[] Flags;
    public static MLibrary[] Siege;
    public static MLibrary[] Mounts;
    public static MLibrary[] NPCs;
    public static MLibrary[] Fishing;
    public static MLibrary[] Pets;
    public static MLibrary[] Transform;
    public static MLibrary[] TransformMounts;
    public static MLibrary[] TransformEffect;
    public static MLibrary[] TransformWeaponEffect;

    static int Progress;
    static int Count;
    static bool Loaded;

    [MenuItem("Mir2Editor/解密资源")]
    public static void InitLibraries()
    {
        //Wiz/War/Tao
        InitLibrary(ref CArmours, CArmourPath, "00");
        InitLibrary(ref CHair, CHairPath, "00");
        InitLibrary(ref CWeapons, CWeaponPath, "00");
        InitLibrary(ref CWeaponEffect, CWeaponEffectPath, "00");
        InitLibrary(ref CHumEffect, CHumEffectPath, "00");

        //Assassin
        InitLibrary(ref AArmours, AArmourPath, "00");
        InitLibrary(ref AHair, AHairPath, "00");
        InitLibrary(ref AWeaponsL, AWeaponPath, "00", " L");
        InitLibrary(ref AWeaponsR, AWeaponPath, "00", " R");
        InitLibrary(ref AHumEffect, AHumEffectPath, "00");

        //Archer
        InitLibrary(ref ARArmours, ARArmourPath, "00");
        InitLibrary(ref ARHair, ARHairPath, "00");
        InitLibrary(ref ARWeapons, ARWeaponPath, "00");
        InitLibrary(ref ARWeaponsS, ARWeaponPath, "00", " S");
        InitLibrary(ref ARHumEffect, ARHumEffectPath, "00");

        //Other
        InitLibrary(ref Monsters, MonsterPath, "000");
        InitLibrary(ref Gates, GatePath, "00");
        InitLibrary(ref Flags, FlagPath, "00");
        InitLibrary(ref Siege, SiegePath, "00");
        InitLibrary(ref NPCs, NPCPath, "00");
        InitLibrary(ref Mounts, MountPath, "00");
        InitLibrary(ref Fishing, FishingPath, "00");
        InitLibrary(ref Pets, PetsPath, "00");
        InitLibrary(ref Transform, TransformPath, "00");
        InitLibrary(ref TransformMounts, TransformMountsPath, "00");
        InitLibrary(ref TransformEffect, TransformEffectPath, "00");
        InitLibrary(ref TransformWeaponEffect, TransformWeaponEffectPath, "00");

        #region Maplibs
        //wemade mir2 (allowed from 0-99)
        MapLibs[0] = new MLibrary(DataPath + "Map/WemadeMir2/Tiles");
        MapLibs[1] = new MLibrary(DataPath + "Map/WemadeMir2/Smtiles");
        MapLibs[2] = new MLibrary(DataPath + "Map/WemadeMir2/Objects");
        for (int i = 2; i < 28; i++)
        {
            MapLibs[i + 1] = new MLibrary(DataPath + "Map/WemadeMir2/Objects" + i.ToString());
        }
        MapLibs[90] = new MLibrary(DataPath + "Map/WemadeMir2/Objects_32bit");

        //shanda mir2 (allowed from 100-199)
        MapLibs[100] = new MLibrary(DataPath + "Map/ShandaMir2/Tiles");
        for (int i = 1; i < 10; i++)
        {
            MapLibs[100 + i] = new MLibrary(DataPath + "Map/ShandaMir2/Tiles" + (i + 1));
        }
        MapLibs[110] = new MLibrary(DataPath + "Map/ShandaMir2/SmTiles");
        for (int i = 1; i < 10; i++)
        {
            MapLibs[110 + i] = new MLibrary(DataPath + "Map/ShandaMir2/SmTiles" + (i + 1));
        }
        MapLibs[120] = new MLibrary(DataPath + "Map/ShandaMir2/Objects");
        for (int i = 1; i < 31; i++)
        {
            MapLibs[120 + i] = new MLibrary(DataPath + "Map/ShandaMir2/Objects" + (i + 1));
        }
        MapLibs[190] = new MLibrary(DataPath + "Map/ShandaMir2/AniTiles1");
        //wemade mir3 (allowed from 200-299)
        string[] Mapstate = { "", "wood/", "sand/", "snow/", "forest/" };
        for (int i = 0; i < Mapstate.Length; i++)
        {
            MapLibs[200 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Tilesc");
            MapLibs[201 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Tiles30c");
            MapLibs[202 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Tiles5c");
            MapLibs[203 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Smtilesc");
            MapLibs[204 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Housesc");
            MapLibs[205 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Cliffsc");
            MapLibs[206 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Dungeonsc");
            MapLibs[207 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Innersc");
            MapLibs[208 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Furnituresc");
            MapLibs[209 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Wallsc");
            MapLibs[210 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "smObjectsc");
            MapLibs[211 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Animationsc");
            MapLibs[212 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Object1c");
            MapLibs[213 + (i * 15)] = new MLibrary(DataPath + "Map/WemadeMir3/" + Mapstate[i] + "Object2c");
        }
        Mapstate = new string[] { "", "wood", "sand", "snow", "forest" };
        //shanda mir3 (allowed from 300-399)
        for (int i = 0; i < Mapstate.Length; i++)
        {
            MapLibs[300 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Tilesc" + Mapstate[i]);
            MapLibs[301 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Tiles30c" + Mapstate[i]);
            MapLibs[302 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Tiles5c" + Mapstate[i]);
            MapLibs[303 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Smtilesc" + Mapstate[i]);
            MapLibs[304 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Housesc" + Mapstate[i]);
            MapLibs[305 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Cliffsc" + Mapstate[i]);
            MapLibs[306 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Dungeonsc" + Mapstate[i]);
            MapLibs[307 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Innersc" + Mapstate[i]);
            MapLibs[308 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Furnituresc" + Mapstate[i]);
            MapLibs[309 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Wallsc" + Mapstate[i]);
            MapLibs[310 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "smObjectsc" + Mapstate[i]);
            MapLibs[311 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Animationsc" + Mapstate[i]);
            MapLibs[312 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Object1c" + Mapstate[i]);
            MapLibs[313 + (i * 15)] = new MLibrary(DataPath + "Map/ShandaMir3/" + "Object2c" + Mapstate[i]);
        }
        #endregion

        LoadLibraries();
        LoadGameLibraries();
    }

    static void InitLibrary(ref MLibrary[] library, string path, string toStringValue, string suffix = "")
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var allFiles = Directory.GetFiles(path, "*" + suffix + MLibrary.Extention, SearchOption.TopDirectoryOnly).OrderBy(x => int.Parse(Regex.Match(x, @"\d+").Value));

        var lastFile = allFiles.Count() > 0 ? Path.GetFileName(allFiles.Last()) : "0";

        var count = int.Parse(Regex.Match(lastFile, @"\d+").Value) + 1;

        library = new MLibrary[count];

        for (int i = 0; i < count; i++)
        {
            library[i] = new MLibrary(path + i.ToString(toStringValue) + suffix);
        }
    }

    static void LoadLibraries()
    {
        ChrSel.Initialize();
        Progress++;

        Prguse.Initialize();
        Progress++;

        Prguse2.Initialize();
        Progress++;

        Prguse3.Initialize();
        Progress++;

        Title.Initialize();
        Progress++;
    }

    private static void LoadGameLibraries()
    {
        Count = MapLibs.Length + Monsters.Length + Gates.Length + Flags.Length + Siege.Length + NPCs.Length + CArmours.Length +
            CHair.Length + CWeapons.Length + CWeaponEffect.Length + AArmours.Length + AHair.Length + AWeaponsL.Length + AWeaponsR.Length +
            ARArmours.Length + ARHair.Length + ARWeapons.Length + ARWeaponsS.Length +
            CHumEffect.Length + AHumEffect.Length + ARHumEffect.Length + Mounts.Length + Fishing.Length + Pets.Length +
            Transform.Length + TransformMounts.Length + TransformEffect.Length + TransformWeaponEffect.Length + 18;

        Dragon.Initialize();
        Progress++;

        BuffIcon.Initialize();
        Progress++;

        Help.Initialize();
        Progress++;

        MiniMap.Initialize();
        Progress++;
        MapLinkIcon.Initialize();
        Progress++;

        MagIcon.Initialize();
        Progress++;
        MagIcon2.Initialize();
        Progress++;

        Magic.Initialize();
        Progress++;
        Magic2.Initialize();
        Progress++;
        Magic3.Initialize();
        Progress++;
        MagicC.Initialize();
        Progress++;

        Effect.Initialize();
        Progress++;

        Weather.Initialize();
        Progress++;

        GuildSkill.Initialize();
        Progress++;

        Background.Initialize();
        Progress++;

        Deco.Initialize();
        Progress++;

        Items.Initialize();
        Progress++;
        StateItems.Initialize();
        Progress++;
        FloorItems.Initialize();
        Progress++;

        for (int i = 0; i < MapLibs.Length; i++)
        {
            if (MapLibs[i] == null)
                MapLibs[i] = new MLibrary("");
            else
                MapLibs[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < Monsters.Length; i++)
        {
            Monsters[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < Gates.Length; i++)
        {
            Gates[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < Flags.Length; i++)
        {
            Flags[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < Siege.Length; i++)
        {
            Siege[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < NPCs.Length; i++)
        {
            NPCs[i].Initialize();
            Progress++;
        }


        for (int i = 0; i < CArmours.Length; i++)
        {
            CArmours[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < CHair.Length; i++)
        {
            CHair[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < CWeapons.Length; i++)
        {
            CWeapons[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < CWeaponEffect.Length; i++)
        {
            CWeaponEffect[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < AArmours.Length; i++)
        {
            AArmours[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < AHair.Length; i++)
        {
            AHair[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < AWeaponsL.Length; i++)
        {
            AWeaponsL[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < AWeaponsR.Length; i++)
        {
            AWeaponsR[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < ARArmours.Length; i++)
        {
            ARArmours[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < ARHair.Length; i++)
        {
            ARHair[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < ARWeapons.Length; i++)
        {
            ARWeapons[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < ARWeaponsS.Length; i++)
        {
            ARWeaponsS[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < CHumEffect.Length; i++)
        {
            CHumEffect[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < AHumEffect.Length; i++)
        {
            AHumEffect[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < ARHumEffect.Length; i++)
        {
            ARHumEffect[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < Mounts.Length; i++)
        {
            Mounts[i].Initialize();
            Progress++;
        }


        for (int i = 0; i < Fishing.Length; i++)
        {
            Fishing[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < Pets.Length; i++)
        {
            Pets[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < Transform.Length; i++)
        {
            Transform[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < TransformEffect.Length; i++)
        {
            TransformEffect[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < TransformWeaponEffect.Length; i++)
        {
            TransformWeaponEffect[i].Initialize();
            Progress++;
        }

        for (int i = 0; i < TransformMounts.Length; i++)
        {
            TransformMounts[i].Initialize();
            Progress++;
        }

        Loaded = true;
    }
}



public sealed class MLibrary
{
    public const string Extention = ".Lib";
    public const int LibVersion = 3;
    private readonly string _filePath;
    private MImage[] _images;
    private int[] _indexList;
    private int _count;
    private bool _initialized;

    private BinaryReader _reader;
    private FileStream _fStream;

    private string ParentDirName;
    private string FileName;

    public MLibrary(string filePath)
    {
        _filePath = Path.ChangeExtension(filePath, Extention);
        ParentDirName = FileToolEditor.GetDirParentDir(_filePath);
        FileName = Path.GetFileName(_filePath);
    }

    public void Initialize()
    {
        _initialized = true;
        if (!File.Exists(_filePath))
            return;

        try
        {
            _fStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            _reader = new BinaryReader(_fStream);
            int currentVersion = _reader.ReadInt32();
            if (currentVersion < 2)
            {
                Debug.LogError("Wrong version, expecting lib version: " + LibVersion.ToString() + " found version: " + currentVersion.ToString() + ".");
                return;
            }
            _count = _reader.ReadInt32();

            int frameSeek = 0;
            if (currentVersion >= 3)
            {
                frameSeek = _reader.ReadInt32();
            }

            _images = new MImage[_count];
            _indexList = new int[_count];

            for (int i = 0; i < _count; i++)
                _indexList[i] = _reader.ReadInt32();

            if (currentVersion >= 3)
            {
                _fStream.Seek(frameSeek, SeekOrigin.Begin);
                var frameCount = _reader.ReadInt32();
               
            }
        }
        catch (Exception e)
        {
            _initialized = false;
            throw;
        }
    }

    private bool CheckImage(int index)
    {
        if (!_initialized)
            Initialize();

        if (_images == null || index < 0 || index >= _images.Length)
            return false;

        if (_images[index] == null)
        {
            _fStream.Position = _indexList[index];
            _images[index] = new MImage(_reader);
        }

        MImage mi = _images[index];
        if (!mi.TextureValid)
        {
            if ((mi.Width == 0) || (mi.Height == 0))
                return false;
            _fStream.Seek(_indexList[index] + 17, SeekOrigin.Begin);

            string path = Path.Combine(MLibraryEditor.OutDir, ParentDirName);
            string fileName = FileName + index + ".png";
            mi.CreateTexture(path, fileName, _reader);
        }

        return true;
    }

}


public sealed class MImage
{
    public short Width, Height, X, Y, ShadowX, ShadowY;
    public byte Shadow;
    public int Length;
    public bool TextureValid;
    public Texture2D Image;
    //layer 2:
    public short MaskWidth, MaskHeight, MaskX, MaskY;
    public int MaskLength;
    public Texture MaskImage;
    public bool HasMask;
    public long CleanTime;
    public byte[] Data;
    public byte[] MaskData;

    public MImage(BinaryReader reader)
    {
        //read layer 1
        Width = reader.ReadInt16();
        Height = reader.ReadInt16();
        X = reader.ReadInt16();
        Y = reader.ReadInt16();
        ShadowX = reader.ReadInt16();
        ShadowY = reader.ReadInt16();
        Shadow = reader.ReadByte();
        Length = reader.ReadInt32();

        //check if there's a second layer and read it
        HasMask = ((Shadow >> 7) == 1) ? true : false;
        if (HasMask)
        {
            reader.ReadBytes(Length);
            MaskWidth = reader.ReadInt16();
            MaskHeight = reader.ReadInt16();
            MaskX = reader.ReadInt16();
            MaskY = reader.ReadInt16();
            MaskLength = reader.ReadInt32();
        }
    }

    public void CreateTexture(string outParentDir, string fileName, BinaryReader reader)
    {
        MemoryStream stream = new MemoryStream();
        DecompressImage(reader.ReadBytes(Length), stream);
        Data = stream.ToArray();
        stream.Close();

        SaveTexture(outParentDir + fileName, Data);
        if (HasMask)
        {
            reader.ReadBytes(12);

            stream = new MemoryStream();
            DecompressImage(reader.ReadBytes(Length), stream);
            MaskData = stream.ToArray();
            stream.Close();
        }

        SaveTexture(outParentDir + "Mask" + fileName, MaskData);
    }

    private void SaveTexture(string outPath, byte[] Data)
    {
        File.WriteAllBytes(outPath, Data);
    }

    private void DecompressImage(byte[] data, Stream destination)
    {
        using (var stream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
        {
            stream.CopyTo(destination);
        }
    }
}
