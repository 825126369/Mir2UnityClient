using NetProtocols.Game;

namespace Mir2
{
    public interface IUserHeroObject
    {
        void Load(packet_data_UserInfo info);
    }
}