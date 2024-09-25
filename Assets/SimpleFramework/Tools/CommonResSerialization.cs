using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CommonResSerialization : MonoBehaviour
{
    public List<GameObject> m_PrefabList = new List<GameObject>();
    public List<SpriteAtlas> m_AtlasList = new List<SpriteAtlas>();
    public List<Sprite> m_SpriteList = new List<Sprite>();
    public List<Texture> m_TextureList = new List<Texture>();
    public List<AudioClip> m_AudoClipList = new List<AudioClip>();
    public List<Shader> m_ShaderList = new List<Shader>();
    public List<Material> m_MaterialList = new List<Material>();
    public List<TextAsset> m_TextAssetList = new List<TextAsset>();

    public TextAsset FindTextAsset(string name)
    {
        return m_TextAssetList.Find((x) => x != null && x.name == name);
    }

    public GameObject FindPrefab(string name)
    {
        return m_PrefabList.Find((x) => x != null && x.name == name);
    }

    public GameObject FindPrefabByPrefixName(string name)
    {
        return m_PrefabList.Find((x) => x != null && x.name.StartsWith(name));
    }

    public Sprite FindSprite(string name)
    {
        return m_SpriteList.Find((x) => x != null && x.name == name);
    }

    public Texture FindTexture(string name)
    {
        return m_TextureList.Find((x) => x != null && x.name == name);
    }

    public AudioClip FindAudioClip(string name)
    {
        return m_AudoClipList.Find((x) => x != null && x.name == name);
    }

    public Shader FindShader(string name)
    {
        return m_ShaderList.Find((x) => x != null && x.name == name);
    }

    public Material FindMaterial(string name)
    {
        return m_MaterialList.Find((x) => x != null && x.name == name);
    }

    public SpriteAtlas GetAtlas(string atlasName)
    {
        return m_AtlasList.Find((x) => x != null && x.name == atlasName);
    }

    public Sprite GetSpriteByAtlas(string atlasName, string spriteName)
    {
        return GetAtlas(atlasName).GetSprite(spriteName);
    }

    //-----------------------------------Editor 相关-----------------------------------------

    [HideInInspector] public string mResFolder = string.Empty;
}
