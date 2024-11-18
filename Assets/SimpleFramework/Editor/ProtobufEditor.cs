using AKNet.Editor;
using UnityEditor;

public class ProtoBufEditor
{
    private const string ProtocPath = "Assets/protoc-28.2-win64/bin/protoc.exe";
    private const string ProtocolPath = "Assets/Protobuf";
    private const string ProtocolCSPath = "Assets/Protobuf/Out/";

    [MenuItem("Tools/Protobuf Gen => public class")]
    private static void DoPublic()
    {
        AKNetProtoBufEditor.DoPublicCSFile(ProtocPath, ProtocolCSPath, ProtocolPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}