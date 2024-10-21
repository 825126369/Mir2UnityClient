using XKNet.Common;
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
namespace NetProtocols.Game
{
	public sealed partial class packet_data_SelectRole_RoleInfo : IProtobufResetInterface
	{
		public void Reset()
		{
			NRoleIndex = 0;
			Name = string.Empty;
			Gender = 0;
			Class = 0;
			Level = 0;
			LastAccess = 0;
		}
	}
}
namespace NetProtocols.Game
{
	public sealed partial class packet_cs_request_AllRoleInfo : IProtobufResetInterface
	{
		public void Reset()
		{
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
			NRoleIndex = 0;
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
