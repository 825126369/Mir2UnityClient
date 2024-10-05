using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public enum LangOpt
{
    zh,//中文
    tw,//中文
    en,//英文
    pt,//巴葡
    ja,//日语
    it,//意大利
    ru,//俄语
    de,//德语
    fr,//法语
    es,//西班牙语
    ind,//印尼
    pl,//波兰
    tr,//土耳其
    hin,//印地语
    ph,//菲律宾
}

public class i18nController : SingleTonMonoBehaviour<i18nController>
{
    public LangOpt default_lang = LangOpt.en;
    Dictionary<string, Dictionary<string, string>> mConfigDic = null;
    public void Init()
    {
        this.mConfigDic = GetConfigDic();

        PrintTool.LogWithColor("RegionInfo.CurrentRegion.Name: ", RegionInfo.CurrentRegion.Name);
        PrintTool.LogWithColor("CultureInfo localLanguage: ", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
        PrintTool.LogWithColor("Application.systemLanguage: ", Application.systemLanguage);
    }

    public void DoReBuildConfig()
    {
        this.mConfigDic = null;
        GetConfigDic();
    }
    
    public Dictionary<string, Dictionary<string, string>> GetConfigDic()
    {
        if(mConfigDic == null)
        {
            if(Application.isPlaying)
            {
                string data = ResCenter.readOnlyInstance.mBundleGameAllRes.FindTextAsset("I18n").text; 
                mConfigDic = JsonTool.FromJson<Dictionary<string, Dictionary<string, string>>>(data);
                PrintTool.Assert(mConfigDic != null, "mConfigDic == null");
            }
            else
            {
#if UNITY_EDITOR
                string data = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/GameAssets/game/Configs/I18n.json").text;
                mConfigDic = JsonTool.FromJson<Dictionary<string, Dictionary<string, string>>>(data);
                PrintTool.Assert(mConfigDic != null, "mConfigDic == null");
#endif
            }
        }
        return mConfigDic;
    }

    public string GetLang()
    {
        LangOpt lang = LangOpt.en;
#if UNITY_EDITOR
        lang = default_lang;
#else
        if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            //中文
            lang = LangOpt.zh;
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            //中文
            lang = LangOpt.tw;
        }
        else if (Application.systemLanguage == SystemLanguage.English)
        {
            //英文
            lang = LangOpt.en;
        }
        else if (Application.systemLanguage == SystemLanguage.Portuguese)
        {
            //巴葡
            lang = LangOpt.pt;
        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            //日语
            lang = LangOpt.ja;
        }
        else if (Application.systemLanguage == SystemLanguage.Italian)
        {
            //意大利
            lang = LangOpt.it;
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            //俄语
            lang = LangOpt.ru;
        }
        else if (Application.systemLanguage == SystemLanguage.German)
        {
            //德语
            lang = LangOpt.de;
        }
        else if (Application.systemLanguage == SystemLanguage.French)
        {
            //法语
            lang = LangOpt.fr;
        }
        else if (Application.systemLanguage == SystemLanguage.Spanish)
        {
            //西班牙语
            lang = LangOpt.es;
        }
        else if (Application.systemLanguage == SystemLanguage.Indonesian)
        {
            //印尼
            lang = LangOpt.ind;
        }
        else if (Application.systemLanguage == SystemLanguage.Polish)
        {
            //波兰
            lang = LangOpt.pl;
        }
        else if (Application.systemLanguage == SystemLanguage.Turkish)
        {
            //土耳其
            lang = LangOpt.tr;
        }
        else if (Application.systemLanguage == SystemLanguage.Hindi)
        {
            //印地语
            lang = LangOpt.hin;
        }
        else
        {
            string localLanguage = System.Globalization.CultureInfo.CurrentUICulture.Name;
            if (localLanguage == "fil-PH")
            {
                //菲律宾
                lang = LangOpt.ph;
            }
        }
#endif
        return lang.ToString();
    }

    public string getKey(string key)
    {
        if(Application.isPlaying)
        {
            if(mConfigDic == null)
            {
                return string.Empty;
            }
        }

        string lang = GetLang();
        mConfigDic = this.GetConfigDic();
        if(mConfigDic != null)
        {
            if(mConfigDic.ContainsKey(lang))
            {
                if (mConfigDic[lang].ContainsKey(key))
                {
                    return mConfigDic[lang][key];
                }
                else
                {
                    PrintTool.LogError("key 不存在：", key);
                }
            }
            else
            {
                PrintTool.LogError("不存在的语言：", lang);
            }
        }
        else
        {
            PrintTool.LogError("mConfigDic == null");
        }
        return "?";
    }
}

