using Net.TCP.Client;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameLauncher : SingleTonMonoBehaviour<GameLauncher>
{
    public bool m_PrintLog = true;
    public bool m_PrintToolLog = true;

    public UIRoot mUIRoot;
    
    protected override void Awake()
    {
        base.Awake();
        gameObject.removeAllChildren();
    }
    
    private void Start()
    {      
        Debug.unityLogger.logEnabled = m_PrintLog;
        if (m_PrintLog == false)
        {
            m_PrintToolLog = false;
        }

        Application.targetFrameRate = 60;
        Application.runInBackground = !GameConst.isMobilePlatform();

        PrintPlatformInfo();
        DontDestroyOnLoad(this);
        LeanTween.init(9000, 9000);
        StartCoroutine(AsyncInit());
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

        DataCenter.Instance.Init();
        UIMgr.Instance.Init();
        NetClientMgr.Instance.InitLoginServerClient();

        while (NetClientMgr.Instance.LoginServer_NetClient.GetSocketState() == SOCKETPEERSTATE.CONNECTING)
        {
            yield return null;
        }

        SceneMgr.Instance.LoadSceneAsync(SceneNames.Login);
    }

    private void LoadLobby()
    {
        var goPrefab = ResCenter.Instance.mBundleGameAllRes.FindPrefab("lobby") as GameObject;
        var go = Instantiate<GameObject>(goPrefab) as GameObject;
        go.transform.SetParent(mUIRoot.mCanvas_Game, false);
        go.SetActive(true);
    }

    private void YourTest()
    {
        
    }

}
