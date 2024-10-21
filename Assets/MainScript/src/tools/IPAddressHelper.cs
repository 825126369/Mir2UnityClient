using System;

public static class IPAddressHelper
{
    public static bool TryParseConnectStr(string connectStr, out string ip, out ushort nPort)
    {
        ip = string.Empty;
        nPort = 0;
        try
        {
            string[] mArray = connectStr.Split(':');
            ip = mArray[0];
            nPort = ushort.Parse(mArray[1]);
            return true;
        }
        catch (Exception e)
        {
            PrintTool.LogError(e);
        }
        return false;
    }
}
