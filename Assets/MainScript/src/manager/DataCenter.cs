using Mir2;
using NetProtocols.SelectGate;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using S = NetProto.SCPacket;

public class DataCenter:SingleTonMonoBehaviour<DataCenter>
{
    // 当前存档时间
    public bool bMute = false;
    public int nWebTag = 0;
    public bool bReview = false;
    public int nThemeIndex = 1;
    public int nCoinCount = 0;
    public int nLevel;
    public bool bShowResurrectionUI = false;

    //登录
    public readonly Regex mAccountRegex = new Regex(@"^[A-Za-z0-9]{8,20}$");
    public readonly Regex mPasswordRegex = new Regex(@"^[A-Za-z0-9]{8,20}$");
    public readonly Regex mMailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

    public const string LoginServerConnectStr = "127.0.0.1:8010";
    public string selectGateServerConnectStr = string.Empty;
    public string GateServerConnectStr = string.Empty;
    public uint nAccountId = 0;
    //选择服务器
    public const string WarriorDescription = "战士是一个充满力量和活力的阶级。他们在战斗中不容易被杀死，并且具有能够使用的优势”+\r\n“各种重型武器和盔甲。因此，战士们喜欢基于近战物理伤害的攻击。他们在射程上很弱”+\r\n“然而，专门为勇士队开发的各种装备弥补了他们在远程作战中的弱点。";
    public const string WizardDescription = "法师属于体力和耐力较低的一类，但他们有能力使用强大的法术。他们的进攻法术非常有效，但+\r\n“因为施展这些法术需要时间，他们很可能会让自己暴露在敌人的攻击之下。因此，身体虚弱的巫师”+\r\n“必须以从安全距离攻击敌人为目标。";
    public const string TaoistDescription = "除了穆公，道教在天文、医学等方面都有很好的纪律。他们的专长不是直接与敌人交战，而是协助盟友提供支持。道教可以召唤强大的生物，对魔法有很高的抵抗力，是一个攻防能力平衡的阶层。";
    public const string AssassinDescription = "刺客是一个秘密组织的成员，他们的历史相对不为人知。他们能够隐藏自己并进行攻击”+\r\n在别人看不见的情况下，这自然使他们擅长快速杀戮。他们有必要避免与之交战+\r\n“由于生命力和力量薄弱，敌人众多。";
    public const string ArcherDescription = "弓箭手是一个具有极高精准度和力量的职业，他们利用弓的强大技能在远距离造成非凡的伤害。就像+\r\n“巫师，他们依靠敏锐的直觉躲避迎面而来的攻击，因为他们倾向于让自己暴露在正面攻击之下。然而，他们的”+\r\n“身体力量和致命的目标使他们能够向任何被击中的人灌输恐惧。";

    public readonly List<ServerItemData> mServerItemDataList = new List<ServerItemData>();
    public ServerItemData currentSelectServerItemData = null;
    //主游戏
    public const int CellWidth = 48;
    public const int CellHeight = 32;
    public readonly List<PlayerData> mPlayerDataList = new List<PlayerData>();
    public readonly UserData UserData = new UserData();
    public readonly MapData MapData = new MapData();

    public void Init()
    {
       
    }

    public void OnNetSyncServerItemList(packet_sc_ServerList_Result mReceiveMsg)
    {
        foreach (var item in mReceiveMsg.MServerInfoList)
        {
            ServerItemData itemData = mServerItemDataList.Find((x) => x.nServerId == item.NServerId);
            if (itemData == null)
            {
                itemData = new ServerItemData();
                mServerItemDataList.Add(itemData);
            }
            itemData.CopyFrom(item);
        }

        List<ServerItemData> mRemoveList = new List<ServerItemData>();
        foreach (var item in mServerItemDataList)
        {
            packet_SelectGateServerToPlayer_Data itemData = mReceiveMsg.MServerInfoList.First((x)=>x.NServerId == item.nServerId);
            if (itemData == null)
            {
                mRemoveList.Add(item);
            }
        }

        foreach(var item in mRemoveList)
        {
            mServerItemDataList.Remove(item);
        }
    }

    public void InitStartGameData(S.packet_sc_StartGame mReceiveMsg)
    {
        //UserData.CopyFrom(mReceiveMsg.);
        //UserData.RefreshEquipmentStats();

        //uint nMapInfoIndex = mReceiveMsg.UserInfo.NMapIndex;
        //var mMapInfo = ExcelTableMgr.Instance.MapInfoList.Find((x) => x.Index == nMapInfoIndex);
        //if (mMapInfo != null)
        //{
        //    string path = $"D:\\Me\\MyProject\\Mir2Server\\Mir2Config\\Maps\\{mMapInfo.FileName}.map";
        //    MapData.mMapInfo = mMapInfo;
        //    MapData.mMapBasicInfo = new MapReader(path);
        //}
    }

}
