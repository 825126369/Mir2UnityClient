using Google.Protobuf;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ProtobufGenReset
{
    private const string ProtocolCSPath = "Assets/Protobuf/Out/";
    private const string namespaceRootName = "NetProtocols";
    const string assemblyFileRelativePath = "./Library/ScriptAssemblies/Assembly-CSharp.dll";

    [MenuItem("Tools/Protobuf Gen IProtobufMessageReset")]
    public static void Build()
    {
        Do2();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void Do2()
    {
        const string ResetFuncName = "Reset";

        string mContent = string.Empty;
        mContent += "using AKNet.Common;\n";
        mContent += "using Google.Protobuf;\n";

        string mNameSpaceStr = string.Empty;
        Type[] mType = Assembly.LoadFile(Path.GetFullPath(assemblyFileRelativePath)).GetTypes();
        foreach (var v in mType)
        {
            bool bOnlyProtobufResetCSFile = true;
            foreach (var v2 in v.GetProperties())
            {
                if (v2.Name.Contains("Parser") || v2.Name.Contains("Descriptor"))
                {
                    bOnlyProtobufResetCSFile = false;
                    break;
                }
            }

            if (!bOnlyProtobufResetCSFile && v.FullName.StartsWith(namespaceRootName) && v.Name != "<>c" && !v.Name.EndsWith("Reflection"))
            {
                Debug.Log("��ǰ����: " + v.Namespace + " | " + v.Name);

                mNameSpaceStr = string.Empty;
                mNameSpaceStr += $"namespace {v.Namespace}\n";
                mNameSpaceStr += $"{{\n";

                string mClassStr = string.Empty;
                mClassStr += $"\tpublic sealed partial class {v.Name} : IProtobufResetInterface\n";
                mClassStr += $"\t{{\n";

                string mStaticFunc = string.Empty;
                mStaticFunc += $"\t\tpublic void {ResetFuncName}()\n";
                mStaticFunc += $"\t\t{{\n";


                foreach (var v2 in v.GetProperties())
                {
                    if (!v2.Name.Contains("Parser") && !v2.Name.Contains("Descriptor"))
                    {
                        if (v2.PropertyType.IsValueType)
                        {
                            mStaticFunc += $"\t\t\t{v2.Name} = default;\n";
                        }
                        else if (v2.PropertyType == typeof(string))
                        {
                            mStaticFunc += $"\t\t\t{v2.Name} = string.Empty;\n";
                        }
                        else if (v2.PropertyType == typeof(ByteString))
                        {
                            mStaticFunc += $"\t\t\t{v2.Name} = ByteString.Empty;\n";
                        }
                        else if (v2.PropertyType.Name.Contains("RepeatedField"))
                        {
                            if (v2.PropertyType.GenericTypeArguments[0].IsClass && v2.PropertyType.GenericTypeArguments[0] != typeof(string))
                            {
                                mStaticFunc += $"\t\t\tforeach(var v in {v2.Name})\n";
                                mStaticFunc += $"\t\t\t{{\n";
                                mStaticFunc += $"\t\t\t\tIMessagePool<{GetClassFullName(v2.PropertyType.GenericTypeArguments[0])}>.recycle(v);\n";
                                mStaticFunc += "\t\t\t}\n";
                            }

                            mStaticFunc += $"\t\t\t{v2.Name}.Clear();\n";
                        }
                        else if (v2.PropertyType.IsClass && !v2.PropertyType.IsGenericType) //��
                        {
                            mStaticFunc += $"\t\t\tIMessagePool<{GetClassFullName(v2.PropertyType)}>.recycle({v2.Name});\n";
                            mStaticFunc += $"\t\t\t{v2.Name} = null;\n";
                        }
                        else
                        {
                            Debug.LogError($"��֧�ֵ����ͣ�{v2.PropertyType.Name} : {v2.Name}");
                        }
                    }
                }

                mStaticFunc += "\t\t}\n";
                mClassStr += mStaticFunc + "\t}\n";
                mNameSpaceStr += mClassStr + "}\n";
                mContent += mNameSpaceStr;

            }
        }
        string filePath = Path.Combine(ProtocolCSPath, "IProtobufMessageReset.cs");
        File.WriteAllText(filePath, mContent);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    private static string GetClassFullName(Type v)
    {
        string className = $"{v.Namespace}.{v.Name}";
        return className;
    }
}