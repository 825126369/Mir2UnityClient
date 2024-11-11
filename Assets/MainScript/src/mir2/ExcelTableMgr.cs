using CrystalMir2;
using System;
using System.Collections.Generic;

namespace Mir2
{
    internal class ExcelTableMgr : Singleton<ExcelTableMgr>
    {
        public List<MapInfo> MapInfoList = new List<MapInfo>();

        public void Init()
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
    }
}
