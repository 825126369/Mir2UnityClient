using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.U2D;

[CustomEditor(typeof(CommonResSerialization)), CanEditMultipleObjects]
public class CommonResSerializationEditor : Editor
{
	private CommonResSerialization mTarget;
	private void OnEnable()
	{
		mTarget = target as CommonResSerialization;
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
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Asset Folder: ");
		mTarget.mResFolder = EditorGUILayout.TextField(mTarget.mResFolder);
		EditorGUILayout.EndHorizontal();
		
		if(string.IsNullOrWhiteSpace(mTarget.mResFolder))
		{
			mTarget.mResFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(mTarget));
		}

		if (GUILayout.Button("一键绑定 GameObject"))
		{
			foreach (var v in Directory.GetFiles(mTarget.mResFolder, "*", SearchOption.AllDirectories))
			{
				if (v.EndsWith(".prefab"))
				{
					var mResObj = AssetDatabase.LoadAssetAtPath<GameObject>(v);
					if (mTarget.m_PrefabList.IndexOf(mResObj) == -1)
					{
						mTarget.m_PrefabList.Add(mResObj);
					}
				}
            }

			EditorUtility.SetDirty(mTarget);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

        if (GUILayout.Button("一键绑定 SpriteAtlas"))
        {
            foreach (var v in Directory.GetFiles(mTarget.mResFolder, "*", SearchOption.AllDirectories))
            {
                if (v.EndsWith(".spriteatlasv2"))
                {
                    var mResObj = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(v);
					if (mTarget.m_AtlasList.IndexOf(mResObj) == -1)
					{
						mTarget.m_AtlasList.Add(mResObj);
					}
                }
            }

            EditorUtility.SetDirty(mTarget);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("一键绑定 Mp3"))
        {
            foreach (var v in Directory.GetFiles(mTarget.mResFolder, "*", SearchOption.AllDirectories))
            {
                if (v.EndsWith(".mp3"))
                {
                    var resObj = AssetDatabase.LoadAssetAtPath<AudioClip>(v);
                    if (mTarget.m_AudoClipList.IndexOf(resObj) == -1)
                    {
                        mTarget.m_AudoClipList.Add(resObj);
                    }
                }
            }

            EditorUtility.SetDirty(mTarget);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("一键绑定 Sprite"))
        {
            foreach (var v in Directory.GetFiles(mTarget.mResFolder, "*", SearchOption.AllDirectories))
            {
                if (v.EndsWith(".png") || v.EndsWith(".jpg"))
                {
                    var resObj = AssetDatabase.LoadAssetAtPath<Sprite>(v);
					if (mTarget.m_SpriteList.IndexOf(resObj) == -1)
					{
						mTarget.m_SpriteList.Add(resObj);
					}
                }
            }

            EditorUtility.SetDirty(mTarget);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        if (GUILayout.Button("一键绑定 Text/Json"))
        {
            foreach (var v in Directory.GetFiles(mTarget.mResFolder, "*", SearchOption.AllDirectories))
            {
                if (v.EndsWith(".txt") || v.EndsWith(".json"))
                {
                    var resObj = AssetDatabase.LoadAssetAtPath<TextAsset>(v);
                    if (mTarget.m_TextAssetList.IndexOf(resObj) == -1)
                    {
                        mTarget.m_TextAssetList.Add(resObj);
                    }
                }
            }

            EditorUtility.SetDirty(mTarget);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }

}