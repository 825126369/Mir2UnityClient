using System.Collections.Generic;

namespace CrystalMir2
{
    public class MLibraryMgr:SingleTonMonoBehaviour<MLibraryMgr>
    {
        public Dictionary<string, MLibrary> mDic = new Dictionary<string, MLibrary>();
        public MLibrary AddOrGet(string path)
        {
            MLibrary mLibrary = null;
            if (!mDic.TryGetValue(path, out mLibrary))
            {
                mLibrary = new MLibrary(path);
                mDic.Add(path, mLibrary);
            }

            return mLibrary;
        }
    }
}
