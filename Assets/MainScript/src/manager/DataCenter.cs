using System.Text.RegularExpressions;

public class DataCenter:SingleTonMonoBehaviour<DataCenter>
{
    // 当前存档时间
    public bool bMute = false;
    public int nWebTag = 0;
    public bool bReview = false;
    public int nThemeIndex = 1;
    public int nCoinCount = 0;
    public int nLevel;
    public bool bShowResurrectionUI = false;

    const string regexPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$|^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public Regex mAccountRegex = null;
    public Regex mPasswordRegex = null;

    public void Init()
    {
        mAccountRegex = new Regex(regexPattern);
        mPasswordRegex = new Regex(regexPattern);
    }
}
