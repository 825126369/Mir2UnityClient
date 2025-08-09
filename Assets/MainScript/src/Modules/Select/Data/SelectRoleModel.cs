using NetProto.ShareData;
using System.Collections.Generic;

public class SelectRoleModel:Singleton<SelectRoleModel>
{
    public List<packet_data_SelectInfo> m_packet_data_SelectInfo_List = new List<packet_data_SelectInfo>();
    public DataBind<List<packet_data_SelectInfo>> mDataBind_packet_data_SelectRole_RoleInfo;

    public void Init()
    {
        mDataBind_packet_data_SelectRole_RoleInfo = new DataBind<List<packet_data_SelectInfo>>(m_packet_data_SelectInfo_List);
    }
}