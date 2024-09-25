using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.U2D;

[CustomEditor(typeof(i18nItem)), CanEditMultipleObjects]
public class i18nItemEditor : Editor
{
    private i18nItem mTarget;
	private void OnEnable()
	{
		mTarget = target as i18nItem;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		DrawInspectorGUI();
		serializedObject.ApplyModifiedProperties();
	}

	protected void DrawInspectorGUI()
	{
		base.DrawDefaultInspector();
		DrawMyInspector();
	}
    
	private void DrawMyInspector()
	{
		if (GUILayout.Button("Build"))
		{
            mTarget.Do();
			EditorUtility.SetDirty(mTarget);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
    }

}
