using XKNet.Common;
namespace NetProtocols.Login
{
	public sealed partial class packet_sc_Login_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			GateServerIp = string.Empty;
			BanReason = string.Empty;
			ExpiryDate = 0;
		}
	}
}
namespace NetProtocols.Login
{
	public sealed partial class packet_cs_Login : IProtobufResetInterface
	{
		public void Reset()
		{
			Account = string.Empty;
			Password = string.Empty;
			NLoginType = 0;
		}
	}
}
namespace NetProtocols.Login
{
	public sealed partial class packet_cs_Register : IProtobufResetInterface
	{
		public void Reset()
		{
			Account = string.Empty;
			Password = string.Empty;
			NLoginType = 0;
			SecretQuestion = string.Empty;
			SecretAnswer = string.Empty;
			EMailAddress = string.Empty;
		}
	}
}
namespace NetProtocols.Login
{
	public sealed partial class packet_sc_Register_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Login
{
	public sealed partial class packet_cs_ChangePassword : IProtobufResetInterface
	{
		public void Reset()
		{
			Account = string.Empty;
			CurrentPassword = string.Empty;
			NewPassword = string.Empty;
		}
	}
}
namespace NetProtocols.Login
{
	public sealed partial class packet_sc_ChangePassword_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_GateServerToSelectGateServer_Data : IProtobufResetInterface
	{
		public void Reset()
		{
			NServerId = 0;
			ServerConnectStr = string.Empty;
			OnlinePlayerCount = 0;
			CreatedPlayerCount = 0;
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_SelectGateServerToGateServer_Data : IProtobufResetInterface
	{
		public void Reset()
		{
			NServerId = 0;
			DataBaseConnectStr = string.Empty;
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_SelectGateServerToPlayer_Data : IProtobufResetInterface
	{
		public void Reset()
		{
			NServerId = 0;
			ServerConnectStr = string.Empty;
			ServerName = string.Empty;
			NState = 0;
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_gsg_SendServerInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			MServerInfo.Reset();
			MServerInfo = null;
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_sgg_SendServerInfo_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			MServerInfo.Reset();
			MServerInfo = null;
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_cs_request_ServerList : IProtobufResetInterface
	{
		public void Reset()
		{
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_sc_ServerList_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			foreach(var v in MServerInfoList)
			{
				IMessagePool<NetProtocols.SelectGate.packet_SelectGateServerToPlayer_Data>.recycle(v);
			}
			MServerInfoList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_RoomEntryInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NRoomId = 0;
			NCreateTime = 0;
			NRoomState = 0;
			BRobotCreate = false;
			StrRoomName = string.Empty;
			StrPassword = string.Empty;
			NMinBetCount = 0;
			NRoomFightCount = 0;
			NRoomFightMaxCount = 0;
			NRoomSpectatorCount = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_RoomEntryListInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NPageIndex = 0;
			NMaxPage = 0;
			foreach(var v in MRoomEntryInfoList)
			{
				IMessagePool<NetProtocols.Game.packet_data_RoomEntryInfo>.recycle(v);
			}
			MRoomEntryInfoList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_playerInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NUserId = 0;
			NLoginType = 0;
			UniqueIdentifier = string.Empty;
			StrName = string.Empty;
			NGoldCount = 0;
			NLevel = 0;
			NLevelExp = 0;
			NVipLevel = 0;
			NHeadIconId = 0;
			NWinCount = 0;
			NLoseCount = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_deskInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
			NUserId = 0;
			BPrepare = false;
			BHang = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_deskcardInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
			CardList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_Basic_RoomInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			StrRoomName = string.Empty;
			StrPassword = string.Empty;
			NMaxDeskCount = 0;
			NMinBetCount = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_Detail_RoomInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NRoomId = 0;
			NRoomState = 0;
			NRoomCreatorId = 0;
			NMyDeskId = 0;
			MRoomBasicInfo.Reset();
			MRoomBasicInfo = null;
			foreach(var v in PlayerInfoList)
			{
				IMessagePool<NetProtocols.Game.packet_data_playerInfo>.recycle(v);
			}
			PlayerInfoList.Clear();
			foreach(var v in DeskInfoList)
			{
				IMessagePool<NetProtocols.Game.packet_data_deskInfo>.recycle(v);
			}
			DeskInfoList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_GameStartInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NOpenCard = 0;
			NWhoGetOpenCardDeskId = 0;
			NWhoFirstGetPokerDeskId = 0;
			NLeftPokerCount = 0;
			foreach(var v in DeskCardList)
			{
				IMessagePool<NetProtocols.Game.packet_data_deskcardInfo>.recycle(v);
			}
			DeskCardList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_RecoverSceneInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NPlayerState = 0;
			NRoomState = 0;
			NGameState = 0;
			MRoomInfo.Reset();
			MRoomInfo = null;
			MGameStartInfo.Reset();
			MGameStartInfo = null;
			FRecoverCdTime = 0;
			NLandlordId = 0;
			NWhoRobLandlordingId = 0;
			RemainLandlordCardList.Clear();
			NFinalAddMultuile = 0;
			NCurrentBetAddMultuileDeskId = 0;
			LastPlayHandInfo.Reset();
			LastPlayHandInfo = null;
			NWhoPlayhandingId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_ConnectLobby : IProtobufResetInterface
	{
		public void Reset()
		{
			MCreaterDB.Reset();
			MCreaterDB = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_ConnectLobby_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			MRoomEntryListInfo.Reset();
			MRoomEntryListInfo = null;
			MRecoverSceneInfo.Reset();
			MRecoverSceneInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_DisConnectLobby : IProtobufResetInterface
	{
		public void Reset()
		{
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_DisConnectLobby_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_RoomEntryInfoPage : IProtobufResetInterface
	{
		public void Reset()
		{
			NPageIndex = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_RoomEntryInfoPage_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			MRoomEntryListInfo.Reset();
			MRoomEntryListInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_UpdatePlayerDb : IProtobufResetInterface
	{
		public void Reset()
		{
			MPlayerDB.Reset();
			MPlayerDB = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_UpdatePlayerDb_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_CreateRoom : IProtobufResetInterface
	{
		public void Reset()
		{
			MRoomBasicInfo.Reset();
			MRoomBasicInfo = null;
			MCreaterDB.Reset();
			MCreaterDB = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_CreateRoomResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			NRoomId = 0;
			MDeskInfo.Reset();
			MDeskInfo = null;
			MPlayerInfo.Reset();
			MPlayerInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_ModifyRoom : IProtobufResetInterface
	{
		public void Reset()
		{
			MRoomBasicInfo.Reset();
			MRoomBasicInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_ModifyRoomResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_EnterRoom : IProtobufResetInterface
	{
		public void Reset()
		{
			NRoomId = 0;
			StrPassword = string.Empty;
			MPlayerDB.Reset();
			MPlayerDB = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_QuickJoinRoom : IProtobufResetInterface
	{
		public void Reset()
		{
			MPlayerDB.Reset();
			MPlayerDB = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_EnterRoomResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			MRecoverSceneInfo.Reset();
			MRecoverSceneInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_LeaveRoom : IProtobufResetInterface
	{
		public void Reset()
		{
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_LeaveRoomResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_addRobot : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_addRobotResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_SpectatorToDesk : IProtobufResetInterface
	{
		public void Reset()
		{
			NUserId = 0;
			NDeskId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_SpectatorToDeskResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_DeskToSpectator : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_DeskToSpectatorResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_ZhuDongSwitchDesk : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
			NAction = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_ZhuDongSwitchDeskResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			NDeskId = 0;
			NAction = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_WhoWithMechangeDesk : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_changeDeskHuiDa : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
			BOk = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_ShuangFangSwitchDeskResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
			BOk = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_kichPlayer : IProtobufResetInterface
	{
		public void Reset()
		{
			NUserId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_kichPlayerResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_orPrepare : IProtobufResetInterface
	{
		public void Reset()
		{
			BPrepare = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_orPrepareReuslt : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			BPrepare = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_Hang : IProtobufResetInterface
	{
		public void Reset()
		{
			BHang = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_HangResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			BHang = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_robLandlord : IProtobufResetInterface
	{
		public void Reset()
		{
			NRobAction = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_robLandlord_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_AddBetMultuile : IProtobufResetInterface
	{
		public void Reset()
		{
			NMultuile = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_AddBetMultuile_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_playhand : IProtobufResetInterface
	{
		public void Reset()
		{
			CardList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_playhand_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_Online_PeopleCount : IProtobufResetInterface
	{
		public void Reset()
		{
			NPeopleCount = 0;
			NRobotCount = 0;
			NRealPlayerCount = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_RoomEntryList_Change : IProtobufResetInterface
	{
		public void Reset()
		{
			MRoomEntryListInfo.Reset();
			MRoomEntryListInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_RoomBasicInfoChange : IProtobufResetInterface
	{
		public void Reset()
		{
			MRoomBasicInfo.Reset();
			MRoomBasicInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_DeskInfoChange : IProtobufResetInterface
	{
		public void Reset()
		{
			DeskInfo.Reset();
			DeskInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_PlayerInfoChange : IProtobufResetInterface
	{
		public void Reset()
		{
			PlayerInfo.Reset();
			PlayerInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_PlayerEnterRoomInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			PlayerInfo.Reset();
			PlayerInfo = null;
			DeskInfo.Reset();
			DeskInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_PlayerLeaveRoomInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NUserId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_KichPlayerInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NUserId = 0;
			NReason = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_UpdateRoomAllInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			MRoomBasicInfo.Reset();
			MRoomBasicInfo = null;
			foreach(var v in PlayerInfoList)
			{
				IMessagePool<NetProtocols.Game.packet_data_playerInfo>.recycle(v);
			}
			PlayerInfoList.Clear();
			foreach(var v in DeskInfoList)
			{
				IMessagePool<NetProtocols.Game.packet_data_deskInfo>.recycle(v);
			}
			DeskInfoList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_entergame : IProtobufResetInterface
	{
		public void Reset()
		{
			MGameStartInfo.Reset();
			MGameStartInfo = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_robLandlord : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_robLandlordResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
			NRobAction = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_SureLandlord : IProtobufResetInterface
	{
		public void Reset()
		{
			RemainLandlordCardList.Reset();
			RemainLandlordCardList = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_who_AddBetMultuile : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_AddBetMultuileResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
			NMultuile = 0;
			NFinalAddMultuile = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_whoPlayhand : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_playhand_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			PlayHandList.Reset();
			PlayHandList = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_notice_winlose : IProtobufResetInterface
	{
		public void Reset()
		{
			NDeskId = 0;
			foreach(var v in OthercardList)
			{
				IMessagePool<NetProtocols.Game.packet_data_deskcardInfo>.recycle(v);
			}
			OthercardList.Clear();
		}
	}
}
