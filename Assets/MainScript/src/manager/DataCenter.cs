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

    public readonly Regex mAccountRegex = new Regex(@"^[A-Za-z0-9]{8,20}$");
    public readonly Regex mPasswordRegex = new Regex(@"^[A-Za-z0-9]{8,20}$");
    public readonly Regex mMailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

    public void Init()
    {
       
    }
}
