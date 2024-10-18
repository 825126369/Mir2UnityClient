public static class NetProtocolCommand
{
    public const ushort CS_REQUEST_LOGIN = 1000; //--登录
    public const ushort SC_REQUEST_LOGIN_RESULT = 1001;

    public const ushort CS_REQUEST_REGISTER = 1002; //--注册
    public const ushort SC_REQUEST_REGISTER_RESULT = 1003;

    public const ushort CS_REQUEST_CHANGE_PASSWORD = 1004; //--修改密码
    public const ushort SC_REQUEST_CHANGE_PASSWORD_RESULT = 1005;

    public const ushort SToGS_SEND_SERVER_INFO = 1006; //--发送给网关服务器信息
    public const ushort CS_REQUEST_SERVER_LIST = 1007; //--客户端请求服务器列表
    public const ushort SC_REQUEST_SERVER_LIST_RESULT = 1008; //--客户端请求服务器列表

    //---------------广播协议---------------------
    //public const ushort SC_NOTICE_ONLINE_PLAYER_COUNT = 1100; // 通知 玩家在线人数
    //public const ushort SC_NOTICE_ROOMENTRYLIST_INFO = 1101; // 通知 房间入口列表信息
}

public static class NetErrorCode
{
    public const uint NoError = 0;
    public const uint DbOpError = 1;
    public const uint ServerError = 2;
    public const uint ClientMsgError = 3;

    public const uint ZhangHaoExist = 10; //-- 账号已存在
    public const uint ZhangHaoMiMaBuPiPei = 11; //-- 账号密码不匹配
}

