using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.U2D;

public class TextureToSpriteEditor: AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (assetPath.EndsWith(".png") || assetPath.EndsWith(".jpg"))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            DoTextureToSprite(assetPath, textureImporter);
        }
    }

    [MenuItem("Assets/Texture To Sprite")]
    static void Do()
    {
        foreach (var obj in Selection.objects)
        {
            string selectionPath = AssetDatabase.GetAssetPath(obj);
            Debug.Log("Selection： " + obj.GetType().Name + " | " + selectionPath);
            if (Directory.Exists(selectionPath))
            {
                string resDirPath = selectionPath;
                DoFolder(resDirPath);
            }
            else
            {
                DoTextureToSprite(AssetDatabase.GetAssetPath(obj));
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    public static void DoFolder(string dirPath)
    {
        string[] dirs = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
        foreach (var v1 in dirs)
        {
            string Extension = Path.GetExtension(v1);
            if ((Extension == ".png" || Extension == ".jpg"))
            {
                DoTextureToSprite(v1);
            }
        }
    }

    private static void DoTextureToSprite(string assetPath)
    {
        TextureImporter ti = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        DoTextureToSprite(assetPath, ti);
    }

    private static void DoTextureToSprite(string assetPath, TextureImporter ti)
    {
        if (ti == null) return;

        bool needRepair = false;
        if (ti.textureType != TextureImporterType.Sprite)
        {
            needRepair = true;
        }
        else if (ti.mipmapEnabled)
        {
            needRepair = true;
        }

        if (needRepair)
        {
            ti.textureType = TextureImporterType.Sprite;
            ti.mipmapEnabled = false;
            ti.SaveAndReimport();
            Debug.Log("Texture To Sprite: " + assetPath);
        }
    }
}
