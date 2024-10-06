using System.Diagnostics;
using System.IO;
using UnityEditor;

public class ProtoBufEditor
{
    private const string ProtocPath = "Assets/SimpleFramework/Tcp/protoc-28.2-win64/bin/protoc.exe";
    private const string ProtocolPath = "Assets/Protobuf";
    private const string ProtocolCSPath = "Assets/Protobuf/Out/";

    private const string NetInnerProtocolPath = "Assets/SimpleFramework/Tcp/Protobuf/";

    [MenuItem("Tools/Protobuf Gen")]
    private static void Do()
    {
        if (!Directory.Exists(ProtocolPath))
        {
            Directory.CreateDirectory(ProtocolPath);
        }
        if (!Directory.Exists(ProtocolCSPath))
        {
            Directory.CreateDirectory(ProtocolCSPath);
        }


        string arg = $"--csharp_out={Path.GetRelativePath(ProtocolPath, ProtocolCSPath)} ";
        foreach (string v in Directory.GetFiles(ProtocolPath, "*.proto", SearchOption.TopDirectoryOnly))
        {
            arg += " " + Path.GetFileName(v);
        }
        
        RunCmd(Path.GetFullPath(ProtocPath), Path.GetFullPath(ProtocolPath), arg);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    //[MenuItem("Tools/Protobuf Gen 2")]
    private static void Do2()
    {
        if (!Directory.Exists(ProtocolPath))
        {
            Directory.CreateDirectory(ProtocolPath);
        }
        if (!Directory.Exists(ProtocolCSPath))
        {
            Directory.CreateDirectory(ProtocolCSPath);
        }
        
        string arg = $"--csharp_out={Path.GetFullPath(ProtocolCSPath)}";
        arg += " --proto_path=" + Path.GetFullPath(ProtocolPath);
        arg += " --proto_path=" + Path.GetFullPath(NetInnerProtocolPath);
        foreach (string v in Directory.GetFiles(ProtocolPath, "*.proto", SearchOption.TopDirectoryOnly))
        {
            arg += " " + Path.GetFileName(v);
        }
        foreach (string v in Directory.GetFiles(NetInnerProtocolPath, "*.proto", SearchOption.TopDirectoryOnly))
        {
            arg += " " + Path.GetFileName(v);
        }

        RunCmd(Path.GetFullPath(ProtocPath), null, arg);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void RunCmd(string exePath, string workPath, string arg)
    {
        ProcessStartInfo info = new ProcessStartInfo();
        info.Arguments = arg;
        info.WorkingDirectory = workPath;
        info.FileName = exePath;
        info.UseShellExecute = false;
        info.CreateNoWindow = true;
        info.RedirectStandardOutput = true;
        info.RedirectStandardError = true;
        info.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
        info.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;

        UnityEngine.Debug.Log("RunCmd: " + info.Arguments);

        Process process = Process.Start(info);
        string strOutput = process.StandardOutput.ReadToEnd();
        if (!string.IsNullOrWhiteSpace(strOutput))
        {
            UnityEngine.Debug.Log(strOutput);
        }
        strOutput = process.StandardError.ReadToEnd();
        if (!string.IsNullOrWhiteSpace(strOutput))
        {
            UnityEngine.Debug.LogError(strOutput);
        }

        process.WaitForExit();
        process.Close();
    }
}