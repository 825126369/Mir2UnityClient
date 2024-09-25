//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine;

//[CustomEditor(typeof(MinePosGenerate)), CanEditMultipleObjects]
//public class MinePosGenerateEditor : Editor
//{
//	private MinePosGenerate mTarget;
//	private static int tab=0;

//	private void OnEnable()
//	{
//		mTarget = target as MinePosGenerate;
//	}

//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();
//		DrawInspectorGUI();
//		serializedObject.ApplyModifiedProperties();
//	}

//	protected void DrawInspectorGUI()
//	{
//		base.DrawDefaultInspector();
//		DrawMyInspector();
//	}

//	private void DrawMyInspector()
//	{
//		if (GUILayout.Button("Build"))
//		{
//			mTarget.GenerateAll();
//		}

//		if (GUILayout.Button("Clear"))
//		{
//			mTarget.Clear();
//		}

//		EditorGUILayout.Space();
//		tab = GUILayout.Toolbar(tab, new string[] { "Select", "Shape" });
//		if (tab == 0)
//		{

//		}
//		else if (tab == 1)
//		{
//			for (int i = 0; i < mTarget.m_RowCount; i++)
//			{
//				EditorGUILayout.BeginHorizontal();
//				for (int j = 0; j < mTarget.m_ColumnCount; j++)
//				{
//					int nIndex = i * mTarget.m_ColumnCount + j;
//					bool bIndexOk = mTarget.mList.Count > nIndex && mTarget.mList[nIndex] != null;
//					bool bToggle = bIndexOk && mTarget.mList[nIndex].gameObject.activeSelf;
//					bool bToggle1 = EditorGUILayout.Toggle(bToggle);
//					if (bToggle1 != bToggle)
//					{
//						if (bIndexOk)
//						{
//							var go = mTarget.mList[nIndex].gameObject;
//							go.SetActive(bToggle1);
//							EditorUtility.SetDirty(go);
//						}
//					}
//				}
//				EditorGUILayout.EndHorizontal();
//			}

//			if (GUILayout.Button("Save"))
//			{
//				AssetDatabase.SaveAssets();
//				AssetDatabase.Refresh();
//				EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

//			}
//		}
//	}

//}