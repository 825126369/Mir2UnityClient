using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FontReplace : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().font = Resources.Load<Font>("ARLRDBD");
    }
}
