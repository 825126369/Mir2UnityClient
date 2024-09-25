using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class VersionUpdateTimeCheckConfig
{
    public Dictionary<string, ulong> mUpdateTimeDic = new Dictionary<string, ulong>();
}

public class VersionUpdateTimeCheck:SingleTonMonoBehaviour<VersionUpdateTimeCheck>
{
    private VersionUpdateTimeCheckConfig mLocalConfig = null;
    private VersionUpdateTimeCheckConfig mWWWConfig = null;
    
    private void InitLocalConfig()
    {
        TextAsset mAssets = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(GameConst.versionUpdateTimeCheckFileName));
        mLocalConfig = JsonTool.FromJson<VersionUpdateTimeCheckConfig>(mAssets.text);
        Debug.Assert(mLocalConfig != null, "mLocalConfig == null");
    }

    public void Do()
    {
        StartCoroutine(CheckRemoteConfig1());
    }

    private IEnumerator CheckRemoteConfig1()
    {
        InitLocalConfig();

        string url = GameConst.GetVersionUpdateTimeCheckUrl();
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            string netErrorDes = $"{GameConst.versionUpdateTimeCheckFileName} WWW Error:" + www.responseCode + " | " + url + " | " + www.error;
            Debug.LogError(netErrorDes);
            www.Dispose();
            yield break;
        }

        string jsonStr = www.downloadHandler.text;
        www.Dispose();
        mWWWConfig = JsonTool.FromJson<VersionUpdateTimeCheckConfig>(jsonStr);
        CheckUpdateOldVersion();
    }

    // 检查是否拉取的WWW内容是 老内容，有可能Google的云存储 有延迟，导致测试某个Bug一直卡住
    private void CheckUpdateOldVersion()
    {
        ulong mLocalUpdateTime = 0;
        if (mLocalConfig.mUpdateTimeDic.ContainsKey(Application.version))
        {
            mLocalUpdateTime = mLocalConfig.mUpdateTimeDic[Application.version];
        }

        ulong mWebUpdateTime = 0;
        if (mWWWConfig.mUpdateTimeDic.ContainsKey(Application.version))
        {
            mWebUpdateTime = mWWWConfig.mUpdateTimeDic[Application.version];
        }

        if (mLocalUpdateTime > mWebUpdateTime)
        {
            Debug.LogError("This WWW Load outdated content from Google Cloud: " + TimeTool.GetLocalTimeFromTimeStamp(mLocalUpdateTime) + " | " + TimeTool.GetLocalTimeFromTimeStamp(mWebUpdateTime));
        }
    }
}
