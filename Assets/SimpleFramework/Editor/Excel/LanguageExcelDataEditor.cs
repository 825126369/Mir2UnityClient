using System;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;

public class LanguageExcelDataEditor : MonoBehaviour
{
    const string inputPath = "Assets/Excel/i18n.xlsx";
    const string outPath = "Assets/Excel/Out/i18n.json";
    const string copyPath = "Assets/ResourceABs/ThemeSolitaire/i18n.json";

    [MenuItem("Tools/ i18n Gen ")]
    static void Do()
    {
        {
            using (var stream = File.Open(inputPath, FileMode.Open, FileAccess.Read))
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
                    ParseDataSet(result.Tables[0]);
                }
            }
        }
    }

    static void ParseDataSet(DataTable result)
    {
        List<List<string>> mDic = new List<List<string>>();
        for (int j = 3; j < result.Rows.Count; j++)
        {
            List<string> mList = new List<string>();
            for(int i = 0; i< result.Columns.Count; i++)
            {
                string value = result.Rows[j][i].ToString();
                mList.Add(value);
            }
            mDic.Add(mList);
        }

        string json = JsonTool.ToJson(mDic);
        File.WriteAllText(outPath, json);
        File.Copy(outPath, copyPath, true);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
