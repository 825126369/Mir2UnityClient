using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

namespace Mir2
{
    public class Mir2Res : SingleTonMonoBehaviour<Mir2Res>
    {
        public readonly Dictionary<string, Sprite> mSpriteDic = new Dictionary<string, Sprite>();
        public static readonly string[] MapLibs = new string[400];
        public static readonly string DataPath = "D:/Me/MyProject/CrystalMir2/Client8/Data/";

        public static string[] CArmours,
                                  CWeapons,
                                  CWeaponEffect,
                                  CHair,
                                  CHumEffect,
                                  AArmours,
                                  AWeaponsL,
                                  AWeaponsR,
                                  AHair,
                                  AHumEffect,
                                  ARArmours,
                                  ARWeapons,
                                  ARWeaponsS,
                                  ARHair,
                                  ARHumEffect,
                                  Monsters,
                                  Gates,
                                  Flags,
                                  Siege,
                                  Mounts,
                                  NPCs,
                                  Fishing,
                                  Pets,
                                  Transform,
                                  TransformMounts,
                                  TransformEffect,
                                  TransformWeaponEffect;


        public string MapPath,
                    SoundPath,
                    ExtraDataPath = @".\Data\Extra\",
                    ShadersPath = @".\Data\Shaders\",
                    MonsterPath = @".\Data\Monster\",
                    GatePath = @".\Data\Gate\",
                    FlagPath = @".\Data\Flag\",
                    SiegePath = @".\Data\Siege\",
                    NPCPath = @".\Data\NPC\",
                    CArmourPath = @".\Data\CArmour\",
                    CWeaponPath = @".\Data\CWeapon\",
                    CWeaponEffectPath = @".\Data\CWeaponEffect\",
                    CHairPath = @".\Data\CHair\",
                    AArmourPath = @".\Data\AArmour\",
                    AWeaponPath = @".\Data\AWeapon\",
                    AHairPath = @".\Data\AHair\",
                    ARArmourPath = @".\Data\ARArmour\",
                    ARWeaponPath = @".\Data\ARWeapon\",
                    ARHairPath = @".\Data\ARHair\",
                    CHumEffectPath = @".\Data\CHumEffect\",
                    AHumEffectPath = @".\Data\AHumEffect\",
                    ARHumEffectPath = @".\Data\ARHumEffect\",
                    MountPath = @".\Data\Mount\",
                    FishingPath = @".\Data\Fishing\",
                    PetsPath = @".\Data\Pet\",
                    TransformPath = @".\Data\Transform\",
                    TransformMountsPath = @".\Data\TransformRide2\",
                    TransformEffectPath = @".\Data\TransformEffect\",
                    TransformWeaponEffectPath = @".\Data\TransformWeaponEffect\",
                    MouseCursorPath = @".\Data\Cursors\";

        public Mir2Res()
        {
            MapPath = Path.Combine(DataPath, "Map");
            SoundPath = Path.Combine(DataPath, "Sound");
            ExtraDataPath = Path.Combine(DataPath, "Extra");
            ShadersPath = Path.Combine(DataPath, "Shaders");
            MonsterPath = Path.Combine(DataPath, "Monster");
            GatePath = Path.Combine(DataPath, "Gate");
            FlagPath = Path.Combine(DataPath, "Flag");
            SiegePath = Path.Combine(DataPath, "Siege");
            NPCPath = Path.Combine(DataPath, "NPC");
            CArmourPath = Path.Combine(DataPath, "CArmour");
            CWeaponPath = Path.Combine(DataPath, "CWeapon");
            CWeaponEffectPath = Path.Combine(DataPath, "CWeaponEffect");
            CHairPath = Path.Combine(DataPath, "CHair");
            AArmourPath = Path.Combine(DataPath, "AArmour");
            AWeaponPath = Path.Combine(DataPath, "AWeapon");
            AHairPath = Path.Combine(DataPath, "AHair");
            ARArmourPath = Path.Combine(DataPath, "ARArmour");
            ARWeaponPath = Path.Combine(DataPath, "ARWeapon");
            ARHairPath = Path.Combine(DataPath, "ARHair");
            CHumEffectPath = Path.Combine(DataPath, "CHumEffect");
            AHumEffectPath = Path.Combine(DataPath, "AHumEffect");
            ARHumEffectPath = Path.Combine(DataPath, "ARHumEffect");
            MountPath = Path.Combine(DataPath, "Mount");
            FishingPath = Path.Combine(DataPath, "Fishing");
            PetsPath = Path.Combine(DataPath, "Pet");
            TransformPath = Path.Combine(DataPath, "Transform");
            TransformMountsPath = Path.Combine(DataPath, "TransformRide2");
            TransformEffectPath = Path.Combine(DataPath, "TransformEffect");
            TransformWeaponEffectPath = Path.Combine(DataPath, "TransformWeaponEffect");
            MouseCursorPath = Path.Combine(DataPath, "Cursors");


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
            MapLibs[0] = (DataPath + "Map\\WemadeMir2\\Tiles");
            MapLibs[1] = (DataPath + "Map\\WemadeMir2\\Smtiles");
            MapLibs[2] = (DataPath + "Map\\WemadeMir2\\Objects");
            for (int i = 2; i < 28; i++)
            {
                MapLibs[i + 1] = (DataPath + "Map\\WemadeMir2\\Objects" + i.ToString());
            }
            MapLibs[90] = (DataPath + "Map\\WemadeMir2\\Objects_32bit");

            //shanda mir2 (allowed from 100-199)
            MapLibs[100] = (DataPath + "Map\\ShandaMir2\\Tiles");
            for (int i = 1; i < 10; i++)
            {
                MapLibs[100 + i] = (DataPath + "Map\\ShandaMir2\\Tiles" + (i + 1));
            }
            MapLibs[110] = (DataPath + "Map\\ShandaMir2\\SmTiles");
            for (int i = 1; i < 10; i++)
            {
                MapLibs[110 + i] = (DataPath + "Map\\ShandaMir2\\SmTiles" + (i + 1));
            }
            MapLibs[120] = (DataPath + "Map\\ShandaMir2\\Objects");
            for (int i = 1; i < 31; i++)
            {
                MapLibs[120 + i] = (DataPath + "Map\\ShandaMir2\\Objects" + (i + 1));
            }
            MapLibs[190] = (DataPath + "Map\\ShandaMir2\\AniTiles1");
            //wemade mir3 (allowed from 200-299)
            string[] Mapstate = { "", "wood\\", "sand\\", "snow\\", "forest\\" };
            for (int i = 0; i < Mapstate.Length; i++)
            {
                MapLibs[200 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Tilesc");
                MapLibs[201 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Tiles30c");
                MapLibs[202 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Tiles5c");
                MapLibs[203 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Smtilesc");
                MapLibs[204 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Housesc");
                MapLibs[205 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Cliffsc");
                MapLibs[206 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Dungeonsc");
                MapLibs[207 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Innersc");
                MapLibs[208 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Furnituresc");
                MapLibs[209 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Wallsc");
                MapLibs[210 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "smObjectsc");
                MapLibs[211 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Animationsc");
                MapLibs[212 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Object1c");
                MapLibs[213 + (i * 15)] = (DataPath + "Map\\WemadeMir3\\" + Mapstate[i] + "Object2c");
            }
            Mapstate = new string[] { "", "wood", "sand", "snow", "forest" };
            //shanda mir3 (allowed from 300-399)
            for (int i = 0; i < Mapstate.Length; i++)
            {
                MapLibs[300 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Tilesc" + Mapstate[i]);
                MapLibs[301 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Tiles30c" + Mapstate[i]);
                MapLibs[302 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Tiles5c" + Mapstate[i]);
                MapLibs[303 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Smtilesc" + Mapstate[i]);
                MapLibs[304 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Housesc" + Mapstate[i]);
                MapLibs[305 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Cliffsc" + Mapstate[i]);
                MapLibs[306 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Dungeonsc" + Mapstate[i]);
                MapLibs[307 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Innersc" + Mapstate[i]);
                MapLibs[308 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Furnituresc" + Mapstate[i]);
                MapLibs[309 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Wallsc" + Mapstate[i]);
                MapLibs[310 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "smObjectsc" + Mapstate[i]);
                MapLibs[311 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Animationsc" + Mapstate[i]);
                MapLibs[312 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Object1c" + Mapstate[i]);
                MapLibs[313 + (i * 15)] = (DataPath + "Map\\ShandaMir3\\" + "Object2c" + Mapstate[i]);
            }
            #endregion
        }

        static void InitLibrary(ref string[] library, string path, string toStringValue, string suffix = "")
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var allFiles = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            library = allFiles.ToArray();
        }

        public void SetMapSprite(SpriteRenderer mSpriteRenderer, int nIndex, int nIndex2)
        {
            StartCoroutine(SetMapSprite2(mSpriteRenderer, nIndex, nIndex2));
        }

        private IEnumerator SetMapSprite2(SpriteRenderer mSpriteRenderer, int nIndex, int nIndex2)
        {
            string path = Path.Combine(MapLibs[nIndex], nIndex2 + ".png");
            path = Path.GetFullPath(path);
            string url = GetPathUrl(path);

            yield return SetWebSprite(url, (mTargetSprite) =>
            {
                mSpriteRenderer.sprite = mTargetSprite;
            });
        }

        public void SetMapSprite(Tile mSpriteRenderer, int nIndex, int nIndex2)
        {
            StartCoroutine(SetMapSprite2(mSpriteRenderer, nIndex, nIndex2));
        }

        public IEnumerator SetMapSprite2(Tile mSpriteRenderer, int nIndex, int nIndex2)
        {
            string path = Path.Combine(MapLibs[nIndex], nIndex2 + ".png");
            path = Path.GetFullPath(path);
            string url = GetPathUrl(path);

            yield return SetWebSprite(url, (mTargetSprite) =>
            {
                mSpriteRenderer.sprite = mTargetSprite;
            });
        }

        public IEnumerator RequestMapSprite(int nIndex, int nIndex2)
        {
            string path = Path.Combine(MapLibs[nIndex], nIndex2 + ".png");
            path = Path.GetFullPath(path);
            string url = GetPathUrl(path);
            yield return SetWebSprite(url);
        }

        public Sprite GetMapSprite(int nIndex, int nIndex2)
        {
            string path = Path.Combine(MapLibs[nIndex], nIndex2 + ".png");
            path = Path.GetFullPath(path);
            string url = GetPathUrl(path);

            if (mSpriteDic.ContainsKey(url))
            {
                return mSpriteDic[url];
            }
            return null;
        }
        
        public void SetSprite(string path, Action<Sprite> mFinishEvent = null)
        {
            //Debug.Log(path);
            path = Path.GetFullPath(path);
            string url = GetPathUrl(path);
            StartCoroutine(SetWebSprite(url, mFinishEvent));
        }

        public IEnumerator SetWebSprite(string url, Action<Sprite> mFinishEvent = null)
        {
            Sprite mTargetSprite = null;
            if (!mSpriteDic.TryGetValue(url, out mTargetSprite))
            {
                var www = UnityWebRequestTexture.GetTexture(url);
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    var mTexture = DownloadHandlerTexture.GetContent(www);
                    mTargetSprite = Sprite.Create(mTexture, new Rect(0, 0, mTexture.width, mTexture.height), Vector2.one * 0.5f, 1);
                    mSpriteDic[url] = mTargetSprite;
                }
                else
                {
                    Debug.LogError(www.result + " | " + www.error + " | " + url);
                }
                www.Dispose();
            }
            mFinishEvent?.Invoke(mTargetSprite);
        }

        public string GetPathUrl(string filePath)
        {
            Uri fileUri = new Uri(filePath);
            return fileUri.AbsoluteUri;
        }
    }
}
