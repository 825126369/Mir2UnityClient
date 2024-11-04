using AKNet.Common;
using Google.Protobuf;
namespace NetProtocols.Login
{
	public sealed partial class packet_sc_Login_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			NAccountId = 0;
			SelectGateServerConnectStr = string.Empty;
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
			IMessagePool<NetProtocols.SelectGate.packet_GateServerToSelectGateServer_Data>.recycle(MServerInfo);
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
			IMessagePool<NetProtocols.SelectGate.packet_SelectGateServerToGateServer_Data>.recycle(MServerInfo);
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
namespace NetProtocols.Gate
{
	public sealed partial class packet_IG_Register : IProtobufResetInterface
	{
		public void Reset()
		{
			NServerType = 0;
			ServerConnectStr = string.Empty;
		}
	}
}
namespace NetProtocols.Gate
{
	public sealed partial class packet_GI_RegisterResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Gate
{
	public sealed partial class packet_GIIG_ClientDisConnect : IProtobufResetInterface
	{
		public void Reset()
		{
			NClientId = 0;
		}
	}
}
namespace NetProtocols.Gate
{
	public sealed partial class packet_CG_Register : IProtobufResetInterface
	{
		public void Reset()
		{
			NPlayerId = 0;
		}
	}
}
namespace NetProtocols.Gate
{
	public sealed partial class packet_GC_RegisterResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Gate
{
	public sealed partial class packet_data_RelayMsg : IProtobufResetInterface
	{
		public void Reset()
		{
			IdList.Clear();
			MMsg = ByteString.Empty;
			DisConnected = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_chat : IProtobufResetInterface
	{
		public void Reset()
		{
			Message = string.Empty;
			foreach(var v in LinkedItems)
			{
				IMessagePool<NetProtocols.Game.packet_data_cs_ChatInfo>.recycle(v);
			}
			LinkedItems.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_chat : IProtobufResetInterface
	{
		public void Reset()
		{
			NType = 0;
			Message = string.Empty;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_StartGame : IProtobufResetInterface
	{
		public void Reset()
		{
			NPlayerId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_request_StartGame_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_TurnDir : IProtobufResetInterface
	{
		public void Reset()
		{
			Direction = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_Walk : IProtobufResetInterface
	{
		public void Reset()
		{
			Direction = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_Run : IProtobufResetInterface
	{
		public void Reset()
		{
			Direction = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_UserLocation : IProtobufResetInterface
	{
		public void Reset()
		{
			Direction = 0;
			IMessagePool<NetProtocols.Game.packet_data_Vector3Int>.recycle(Location);
			Location = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_broadcast_TurnDir : IProtobufResetInterface
	{
		public void Reset()
		{
			ObjectID = 0;
			Direction = 0;
			IMessagePool<NetProtocols.Game.packet_data_Vector3Int>.recycle(Location);
			Location = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_broadcast_Walk : IProtobufResetInterface
	{
		public void Reset()
		{
			ObjectID = 0;
			Direction = 0;
			IMessagePool<NetProtocols.Game.packet_data_Vector3Int>.recycle(Location);
			Location = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_broadcast_Run : IProtobufResetInterface
	{
		public void Reset()
		{
			ObjectID = 0;
			Direction = 0;
			IMessagePool<NetProtocols.Game.packet_data_Vector3Int>.recycle(Location);
			Location = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_Vector3Int : IProtobufResetInterface
	{
		public void Reset()
		{
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_Color : IProtobufResetInterface
	{
		public void Reset()
		{
			R = 0;
			G = 0;
			B = 0;
			A = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_SelectRole_RoleInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NRoleId = 0;
			Name = string.Empty;
			Gender = 0;
			Class = 0;
			Level = 0;
			NLastLoginTime = 0;
			NCreateTime = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_MapInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			MapIndex = 0;
			FileName = string.Empty;
			Title = string.Empty;
			MiniMap = 0;
			BigMap = 0;
			Music = 0;
			Lights = 0;
			Lightning = false;
			Fire = false;
			MapDarkLight = 0;
			WeatherParticles = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_ItemAtt : IProtobufResetInterface
	{
		public void Reset()
		{
			NType = 0;
			NValue = 0;
			NMinValue = 0;
			NMaxValue = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_ItemInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NItemId = 0;
			Name = string.Empty;
			Type = 0;
			Grade = 0;
			RequiredType = 0;
			RequiredClass = 0;
			RequiredGender = 0;
			Set = 0;
			Shape = 0;
			Weight = 0;
			Light = 0;
			RequiredAmount = 0;
			Image = 0;
			Durability = 0;
			Price = 0;
			StackSize = 0;
			StartItem = false;
			Effect = 0;
			NeedIdentify = false;
			ShowGroupPickup = false;
			GlobalDropNotify = false;
			ClassBased = false;
			LevelBased = false;
			CanMine = false;
			CanFastRun = false;
			CanAwakening = false;
			Bind = 0;
			Unique = 0;
			RandomStatsId = 0;
			RandomStats = 0;
			ToolTip = string.Empty;
			Slots = 0;
			foreach(var v in Stats)
			{
				IMessagePool<NetProtocols.Game.packet_data_ItemAtt>.recycle(v);
			}
			Stats.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_UserItem : IProtobufResetInterface
	{
		public void Reset()
		{
			IMessagePool<NetProtocols.Game.packet_data_ItemInfo>.recycle(ItemInfo);
			ItemInfo = null;
			Count = 0;
			UniqueID = 0;
			ItemIndex = 0;
			CurrentDura = 0;
			MaxDura = 0;
			ExpiryDate = 0;
			foreach(var v in AddedStats)
			{
				IMessagePool<NetProtocols.Game.packet_data_ItemAtt>.recycle(v);
			}
			AddedStats.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_ClientMagic : IProtobufResetInterface
	{
		public void Reset()
		{
			Name = string.Empty;
			Spell = 0;
			BaseCost = 0;
			LevelCost = 0;
			Icon = 0;
			Level1 = 0;
			Level2 = 0;
			Level3 = 0;
			Need1 = 0;
			Need2 = 0;
			Need3 = 0;
			Level = 0;
			Key = 0;
			Range = 0;
			Experience = 0;
			IsTempSpell = false;
			CastTime = 0;
			Delay = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_UserInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			ObjectID = 0;
			RealId = 0;
			Name = string.Empty;
			GuildName = string.Empty;
			GuildRank = string.Empty;
			IMessagePool<NetProtocols.Game.packet_data_Color>.recycle(NameColour);
			NameColour = null;
			Class = 0;
			Gender = 0;
			Level = 0;
			IMessagePool<NetProtocols.Game.packet_data_Vector3Int>.recycle(Location);
			Location = null;
			Direction = 0;
			Hair = 0;
			HP = 0;
			MP = 0;
			Experience = 0;
			MaxExperience = 0;
			LevelEffects = 0;
			HasHero = false;
			HeroBehaviour = 0;
			foreach(var v in Inventory)
			{
				IMessagePool<NetProtocols.Game.packet_data_UserItem>.recycle(v);
			}
			Inventory.Clear();
			foreach(var v in Equipment)
			{
				IMessagePool<NetProtocols.Game.packet_data_UserItem>.recycle(v);
			}
			Equipment.Clear();
			foreach(var v in QuestInventory)
			{
				IMessagePool<NetProtocols.Game.packet_data_UserItem>.recycle(v);
			}
			QuestInventory.Clear();
			Gold = 0;
			Credit = 0;
			HasExpandedStorage = false;
			ExpandedStorageExpiryTime = 0;
			foreach(var v in Magics)
			{
				IMessagePool<NetProtocols.Game.packet_data_ClientMagic>.recycle(v);
			}
			Magics.Clear();
			SummonedCreatureType = 0;
			CreatureSummoned = false;
			AllowObserve = false;
			Observer = false;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_cs_ChatInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			UniqueID = 0;
			Title = string.Empty;
			Grid = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_AllRoleInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NAccountId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_request_AllRoleInfo_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			foreach(var v in MRoleList)
			{
				IMessagePool<NetProtocols.Game.packet_data_SelectRole_RoleInfo>.recycle(v);
			}
			MRoleList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_CreateRole : IProtobufResetInterface
	{
		public void Reset()
		{
			NAccountId = 0;
			Name = string.Empty;
			Gender = 0;
			Class = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_request_CreateRole_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			foreach(var v in MRoleList)
			{
				IMessagePool<NetProtocols.Game.packet_data_SelectRole_RoleInfo>.recycle(v);
			}
			MRoleList.Clear();
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_DeleteRole : IProtobufResetInterface
	{
		public void Reset()
		{
			NPlayerId = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_request_DeleteRole_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = 0;
			foreach(var v in MRoleList)
			{
				IMessagePool<NetProtocols.Game.packet_data_SelectRole_RoleInfo>.recycle(v);
			}
			MRoleList.Clear();
		}
	}
}
