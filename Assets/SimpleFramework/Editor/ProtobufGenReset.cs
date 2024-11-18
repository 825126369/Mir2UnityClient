using Google.Protobuf;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using AKNet.Editor;

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