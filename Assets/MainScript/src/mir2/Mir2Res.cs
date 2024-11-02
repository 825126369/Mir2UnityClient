using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class Mir2Res:SingleTonMonoBehaviour<Mir2Res>
{
    public readonly Dictionary<string, Sprite> mSpriteDic = new Dictionary<string, Sprite>();
    public static readonly string[] MapLibs = new string[400];
    public static readonly string DataPath = "D:/Me/MyProject/CrystalMir2/Client8/Data/";
    public Mir2Res()
    {
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

    public void SetMapSprite(SpriteRenderer mSpriteRenderer, int nIndex, int nIndex2)
    {
        StartCoroutine(SetMapSprite2(mSpriteRenderer, nIndex, nIndex2));
    }

    private IEnumerator SetMapSprite2(SpriteRenderer mSpriteRenderer, int nIndex, int nIndex2)
    {
        string path = Path.Combine(MapLibs[nIndex], nIndex2 + ".png");
        path = Path.GetFullPath(path);
        string url = GetPathUrl(path);

        Sprite mTargetSprite = null;
        if (!mSpriteDic.TryGetValue(url, out mTargetSprite))
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var mTexture = DownloadHandlerTexture.GetContent(www);
                mTargetSprite = Sprite.Create(mTexture, new Rect(0, 0, mTexture.width, mTexture.height), Vector2.zero, 1);
                mSpriteDic.Add(url, mTargetSprite);
            }
            else
            {
                Debug.LogError(www.result + " | " + www.error + " | " + url);
            }
        }
        mSpriteRenderer.sprite = mTargetSprite;
    }

    public void SetMapSprite(RuleTile mSpriteRenderer, int nIndex, int nIndex2)
    {
        StartCoroutine(SetMapSprite2(mSpriteRenderer, nIndex, nIndex2));
    }

    public IEnumerator SetMapSprite2(RuleTile mSpriteRenderer, int nIndex, int nIndex2)
    {
        string path = Path.Combine(MapLibs[nIndex], nIndex2 + ".png");
        path = Path.GetFullPath(path);
        string url = GetPathUrl(path);

        Sprite mTargetSprite = null;
        if (!mSpriteDic.TryGetValue(url, out mTargetSprite))
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var mTexture = DownloadHandlerTexture.GetContent(www);
                mTargetSprite = Sprite.Create(mTexture, new Rect(0, 0, mTexture.width, mTexture.height), Vector2.zero, 1);
                mSpriteDic.Add(url, mTargetSprite);
            }
            else
            {
                Debug.LogError(www.result + " | " + www.error + " | " + url);
            }
        }
        mSpriteRenderer.m_DefaultSprite = mTargetSprite;
    }

    public string GetPathUrl(string filePath)
    {
        Uri fileUri = new Uri(filePath);
        return fileUri.AbsoluteUri;
    }
}
