using AKNet.Common;
using Mir2;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameLauncher : SingleTonMonoBehaviour<GameLauncher>
{
    public bool m_PrintLog = true;
    public UIRoot mUIRoot;
    
    protected override void Awake()
    {
        base.Awake();
        gameObject.removeAllChildren();
        AddNetLog();
    }
    
    private void Start()
    {      
        Debug.unityLogger.logEnabled = m_PrintLog;
        if (m_PrintLog == false)
        {
            PrintTool.m_PrintToolLog = false;
        }

        Application.targetFrameRate = 60;
        Application.runInBackground = !GameConst.isMobilePlatform();

        PrintPlatformInfo();
        DontDestroyOnLoad(this);
        LeanTween.init(9000, 9000);
        StartCoroutine(AsyncInit());
    }

    private void AddNetLog()
    {
        Action<string> LogFunc = (string message) =>
        {
            PrintTool.Log(message);
        };

        Action<string> LogErrorFunc = (string message) =>
        {
            PrintTool.LogError(message);
        };

        Action<string> LogWarningFunc = (string message) =>
        {
            PrintTool.LogWarning(message);
        };

        NetLog.AddLogFunc(LogFunc, LogErrorFunc, LogWarningFunc);
    }

    private IEnumerator AsyncInit()
    {
        if(GameConst.orUseAssetBundle())
        {
            yield return Addressables.InitializeAsync();
        }

        GameObject goPrefab = Resources.Load("UIRoot") as GameObject;
        GameObject goUIRoot = Instantiate<GameObject>(goPrefab);
        goUIRoot.SetActive(true);
        mUIRoot = goUIRoot.GetComponent<UIRoot>();

        yield return AssetsLoader.Instance.AsyncLoadManyAssetsByLabel("InitScene", ()=>
        {
            InitScene.Instance.Init();
        });
    }

    private void PrintPlatformInfo()
    {
        Debug.Log("orUseAssetBundle: " + GameConst.orUseAssetBundle());
    }
    
    public void OnHotUpdateFinish()
    {
        StartCoroutine(StartEnterGame());
    }

    public IEnumerator StartEnterGame()
    {
        yield return ResCenter.Instance.AsyncInit();
        MLibrarys.InitLibraries();
        yield return null;

        ExcelTableMgr.Instance.Init();
        DataCenter.Instance.Init();
        UIMgr.Instance.Init();

        NetClientLoginMgr.Instance.InitLoginServerClient();
        while (NetClientLoginMgr.mNetClient.GetSocketState() == SOCKET_PEER_STATE.CONNECTING)
        {
            yield return null;
        }

        if(NetClientLoginMgr.mNetClient.GetSocketState() != SOCKET_PEER_STATE.CONNECTED)
        {
            UIMgr.CommonDialogView.ShowOk("提示", "连接服务器失败！！！");
            yield break;
        }

        UIMgr.Instance.Show_LoginView();
        yield return WaitToDestroyInitScene();
    }

    private void LoadLobby()
    {
        var goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("lobby") as GameObject;
        var go = Instantiate<GameObject>(goPrefab) as GameObject;
        go.transform.SetParent(mUIRoot.mCanvas_Game, false);
        go.SetActive(true);
    }

    private IEnumerator WaitToDestroyInitScene()
    {
        while (!InitScene.readOnlyInstance.bProgressFull)
        {
            yield return null;
        }
        Destroy(InitScene.readOnlyInstance.mInitSceneView.gameObject);
        Destroy(InitScene.readOnlyInstance.gameObject);
    }

    private void YourTest()
    {
        
    }

}
