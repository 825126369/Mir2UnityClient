using System;
using System.Collections.Generic;

namespace Mir2
{
    internal class ExcelTableMgr : Singleton<ExcelTableMgr>
    {
        public List<MapInfo> MapInfoList = new List<MapInfo>();
        public List<ItemInfo> mItemList = new List<ItemInfo>();

        public void Init()
        {
            ParseMapJson();
            ParseItemCsv();
            PrintTool.Log("ExcelTableMgr Init Finish");
        }

        private void ParseMapJson()
        {
            try
            {
                string json = ResCenter.Instance.mBundleGameAllRes.FindTextAsset("MapInfo").text;
                MapInfoList = JsonTool.FromJson<List<MapInfo>>(json);
            }
            catch (Exception e)
            {
                PrintTool.LogError(e);
            }

            PrintTool.Log("ExcelTableMgr Init Finish");
        }

        private void ParseItemCsv()
        {
            string content = ResCenter.Instance.mBundleGameAllRes.FindTextAsset("ItemInfo.csv").text;
            string[] lineList = content.Split(Environment.NewLine);
            string[] varList = lineList[0].Split(',');

            for (int i = 1; i < lineList.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lineList[i]))
                {
                    continue;
                }

                ItemInfo itemInfo = new ItemInfo();
                string[] valueList = lineList[i].Split(',');
                for (int j = 0; j < valueList.Length; j++)
                {
                    if (j >= varList.Length) continue;

                    string value = valueList[j];
                    var mFieldInfo = itemInfo.GetType().GetField(varList[j]);
                    if (mFieldInfo != null)
                    {
                        if (mFieldInfo.GetType() == typeof(int))
                        {
                            mFieldInfo.SetValue(itemInfo, int.Parse(value));
                        }
                        else if (mFieldInfo.GetType() == typeof(bool))
                        {
                            mFieldInfo.SetValue(itemInfo, int.Parse(value));
                        }
                        else if (mFieldInfo.GetType() == typeof(string))
                        {
                            mFieldInfo.SetValue(itemInfo, value);
                        }
                    }
                }
                mItemList.Add(itemInfo);
            }
        }
    }
}
