//using System;
//using System.Data;
//using System.IO;
//using ExcelDataReader;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using System.Text;

//public class ExcelToLuaEditor
//{
//    const bool bRemovePrefix4 = true;
//    const string inputPath = "../../3d_N1_Math/data/";
//    const string outPath = "Assets/Lua/GameConfig/";
//    static List<string> mListColumnTypeInfo = null;
//    static List<int> mListValidColumn = null;

//    [MenuItem("Tools/Excel To Lua")]
//    public static void Main()
//    {
//        if (Directory.Exists(outPath))
//        {
//            Directory.Delete(outPath, true);
//        }
//        Directory.CreateDirectory(outPath);
//        foreach (var v in Directory.GetFiles(inputPath, "*.xlsx", SearchOption.AllDirectories))
//        {
//            string fileName = Path.GetFileName(v);
//            if (!fileName.StartsWith(".~"))
//            {
//                Debug.Log(v);
//                Do(v);
//            }
//        }

//        AssetDatabase.Refresh();
//        AssetDatabase.SaveAssets();
//    }

//    static void Do(string filePath)
//    {
//       // try
//        {
//            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
//            {
//                using (var reader = ExcelReaderFactory.CreateReader(stream))
//                {
//                    do
//                    {
//                        while (reader.Read())
//                        {

//                        }
//                    } while (reader.NextResult());

//                    System.Data.DataSet result = reader.AsDataSet();
//                    ParseDataSet(filePath, result.Tables[0]);
//                }
//            }
//        }
//       //catch(Exception e)
//        //{
//        //    Debug.LogError(e.Message + "  " + e.StackTrace);
//        //    Debug.LogError($"{filePath}");
//        //}
//    }

//    static void ParseDataSet(string filePath, DataTable result)
//    {
//        ParseColumnType(result);
//        ParseData(filePath, result);
//    }

//    static void ParseColumnType(DataTable mTable)
//    {
//        mListColumnTypeInfo = new List<string>();
//        mListValidColumn = new List<int>();
        
//        for (int j = 0; j < mTable.Columns.Count; j++)
//        {
//            string fieldName = mTable.Rows[0][j].ToString();
//            string typeName = mTable.Rows[1][j].ToString();
//            string desName = mTable.Rows[2][j].ToString();

//            mListColumnTypeInfo.Add(fieldName.Trim());
//            mListColumnTypeInfo.Add(desName.Trim());
//            mListColumnTypeInfo.Add(typeName.Trim());
//        }
//    }

//    static bool CheckValidColumn(string fieldName, string typeName)
//    {
//        if(string.IsNullOrWhiteSpace(fieldName) || string.IsNullOrWhiteSpace(typeName))
//        {
//            return false;
//        }

//        if (typeName == "int" || typeName == "string" || typeName == "float" ||
//            typeName == "int{}" || typeName == "string{}" || typeName == "float{}")
//        {
//            return true;
//        }

//        return false;
//    }

//    static bool CheckValidRow(string value)
//    {
//        return !string.IsNullOrWhiteSpace(value);
//    }

//    static void ParseData(string filePath, DataTable mTable)
//    {
//        string fileName = Path.GetFileNameWithoutExtension(filePath);
//        string LuaFileName = fileName;
//        if (bRemovePrefix4)
//        {
//            if (LuaFileName.Length > 4)
//            {
//                LuaFileName = LuaFileName.Substring(4, LuaFileName.Length - 4);
//            }
//        }

//        LuaFileName = "Config_" + LuaFileName;
//        string outStr = "local " + LuaFileName + " = {\n";

//        for (int j = 3; j < mTable.Rows.Count; j++)
//        {
//            if(!CheckValidRow(mTable.Rows[j][0].ToString()))
//            {
//                break;
//            }

//            int nRowIdnex = j - 2;
//            outStr += "\t["+ nRowIdnex + "]={";
//            for (int k = 0; k < mTable.Columns.Count; k++)
//            {
//                string value = mTable.Rows[j][k].ToString();

//                string fieldName = mListColumnTypeInfo[k * 3 + 0];
//                string desName = mListColumnTypeInfo[k * 3 + 1];
//                string typeName = mListColumnTypeInfo[k * 3 + 2];

//                if (!CheckValidColumn(fieldName, typeName))
//                {
//                    break;
//                }

//                if (typeName == "float")
//                {
//                    float fValue = float.Parse(value);
//                    outStr += fieldName + " = " + fValue;
//                }
//                else if (typeName == "int")
//                {
//                    outStr += fieldName + " = " + ParseNumberValue(value, fileName, j, k);
//                }
//                else if (typeName == "string")
//                {
//                    string strValue = value;
//                    outStr += fieldName + " = \"" + strValue + "\"";
//                }
//                else if (typeName.EndsWith("{}"))
//                {
//                    string subtypeName = typeName.Substring(0, typeName.Length - 2);

//                    string strValue = value;
//                    if (strValue.StartsWith("{", StringComparison.Ordinal))
//                    {
//                        strValue = strValue.Substring(1, strValue.Length - 1);
//                    }

//                    if (strValue.EndsWith("}", StringComparison.Ordinal))
//                    {
//                        strValue = strValue.Substring(0, strValue.Length - 1);
//                    }

//                    string[] words = strValue.Split(',');

//                    outStr += fieldName + " = {";
//                    for (int n = 0; n < words.Length; n++)
//                    {
//                        string tempvalue = words[n];
//                        if (subtypeName == "float")
//                        {
//                            float fValue = float.Parse(tempvalue);
//                            if (n == 0)
//                            {
//                                outStr += fValue;
//                            }
//                            else
//                            {
//                                outStr += ", " + fValue;
//                            }
//                        }
//                        else if (subtypeName == "int")
//                        {
//                            var valueResult = ParseNumberValue(tempvalue, fileName, j, k);
//                            if (n == 0)
//                            {
//                                outStr += valueResult;
//                            }
//                            else
//                            {
//                                outStr += ", " + valueResult;
//                            }
//                        }
//                        else if (subtypeName == "string")
//                        {
//                            if (n == 0)
//                            {
//                                outStr += tempvalue;
//                            }
//                            else
//                            {
//                                outStr += ", " + tempvalue;
//                            }
//                        }
//                        else
//                        {
//                            Debug.LogError($"类型错误:{subtypeName},  {fileName}: [{j}, {k}]:{value}");
//                            Debug.Assert(false);
//                        }
//                    }
//                    outStr += "}";
//                }
//                else
//                {
//                    Debug.LogError($"类型错误:{typeName},  {fileName}: [{j}, {k}]:{value}");
//                    break;
//                }

//                if (k < mTable.Columns.Count - 1)
//                {
//                    outStr += ",\t";
//                }
//            }

//            outStr += "},\n";
//        }

//        outStr += "}\n\n";
        
//        outStr += "return " + LuaFileName + "\n";
//        String outFileName = outPath + LuaFileName + ".lua";
//        File.WriteAllText(outFileName, outStr, Encoding.UTF8);
//    }

//    private static string ParseNumberValue(string value, string fileName, int nRow, int nColumn)
//    {
//        string outStr = "0";
//        try
//        {
//            long nValue;
//            nValue = long.Parse(value, System.Globalization.NumberStyles.AllowDecimalPoint, null);
//            outStr = "" +nValue;
//        }
//        catch (Exception e)
//        {
//            Debug.LogWarning(e.Message + " | " + e.StackTrace);
//            Debug.LogWarning($"{fileName}: [{nRow}, {nColumn}]:{value}");
//        }

//        try
//        {
//            Decimal dData = 0.0M;
//            if (value.Contains("E"))
//            {
//                dData = Convert.ToDecimal(Decimal.Parse(value, System.Globalization.NumberStyles.Float));
//            }
//            else
//            {
//                dData = Convert.ToDecimal(value);
//            }

//            outStr = "" +dData;
//        }
//        catch (Exception e)
//        {
//            Debug.LogWarning(e.Message + " | " + e.StackTrace);
//            Debug.LogWarning($"{fileName}: [{nRow}, {nColumn}]:{value}");
//        }

//        return outStr;
//    }
//}
