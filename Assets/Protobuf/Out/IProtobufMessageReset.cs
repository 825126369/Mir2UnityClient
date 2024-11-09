using AKNet.Common;
using Google.Protobuf;
namespace NetProtocols.Login
{
	public sealed partial class packet_sc_Login_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = default;
			NAccountId = default;
			SelectGateServerConnectStr = string.Empty;
			BanReason = string.Empty;
			ExpiryDate = default;
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
			NLoginType = default;
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
			NLoginType = default;
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
			NErrorCode = default;
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
			NErrorCode = default;
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_GateServerToSelectGateServer_Data : IProtobufResetInterface
	{
		public void Reset()
		{
			NServerId = default;
			ServerConnectStr = string.Empty;
			OnlinePlayerCount = default;
			CreatedPlayerCount = default;
		}
	}
}
namespace NetProtocols.SelectGate
{
	public sealed partial class packet_SelectGateServerToGateServer_Data : IProtobufResetInterface
	{
		public void Reset()
		{
			NServerId = default;
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
			NServerId = default;
			ServerConnectStr = string.Empty;
			ServerName = string.Empty;
			NState = default;
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
			NErrorCode = default;
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
			NServerType = default;
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
			NErrorCode = default;
		}
	}
}
namespace NetProtocols.Gate
{
	public sealed partial class packet_GIIG_ClientDisConnect : IProtobufResetInterface
	{
		public void Reset()
		{
			NClientId = default;
		}
	}
}
namespace NetProtocols.Gate
{
	public sealed partial class packet_CG_Register : IProtobufResetInterface
	{
		public void Reset()
		{
			NPlayerId = default;
		}
	}
}
namespace NetProtocols.Gate
{
	public sealed partial class packet_GC_RegisterResult : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = default;
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
			DisConnected = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_StartGame : IProtobufResetInterface
	{
		public void Reset()
		{
			NPlayerId = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_request_StartGame_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_TurnDir : IProtobufResetInterface
	{
		public void Reset()
		{
			Direction = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_Walk : IProtobufResetInterface
	{
		public void Reset()
		{
			Direction = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_Run : IProtobufResetInterface
	{
		public void Reset()
		{
			Direction = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_UserLocation : IProtobufResetInterface
	{
		public void Reset()
		{
			Direction = default;
			IMessagePool<NetProtocols.Game.packet_data_Vector3Int>.recycle(Location);
			Location = null;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_broadcast_Location : IProtobufResetInterface
	{
		public void Reset()
		{
			ObjectID = default;
			Direction = default;
			IMessagePool<NetProtocols.Game.packet_data_Vector3Int>.recycle(Location);
			Location = null;
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
			NType = default;
			Message = string.Empty;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_Vector3Int : IProtobufResetInterface
	{
		public void Reset()
		{
			X = default;
			Y = default;
			Z = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_Color : IProtobufResetInterface
	{
		public void Reset()
		{
			R = default;
			G = default;
			B = default;
			A = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_SelectRole_RoleInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NRoleId = default;
			Name = string.Empty;
			Gender = default;
			Class = default;
			Level = default;
			NLastLoginTime = default;
			NCreateTime = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_MapInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			MapIndex = default;
			FileName = string.Empty;
			Title = string.Empty;
			MiniMap = default;
			BigMap = default;
			Music = default;
			Lights = default;
			Lightning = default;
			Fire = default;
			MapDarkLight = default;
			WeatherParticles = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_ItemAtt : IProtobufResetInterface
	{
		public void Reset()
		{
			NType = default;
			NValue = default;
			NMinValue = default;
			NMaxValue = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_ItemInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NItemId = default;
			Name = string.Empty;
			Type = default;
			Grade = default;
			RequiredType = default;
			RequiredClass = default;
			RequiredGender = default;
			Set = default;
			Shape = default;
			Weight = default;
			Light = default;
			RequiredAmount = default;
			Image = default;
			Durability = default;
			Price = default;
			StackSize = default;
			StartItem = default;
			Effect = default;
			NeedIdentify = default;
			ShowGroupPickup = default;
			GlobalDropNotify = default;
			ClassBased = default;
			LevelBased = default;
			CanMine = default;
			CanFastRun = default;
			CanAwakening = default;
			Bind = default;
			Unique = default;
			RandomStatsId = default;
			RandomStats = default;
			ToolTip = string.Empty;
			Slots = default;
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
			Count = default;
			UniqueID = default;
			ItemIndex = default;
			CurrentDura = default;
			MaxDura = default;
			ExpiryDate = default;
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
			Spell = default;
			BaseCost = default;
			LevelCost = default;
			Icon = default;
			Level1 = default;
			Level2 = default;
			Level3 = default;
			Need1 = default;
			Need2 = default;
			Need3 = default;
			Level = default;
			Key = default;
			Range = default;
			Experience = default;
			IsTempSpell = default;
			CastTime = default;
			Delay = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_UserInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			ObjectID = default;
			RealId = default;
			Name = string.Empty;
			GuildName = string.Empty;
			GuildRank = string.Empty;
			IMessagePool<NetProtocols.Game.packet_data_Color>.recycle(NameColour);
			NameColour = null;
			Class = default;
			Gender = default;
			Level = default;
			IMessagePool<NetProtocols.Game.packet_data_Vector3Int>.recycle(Location);
			Location = null;
			Direction = default;
			Hair = default;
			HP = default;
			MP = default;
			Experience = default;
			MaxExperience = default;
			LevelEffects = default;
			HasHero = default;
			HeroBehaviour = default;
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
			Gold = default;
			Credit = default;
			HasExpandedStorage = default;
			ExpandedStorageExpiryTime = default;
			foreach(var v in Magics)
			{
				IMessagePool<NetProtocols.Game.packet_data_ClientMagic>.recycle(v);
			}
			Magics.Clear();
			SummonedCreatureType = default;
			CreatureSummoned = default;
			AllowObserve = default;
			Observer = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_data_cs_ChatInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			UniqueID = default;
			Title = string.Empty;
			Grid = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_AllRoleInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NAccountId = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_request_AllRoleInfo_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = default;
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
			NAccountId = default;
			Name = string.Empty;
			Gender = default;
			Class = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_request_CreateRole_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = default;
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
			NPlayerId = default;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_request_DeleteRole_Result : IProtobufResetInterface
	{
		public void Reset()
		{
			NErrorCode = default;
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
	public sealed partial class packet_sc_broadcast_TurnDir : IProtobufResetInterface
	{
		public void Reset()
		{
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_broadcast_Walk : IProtobufResetInterface
	{
		public void Reset()
		{
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_sc_broadcast_Run : IProtobufResetInterface
	{
		public void Reset()
		{
		}
	}
}
