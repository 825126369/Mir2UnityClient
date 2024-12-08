using AKNet.Extentions.Protobuf.Editor;
using UnityEditor;

public class ProtobufGenReset
{
    private const string ProtocolCSPath = "Assets/Protobuf/Out/";
    private const string namespaceRootName = "NetProtocols";
    const string assemblyFileRelativePath = "./Library/ScriptAssemblies/Assembly-CSharp.dll";

    [MenuItem("Tools/Protobuf Gen IProtobufMessageReset")]
    public static void Build()
    {
        AKNetProtoBufEditor.DoProtoResetCSFile(ProtocolCSPath, namespaceRootName, assemblyFileRelativePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}