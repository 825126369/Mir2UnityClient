using UnityEditor;
using UnityEngine;

namespace Mir2Editor
{
    public class MirDBEditor : MonoBehaviour
    {
        [MenuItem("Mir2Editor/���� MirDB ���ݿ�")]
        public static void Do()
        {
            Envir.LoadDB();
            AKParser.ParseUnityAny(Envir.MapInfoList);
        }
    }
}


