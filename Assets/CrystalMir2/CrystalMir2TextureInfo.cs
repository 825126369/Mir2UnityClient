using System;
using System.IO;
using UnityEngine;

namespace Mir2
{
    public class CrystalMir2TextureInfo : MonoBehaviour
    {
        private string url;
        [SerializeField]
        public MImage mInfo;

        public void SetUrl(string url)
        {
            this.url = url;

            GetTextureInfo();
        }

        public void GetTextureInfo()
        {
            Debug.Log("url: " + url);
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;
            Debug.Log("path: " + path);

            int nIndex = int.Parse(Path.GetFileNameWithoutExtension(path));

            string libPath = Path.GetDirectoryName(path) + ".Lib";
            int nlibPrefixIndex = libPath.LastIndexOf("Data" + Path.DirectorySeparatorChar);
            libPath = Path.Combine(MLibrarys.RootDir, libPath.Substring(nlibPrefixIndex));

            Debug.Log("libPath: " + libPath);
            var mLib = MLibraryMgr.Instance.AddOrGet(libPath);
            mLib.CheckImage(nIndex);
            mInfo = mLib.GetImage(nIndex);

            SetOffset();
        }

        private void SetOffset()
        {
            transform.localPosition = Vector3.zero + new Vector3(mInfo.X, -mInfo.Y);
        }
    }
}
