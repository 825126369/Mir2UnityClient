using System;
using System.Collections.Generic;
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
        // Do();
        Do2();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void Do()
    {
        const string ResetFuncName = "Reset2";

        string mClassStr = string.Empty;
        mClassStr += "namespace XKNet.Common\r\n{\n";
        mClassStr += "\tpublic static partial class IMessageExtention\r\n{\n";

        string mStaticFunc = string.Empty;
        Type[] mType = Assembly.LoadFile(Path.GetFullPath(assemblyFileRelativePath)).GetTypes();
        foreach (var v in mType)
        {
            if (v.FullName.StartsWith(namespaceRootName) && v.Name != "<>c" && !v.Name.EndsWith("Reflection"))
            {
                Debug.Log("当前类型: " + v.Namespace + " | " + v.Name);

                mStaticFunc = string.Empty;
                mStaticFunc += $"\t\tpublic static void {ResetFuncName}(this {GetClassFullName(v)} message)\n";
                mStaticFunc += $"\t\t{{\n";

                foreach (var v2 in v.GetProperties())
                {
                    if (!v2.Name.Contains("Parser") && !v2.Name.Contains("Descriptor"))
                    {
                        if (v2.PropertyType == typeof(uint) || v2.PropertyType == typeof(ulong))
                        {
                            mStaticFunc += $"\t\t\tmessage.{v2.Name} = 0;\n";
                        }
                        else if (v2.PropertyType == typeof(bool))
                        {
                            mStaticFunc += $"\t\t\tmessage.{v2.Name} = false;\n";
                        }
                        else if (v2.PropertyType == typeof(string))
                        {
                            mStaticFunc += $"\t\t\tmessage.{v2.Name} = string.Empty;\n";
                        }
                        else if (v2.PropertyType.Name.Contains("RepeatedField"))
                        {
                            if (v2.PropertyType.GenericTypeArguments[0].IsClass)
                            {
                                mStaticFunc += $"\t\t\tforeach(var v in message.{v2.Name})\n";
                                mStaticFunc += $"\t\t\t{{\n";
                                mStaticFunc += $"\t\t\t\tv.{ResetFuncName}();\n";
                                mStaticFunc += $"\t\t\t\tIMessagePool<{GetClassFullName(v2.PropertyType.GenericTypeArguments[0])}>.recycle(v);\n";
                                mStaticFunc += "\t\t\t}\n";
                            }

                            mStaticFunc += $"\t\t\tmessage.{v2.Name}.Clear();\n";
                        }
                        else if (v2.PropertyType.IsClass && !v2.PropertyType.IsGenericType) //类
                        {
                            mStaticFunc += $"\t\t\tmessage.{v2.Name}.{ResetFuncName}();\n";
                            mStaticFunc += $"\t\t\tmessage.{v2.Name} = null;\n";
                        }
                        else
                        {
                            Debug.LogError($"不支持的类型：{v2.PropertyType.Name} : {v2.Name}");
                        }
                    }
                }

                mStaticFunc += "\t\t}\n";
                mClassStr += mStaticFunc;
            }
        }

        mClassStr += "\t}\n";
        mClassStr += "}";

        string filePath = Path.Combine(ProtocolCSPath, "IMessageExtention.cs");
        File.WriteAllText(filePath, mClassStr);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void Do2()
    {
        const string ResetFuncName = "Reset";

        string mContent = string.Empty;
        mContent += "using XKNet.Common;\n";

        string mNameSpaceStr = string.Empty;
        Type[] mType = Assembly.LoadFile(Path.GetFullPath(assemblyFileRelativePath)).GetTypes();
        foreach (var v in mType)
        {
            if (v.FullName.StartsWith(namespaceRootName) && v.Name != "<>c" && !v.Name.EndsWith("Reflection"))
            {
                Debug.Log("当前类型: " + v.Namespace + " | " + v.Name);

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
                        if (v2.PropertyType == typeof(uint) || v2.PropertyType == typeof(ulong))
                        {
                            mStaticFunc += $"\t\t\t{v2.Name} = 0;\n";
                        }
                        else if (v2.PropertyType == typeof(bool))
                        {
                            mStaticFunc += $"\t\t\t{v2.Name} = false;\n";
                        }
                        else if (v2.PropertyType == typeof(string))
                        {
                            mStaticFunc += $"\t\t\t{v2.Name} = string.Empty;\n";
                        }
                        else if (v2.PropertyType.Name.Contains("RepeatedField"))
                        {
                            if (v2.PropertyType.GenericTypeArguments[0].IsClass)
                            {
                                mStaticFunc += $"\t\t\tforeach(var v in {v2.Name})\n";
                                mStaticFunc += $"\t\t\t{{\n";
                                mStaticFunc += $"\t\t\t\tIMessagePool<{GetClassFullName(v2.PropertyType.GenericTypeArguments[0])}>.recycle(v);\n";
                                mStaticFunc += "\t\t\t}\n";
                            }

                            mStaticFunc += $"\t\t\t{v2.Name}.Clear();\n";
                        }
                        else if(v2.PropertyType.IsClass && !v2.PropertyType.IsGenericType) //类
                        {
                            mStaticFunc += $"\t\t\t{v2.Name}.{ResetFuncName}();\n";
                            mStaticFunc += $"\t\t\t{v2.Name} = null;\n";
                        }
                        else
                        {
                            Debug.LogError($"不支持的类型：{v2.PropertyType.Name} : {v2.Name}");
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