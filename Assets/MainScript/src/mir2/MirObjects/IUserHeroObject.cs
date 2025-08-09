using NetProto.ShareData;

namespace Mir2
{
    public interface IUserHeroObject
    {
        void Load(NetProto.SCPacket.packet_sc_UserInformation info);
    }
}