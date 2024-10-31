//#define Use
#if Use

using Newtonsoft.Json;
using Server.MirDatabase;

namespace Server.MirEnvir
{
    public static class AKParser
    {
        const string saveDir = "D:\\Me\\MyProject\\CrystalMir2\\DbInfo\\";
        public static void ParseEnvir(Envir mInfo)
        {
            string content = JsonConvert.SerializeObject(mInfo);
            File.WriteAllText(saveDir + "Envir.json", content);
        }

        public static void ParseAny(object mInfo)
        {
            string Name = string.Empty;
            if (mInfo.GetType().IsGenericType)
            {
                Name = mInfo.GetType().GetGenericArguments()[0].Name;
            }
            else
            {
                Name = mInfo.GetType().Name;
            }

            string content = JsonConvert.SerializeObject(mInfo);
            string outPath = Path.Combine(saveDir, Name + ".json");
            File.WriteAllText(outPath, content);
        }
    }
}

#endif
