using Newtonsoft.Json;
using System.IO;

public static class AKParser
{
    const string saveDir = "D:\\Me\\MyProject\\CrystalMir2\\DbInfo\\";
    const string saveMapDir = "D:\\Me\\MyProject\\CrystalMir2\\DbInfo\\Map\\";

    public static void ParseMapAny(string fileName, object mInfo)
    {
        string Name = string.Empty;
        if (mInfo.GetType().IsGenericType)
        {
            Name = mInfo.GetType().GetGenericArguments()[0].Name;
        }
        else
        {
            Name = mInfo.GetType().Name;
        }

        string content = JsonConvert.SerializeObject(mInfo);
        string outPath = Path.Combine(saveMapDir, fileName + ".json");
        File.WriteAllText(outPath, content);
    }

    public static void ParseAny(object mInfo)
    {
        string Name = string.Empty;
        if (mInfo.GetType().IsGenericType)
        {
            Name = mInfo.GetType().GetGenericArguments()[0].Name;
        }
        else
        {
            Name = mInfo.GetType().Name;
        }

        string content = JsonConvert.SerializeObject(mInfo);
        string outPath = Path.Combine(saveDir, Name + ".json");
        File.WriteAllText(outPath, content);
    }

    public static void ParseUnityAny(object mInfo)
    {
        string saveDir = "Assets/CrystalMir2Export/";
        string Name = string.Empty;
        if (mInfo.GetType().IsGenericType)
        {
            Name = mInfo.GetType().GetGenericArguments()[0].Name;
        }
        else
        {
            Name = mInfo.GetType().Name;
        }

        string content = JsonConvert.SerializeObject(mInfo);
        string outPath = Path.Combine(saveDir, Name + ".json");
        File.WriteAllText(outPath, content);
    }

}
