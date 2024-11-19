using System;
using System.IO;


public static class FileTool
{
    public static string GetConfigDir()
    {
        string dir = GetParentSpecialDir("Config");
        return dir;
    }

    public static string GetMir2ConfigDir()
    {
        string dir = GetParentSpecialDir("Mir2Config");
        return dir;
    }

    public static string GetLogDir()
    {
        string dir = GetParentSpecialDir("Log");
        return dir;
    }

    public static string GetCSProjDir()
    {
        return GetSpecialSuffixDir(AppDomain.CurrentDomain.BaseDirectory, ".csproj");
    }

    public static string GetSlnDir()
    {
        return GetSpecialSuffixDir(AppDomain.CurrentDomain.BaseDirectory, ".sln");
    }

    public static string FindChildSpecialDir(string specialDirName)
    {
        string[] mDirList = Directory.GetDirectories(GetSlnDir(), $"*", SearchOption.AllDirectories);
        foreach (string v in mDirList)
        {
            if (v.EndsWith(specialDirName))
            {
                return v;
            }
        }
        return string.Empty;
    }

    public static string FindChildSpecialDir(string rootDirName, string specialDirName)
    {
        string[] mDirList = Directory.GetDirectories(rootDirName, $"*", SearchOption.AllDirectories);
        foreach (string v in mDirList)
        {
            if (v.EndsWith(specialDirName))
            {
                return v;
            }
        }

        return string.Empty;
    }

    public static string GetParentSpecialDir(string dirName)
    {
        string dirPath = AppDomain.CurrentDomain.BaseDirectory;
        while (!string.IsNullOrWhiteSpace(dirPath))
        {
            string[] mDirList = Directory.GetDirectories(dirPath, $"*", SearchOption.TopDirectoryOnly);
            foreach (string v in mDirList)
            {
                if (GetShortDirName(v) == dirName)
                {
                    return v;
                }
            }
            dirPath = Path.GetDirectoryName(dirPath);
        }
        return null;
    }

    public static string GetSpecialSuffixDir(string dirPath, string suffix)
    {
        if (!dirPath.EndsWith(Path.DirectorySeparatorChar))
        {
            dirPath = Path.GetDirectoryName(dirPath);
        }

        while (!string.IsNullOrWhiteSpace(dirPath))
        {
            string[] v = Directory.GetFiles(dirPath, $"*{suffix}", SearchOption.TopDirectoryOnly);
            if (v != null && v.Length > 0)
            {
                return dirPath;
            }
            else
            {
                dirPath = Path.GetDirectoryName(dirPath);
            }
        }

        return null;
    }

    public static string GetShortDirName(string dirPath)
    {
        if (dirPath.EndsWith(Path.PathSeparator))
        {
            dirPath = Path.GetDirectoryName(dirPath);
        }

        int nBegIndex = dirPath.LastIndexOf(Path.DirectorySeparatorChar);
        string shortName = dirPath.Substring(nBegIndex + 1);
        return shortName;
    }
}