
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FileToolEditor
{
    public static bool CheckFolder(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }
        return false;
    }

    public static void OpenFolder(string path)
    {
        if (CheckFolder(path))
        {
            System.Diagnostics.Process.Start(path);
        }

    }

    public static void CopyFolder(Dictionary<string, string> copyDic)
    {
        foreach (KeyValuePair<string, string> path in copyDic)
        {

            if (CheckFolder(path.Key))
            {

                CopyDir(path.Key, path.Value);
                Debug.Log("Copy Success : \n\tFrom:" + path.Key + " \n\tTo:" + path.Value);
            }
        }
        EditorUtility.ClearProgressBar();
    }

    public static void CopyFolder(string fromPath, string toPath)
    {
        CopyDir(fromPath, toPath);
        Debug.Log("Copy Success : From: " + fromPath + " To: " + toPath);
        EditorUtility.ClearProgressBar();
    }

    public static void CreateFolder(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
    }

    public static void DeleteFolder(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            File.Delete(path + ".meta");
        }
    }

    public static string GetDirParentDir(string path)
    {
        if (Directory.Exists(path))
        {
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }

            int nIndex = path.LastIndexOf("/");
            return path.Substring(0, nIndex);
        }

        return null;
    }
    
    public static string GetParentDirName(string path, string rootDir = null)
    {
        if (File.Exists(path))
        {
            path = Path.GetDirectoryName(path) + "/";
            path = ChangePathSeparator(path);
            rootDir = ChangePathSeparator(rootDir);
            
            if (rootDir == null)
            {
                path = path.Substring(0, path.LastIndexOf("/"));
                int nIndex = path.LastIndexOf("/");
                return path.Substring(nIndex + 1);
            }
            else
            {
                int nIndex = path.IndexOf(rootDir);
                if (nIndex >= 0)
                {
                    path = path.Substring(nIndex + rootDir.Length);
                    return path;
                }else
                {
                    Debug.LogError($"GetParentDirName: path: {path} ");
                    Debug.LogError($"GetParentDirName: rootDir: {rootDir} 不存在");
                }
            }
        }
        else
        {
            Debug.LogError($"GetParentDirName: path: {path} 不存在");
        }

        return null;
    }

    private static string ChangePathSeparator(string path)
    {
        path = path.Replace("\\\\", "/");
        path = path.Replace("\\", "/");
        return path;
    }


    private static void CopyDir(string origin, string target)
    {
        if (!Directory.Exists(target))
        {
            Directory.CreateDirectory(target);
        }

        DirectoryInfo info = new DirectoryInfo(origin);
        FileInfo[] fileList = info.GetFiles();
        DirectoryInfo[] dirList = info.GetDirectories();
        float index = 0;
        foreach (FileInfo fi in fileList)
        {
            if (fi.Extension == ".meta")
            {
                continue;
            }

            float progress = (index / (float)fileList.Length);
            EditorUtility.DisplayProgressBar("Copy ", "Copying: " + Path.GetFileName(fi.FullName), progress);
            File.Copy(fi.FullName, target + fi.Name, true);
            index++;
        }

        foreach (DirectoryInfo di in dirList)
        {
            CopyDir(di.FullName, target + "\\" + di.Name);
        }
    }

    public static void ClearEmptyFolder(string Dir)
    {
        foreach (var v in Directory.GetDirectories(Dir, "*", SearchOption.TopDirectoryOnly))
        {
            bool isEmptyFolder = true;
            foreach (string v1 in Directory.GetFiles(v, "*", SearchOption.TopDirectoryOnly))
            {
                if (Path.GetExtension(v1) != ".meta")
                {
                    isEmptyFolder = false;
                }
            }

            if(isEmptyFolder)
            {
                isEmptyFolder = Directory.GetDirectories(v, "*", SearchOption.TopDirectoryOnly).Length == 0;
            }
            
            if (isEmptyFolder)
            {
                DeleteFolder(v);
                Debug.Log("Empty Folder: " + v);
            }
            else
            {
                ClearEmptyFolder(v);
            }
        }
    }



}