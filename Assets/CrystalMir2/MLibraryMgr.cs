using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mir2
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

        public MImage GetMapImage(int nMapLibIndex, int nIndex)
        {
            MLibrarys.InitLibraries();
            var mLibrary = MLibrarys.MapLibs[nMapLibIndex];
            mLibrary.CheckImage(nIndex);
            return mLibrary.GetImage(nIndex);
        }

        public MImage GetImage(string url)
        {
            int nIndex = -1;
            string libPath = null;
            GetLibTexturePath(url, out libPath, out nIndex);
            Debug.Log("libPath: " + libPath + " | " + nIndex);

            var mLib = AddOrGet(libPath);
            Debug.Assert(mLib != null, libPath);
            mLib.CheckImage(nIndex);
            return mLib.GetImage(nIndex);
        }

        public MImage GetImage(MLibrary mLib, int nIndex)
        {
            mLib.CheckImage(nIndex);
            return mLib.GetImage(nIndex);
        }

        public void GetLibTexturePath(string url, out string libPath, out int nIndex)
        {
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;
            nIndex = int.Parse(Path.GetFileNameWithoutExtension(path));

            libPath = Path.GetDirectoryName(path) + ".Lib";
            int nlibPrefixIndex = libPath.LastIndexOf("Data" + Path.DirectorySeparatorChar);
            libPath = Path.Combine(MLibrarys.RootDir, libPath.Substring(nlibPrefixIndex));
        }

        public string GetTextureName(string url)
        {
            int nIndex = -1;
            string libPath = null;
            GetLibTexturePath(url, out libPath, out nIndex);
            if (!string.IsNullOrWhiteSpace(libPath))
            {
                int nIndex2 = libPath.IndexOf("Data");
                return libPath.Substring(nIndex2) + nIndex;
            }
            return string.Empty;
        }

        public string GetTextureName(string prefix, int nLibIndex, int nImageIndex)
        {
            return $"{prefix}_{nLibIndex}_{nImageIndex}";
        }

        public static void SetTextureParam(Texture2D mTexture)
        {
            if (mTexture != null)
            {
                mTexture.filterMode = FilterMode.Point;
                mTexture.wrapMode = TextureWrapMode.Clamp;
                mTexture.Apply();
            }
        }

        public static Sprite CreateSprite(Texture2D mTexture)
        {
            SetTextureParam(mTexture);

            if (mTexture == null) return null;
            var mTargetSprite = Sprite.Create(
                mTexture, 
                new Rect(0, 0, mTexture.width, mTexture.height), 
                new Vector2(0, 1), 
                1, 
                0, 
                SpriteMeshType.FullRect
            );

            mTargetSprite.name = mTexture.name;
            return mTargetSprite;
        }

    }
}
