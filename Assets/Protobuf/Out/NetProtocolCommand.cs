public static class NetProtocolCommand
{
    //登录===========================================================================
    public const ushort CS_REQUEST_LOGIN = 1000; //--登录
    public const ushort SC_REQUEST_LOGIN_RESULT = 1001;

    public const ushort CS_REQUEST_REGISTER = 1002; //--注册
    public const ushort SC_REQUEST_REGISTER_RESULT = 1003;

    public const ushort CS_REQUEST_CHANGE_PASSWORD = 1004; //--修改密码
    public const ushort SC_REQUEST_CHANGE_PASSWORD_RESULT = 1005;

    //选择服务器===========================================================================
    public const ushort GSG_SERVER_INFO = 1020; //--【网关】发送给【选择网关】服务器信息
    public const ushort SGG_SERVER_INFO_RESULT = 1021; 

    public const ushort CS_REQUEST_SERVER_LIST = 1022; //--客户端请求服务器列表
    public const ushort SC_REQUEST_SERVER_LIST_RESULT = 1023;

    //网关命令===========================================================================
    public const ushort IG_REGISTER_SERVER_INFO = 1040; //--【内部】 发送给 【网关】的注册信息
    public const ushort GI_REGISTER_SERVER_INFO_RESULT = 1041;
    public const ushort GC_INNER_SERVER_NET_ERROR = 1042;

    //游戏命令===========================================================================
    public const ushort CS_REQUEST_SELECTROLE_ALL_ROLEINFO = 1100; //-- 选择界面，请求所有角色信息
    public const ushort SC_REQUEST_SELECTROLE_ALL_ROLEINFO_RESULT = 1101; //--

    public const ushort CS_REQUEST_SELECTROLE_CREATE_ROLE = 1102; //-- 选择界面，创建角色
    public const ushort SC_REQUEST_SELECTROLE_CREATE_ROLE_RESULT = 1103; //

    public const ushort CS_REQUEST_SELECTROLE_DELETE_ROLE = 1104; //-- 选择界面，删除角色
    public const ushort SC_REQUEST_SELECTROLE_DELETE_ROLE_RESULT = 1105; //
    
}

public static class NetErrorCode
{
    public const uint NoError = 0;
    public const uint DbOpError = 1;
    public const uint ServerError = 2;
    public const uint ClientMsgError = 3;
}

