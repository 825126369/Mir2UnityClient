using System;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

public class ThemeSolitaire_ExcelToLua
{
    const string inputPath = "Assets/Excel/";
    const string outPath = "Assets/Excel/Out/";
    const string copyPath = "Assets/Lua/MainLogic/AutoGenExcelOut/";
    static List<string> mListColumnTypeInfo = null;
    static List<int> mListValidColumn = null;

    [MenuItem("ExcelToLua/ThemeSolitaire")]
    public async static void Main()
    {
        EditorApplication.update += Update;
        if (Directory.Exists(outPath))
        {
            Directory.Delete(outPath, true);
        }

        Directory.CreateDirectory(outPath);
        foreach (var v in Directory.GetFiles(inputPath, "*.xlsx", SearchOption.AllDirectories))
        {
            string fileName = Path.GetFileName(v);
            if (!fileName.StartsWith(".~"))
            {
                Debug.Log(v);
                await Do(v);
            }
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    [InitializeOnLoadMethod]
    private static void ListentScriptEvent()
    {
        Debug.Log("ListentScriptEvent: --------------------------------");
        CancelTask();
    }

    private static string textProgressBarInfo = string.Empty;
    private static float fProgress = 0f;
    private static int nTaskState = 0;
    private static void Update()
    {
        if (nTaskState == 1)
        {
            if (EditorUtility.DisplayCancelableProgressBar("ThemeSolitaire_ExcelToLua", textProgressBarInfo, fProgress))
            {
                CancelTask();
            }
        }
        else if (nTaskState == 2)
        {
            EditorUtility.ClearProgressBar();
            nTaskState = 0;
        }
    }

    static async Task Do(string filePath)
    {
        // try
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {

                        }
                    } while (reader.NextResult());

                    System.Data.DataSet result = reader.AsDataSet();
                    await ParseDataSet(filePath, result.Tables[0]);
                }
            }
        }
    }

    static async Task ParseDataSet(string filePath, DataTable result)
    {
        ParseColumnType(result); ;
        await ParseAllRawData(filePath, result);
    }

    static CancellationTokenSource tokenSource = null;
    static List<Task> mAllTask = null;
    static void CancelTask()
    {
        if (tokenSource != null)
        {
            Debug.Log("CancelTask");
            tokenSource.Cancel();
            EditorUtility.ClearProgressBar();
            nTaskState = 0;
            tokenSource = null;
        }
    }

    static void ParseColumnType(DataTable mTable)
    {
        mListColumnTypeInfo = new List<string>();
        mListValidColumn = new List<int>();

        for (int j = 0; j < mTable.Columns.Count; j++)
        {
            string fieldName = mTable.Rows[0][j].ToString();
            string typeName = mTable.Rows[1][j].ToString();
            string desName = mTable.Rows[2][j].ToString();

            mListColumnTypeInfo.Add(fieldName.Trim());
            mListColumnTypeInfo.Add(desName.Trim());
            mListColumnTypeInfo.Add(typeName.Trim());
        }
    }

    static bool CheckValidColumn(string fieldName, string typeName)
    {
        if (string.IsNullOrWhiteSpace(fieldName) || string.IsNullOrWhiteSpace(typeName))
        {
            return false;
        }

        if (typeName == "int" || typeName == "string" || typeName == "float" ||
            typeName == "int{}" || typeName == "string{}" || typeName == "float{}")
        {
            return true;
        }

        return false;
    }

    static bool CheckValidRow(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    static async Task ParseAllRawData(string filePath, DataTable mTable)
    {
        tokenSource = new CancellationTokenSource();
        mAllTask = new List<Task>();
        nTaskState = 1;

        for (int j = 3; j < mTable.Rows.Count; j++)
        {
            if (!CheckValidRow(mTable.Rows[j][0].ToString()))
            {
                break;
            }

            int nRowIndex = j;
            var mTask = Task.Run(() =>
            {
                string outStr = ParseOneRawData(nRowIndex, filePath, mTable);
                if (!tokenSource.IsCancellationRequested)
                {
                    lock (mLayerDic)
                    {
                        mLayerDic[nRowIndex] = outStr;

                        fProgress = nRowIndex / (float)mTable.Rows.Count;
                        textProgressBarInfo = $"Parse Raw: {nRowIndex}/{mTable.Rows.Count}";
                    }
                }
            });
            mAllTask.Add(mTask);
        }

        if (!tokenSource.IsCancellationRequested)
        {
            var nLastDateTime = DateTime.Now;
            await Task.WhenAll(mAllTask);

            PrintTool.LogWithColor("Parse All Raw Time: " + (DateTime.Now - nLastDateTime).TotalSeconds);
                
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string LuaFileName = fileName;
            int nRemoveCount = 0;
            for(int i = 0; i < LuaFileName.Length; i++)
            {
                int _;
                if(int.TryParse(LuaFileName[i].ToString(), out _))
                {
                    nRemoveCount++;
                }else
                {
                    break;
                }
            }
            LuaFileName = LuaFileName.Remove(0, nRemoveCount);
            LuaFileName = "Config_" + LuaFileName;

            StringBuilder mBuilder = new StringBuilder();
            mBuilder.Append("local " + LuaFileName + " = {\n");

            for (int i = 0; i < mTable.Rows.Count; i++)
            {
                if (mLayerDic.ContainsKey(i) && !string.IsNullOrWhiteSpace(mLayerDic[i]))
                {
                    mBuilder.Append(mLayerDic[i]);
                }

                fProgress = i / (float)mTable.Rows.Count;
                textProgressBarInfo = $"Combine Raw: {i}/{mTable.Rows.Count}";
            }

            mBuilder.Append("}\n\n");
            mBuilder.Append("return " + LuaFileName + "\n");

            string outFileName = LuaFileName + ".lua";
            string outFilePath = Path.Combine(outPath, outFileName);
            File.WriteAllText(outFilePath, mBuilder.ToString(), Encoding.UTF8);
            File.Copy(outFilePath, Path.Combine(copyPath, outFileName), true);
        }

        EditorUtility.ClearProgressBar();
        nTaskState = 0;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        await Task.CompletedTask;
    }

    static string ParseOneRawData(int j, string filePath, DataTable mTable)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string outStr = string.Empty;

        int nRowIdnex = j - 2;
        outStr += "\t[" + nRowIdnex + "]={";
        for (int k = 0; k < mTable.Columns.Count; k++)
        {
            string value = mTable.Rows[j][k].ToString();

            string fieldName = mListColumnTypeInfo[k * 3 + 0];
            string desName = mListColumnTypeInfo[k * 3 + 1];
            string typeName = mListColumnTypeInfo[k * 3 + 2];

            if (tokenSource.IsCancellationRequested)
            {
                return outStr;
            }

            if (!CheckValidColumn(fieldName, typeName))
            {
                break;
            }

            if (typeName == "float")
            {
                float fValue = float.Parse(value);
                outStr += fieldName + " = " + fValue;
            }
            else if (typeName == "int")
            {
                outStr += fieldName + " = " + ParseNumberValue(value, fileName, j, k);
            }
            else if (typeName == "string")
            {
                string strValue = value;
                strValue = strValue.Replace("\"", "");
                strValue = strValue.Replace("\"", "");
                strValue = strValue.Replace("“", "");
                strValue = strValue.Replace("”", "");
                strValue = strValue.Replace("‘", "");
                strValue = strValue.Replace("’", "");
                outStr += fieldName + " = \"" + strValue + "\"";
            }
            else if (typeName.EndsWith("{}"))
            {
                string subtypeName = typeName.Substring(0, typeName.Length - 2);
                string strValue = value;
                string[] words = strValue.Split(',');

                outStr += fieldName + " = {";
                for (int n = 0; n < words.Length; n++)
                {
                    string tempvalue = words[n];
                    if (subtypeName == "float")
                    {
                        float fValue = float.Parse(tempvalue);
                        if (n == 0)
                        {
                            outStr += fValue;
                        }
                        else
                        {
                            outStr += ", " + fValue;
                        }
                    }
                    else if (subtypeName == "int")
                    {
                        var valueResult = ParseNumberValue(tempvalue, fileName, j, k);
                        if (n == 0)
                        {
                            outStr += valueResult;
                        }
                        else
                        {
                            outStr += ", " + valueResult;
                        }
                    }
                    else if (subtypeName == "string")
                    {
                        if (n == 0)
                        {
                            outStr += tempvalue;
                        }
                        else
                        {
                            outStr += ", " + tempvalue;
                        }
                    }
                    else
                    {
                        Debug.LogError($"类型错误:{subtypeName},  {fileName}: [{j}, {k}]:{value}");
                        Debug.Assert(false);
                    }
                }
                outStr += "}";
            }
            else
            {
                Debug.LogError($"类型错误:{typeName},  {fileName}: [{j}, {k}]:{value}");
                break;
            }

            if (k < mTable.Columns.Count - 1)
            {
                outStr += ",\t";
            }
        }

        outStr += "},\n";
        return outStr;
    }

    static Dictionary<int, string> mLayerDic = new Dictionary<int, string>();

    private static string ParseNumberValue(string value, string fileName, int nRow, int nColumn)
    {
        string outStr = "0";
        try
        {
            int nValue;
            nValue = int.Parse(value);
            outStr = "" + nValue;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message + " | " + e.StackTrace);
            Debug.LogWarning($"{fileName}: [{nRow}, {nColumn}]:{value}");
        }

        return outStr;
    }
}
