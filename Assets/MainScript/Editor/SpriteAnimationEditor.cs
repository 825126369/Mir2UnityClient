//using System;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.U2D;

//[CustomEditor(typeof(AnimationSprite))]
//public class SpriteAnimationEditor : Editor
//{
//    private AnimationSprite mTarget;
//    private static int tab = 0;

//    private void OnEnable()
//    {
//        mTarget = target as AnimationSprite;
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        DrawInspectorGUI();
//        serializedObject.ApplyModifiedProperties();
//    }

//    protected void DrawInspectorGUI()
//    {
//        base.DrawDefaultInspector();
//    }
//}