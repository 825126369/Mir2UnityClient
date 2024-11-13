using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CSVToCSEditor
{
    const string inputPath = "Assets/Excel";
    const string outPath = "Assets/Excel/Out/";

    [MenuItem("Tools/CSV To CS")]
    private static void DoAll()
    {
        if (Directory.Exists(outPath))
        {
            Directory.Delete(outPath, true);
        }
        Directory.CreateDirectory(outPath);

        foreach (var v in Directory.GetFiles(inputPath, "*.csv", SearchOption.AllDirectories))
        {
            string fileName = Path.GetFileName(v);
            if (!fileName.StartsWith(".~"))
            {
                Debug.Log(v);
                Do(v);
            }
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        Debug.Log("CSV To CS Finish");
    }

    private static void Do(string filePath)
    {
        string[] lineList = File.ReadAllLines(filePath);
        string[] varList = lineList[0].Split(',');
        string[] valueList = lineList[1].Split(',');

        string content = $"namespace Mir2\r\n";
        content += "{\r\n";
        content += $"\tpublic class {Path.GetFileNameWithoutExtension(filePath)}\r\n";
        content += "\t{\n";
        for (int j = 0; j < varList.Length; j++)
        {
            if (!string.IsNullOrWhiteSpace(varList[j]))
            {
                content += $"\t\tpublic {GetType(valueList[j])} {varList[j]};\n";
            }
        }
        content += "\t}\n";
        content += "}\n";
        
        string outPath2 = Path.Combine(outPath, Path.GetFileNameWithoutExtension(filePath) + ".cs");
        File.WriteAllText(outPath2, content);
    }

    private static string GetType(string content)
    {
        if(int.TryParse(content, out _))
        {
            return "int";
        }

        if (bool.TryParse(content, out _))
        {
            return "bool";
        }

        return "string";
    }


}
