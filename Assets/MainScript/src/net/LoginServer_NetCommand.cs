
public static class LoginServer_NetErrorCode
{
    public const uint NoError = 0;
    public const uint DbOpError = 1;
    public const uint ServerError = 2;
    public const uint ZhangHaoExist = 10; //-- 账号已存在
}

public static class LoginServer_NetCommand
{
    public const ushort CS_REQUEST_LOGIN = 1000; //--请求登录
    public const ushort SC_REQUEST_LOGIN_RESULT = 1001;

    //---------------广播协议---------------------
    //public const ushort SC_NOTICE_ONLINE_PLAYER_COUNT = 1100; // 通知 玩家在线人数
    //public const ushort SC_NOTICE_ROOMENTRYLIST_INFO = 1101; // 通知 房间入口列表信息
}
