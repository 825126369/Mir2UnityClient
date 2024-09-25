public static class AdEvent
{
    public const int Idle = 0;
    public const int InitOk = 100;
    public const int Ad_Revenue_Reward = 101;
    public const int Ad_Revenue = 102;

    // Interstitial ad
    public const int LoadSuccess = 1;
    public const int LoadFailed = 2;
    public const int Start = 3;
    public const int Closed = 4;
    public const int Clicked = 5;
    public const int PlayError = 6;
    // public const int Ready = 7;

    //Reward Ad
    public const int Reward_LoadSuccess = 11;
    public const int Reward_LoadFailed = 12;
    public const int Reward_Start = 13;
    public const int Reward_Completed = 14;
    public const int Reward_Closed = 15;
    public const int Reward_Clicked = 16;
    public const int Reward_PlayError = 17;

    // banner 
    public const int Banner_LoadSuccess = 27;
}
