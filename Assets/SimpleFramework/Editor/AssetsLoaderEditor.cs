using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CustomEditor(typeof(AssetsLoader))]
public class AssetsLoaderEditor : Editor
{
	private AssetsLoader mTarget;
	private void OnEnable()
	{
		mTarget = target as AssetsLoader;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		base.DrawDefaultInspector();
		DrawCustomInspectorGUI();
		serializedObject.ApplyModifiedProperties();
	}

    private void DrawCustomInspectorGUI()
    {
        var mChildrenList1 = Get_m_resultToHandle();
        EditorGUILayout.LabelField("Inner Handle Count: " + mChildrenList1.Count);
        EditorGUILayout.Space();
        foreach (var v in mChildrenList1)
        {
            EditorGUILayout.BeginHorizontal();
            if (v.Key is UnityEngine.Object)
            {
                EditorGUILayout.LabelField(v.Key.GetType().Name);
            }
            else if (v.Key is IList<UnityEngine.Object>)
            {
                var Key2 = v.Key as IList<UnityEngine.Object>;
                EditorGUILayout.LabelField(v.Key.GetType().Name + " | " +Key2[0].GetType().Name + ": " + Key2.Count);
            }
            else if (v.Key is UnityEngine.ResourceManagement.ResourceProviders.SceneInstance)
            {
                EditorGUILayout.LabelField(v.Key.GetType().Name);
            }
            else
            {
                Debug.LogError("Error: " + v.Key.GetType().Name);
            }

            EditorGUILayout.LabelField(v.Value.GetType().Name);
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        var mChildrenList = Get_mAssetDic();
        EditorGUILayout.LabelField("Asset Count: " + mChildrenList.Count);
        EditorGUILayout.Space();
        foreach (var v in mChildrenList)
        {
            EditorGUILayout.ObjectField(v.Key, v.Value, typeof(UnityEngine.Object), false);
        }
    }

    private Dictionary<string, UnityEngine.Object> Get_mAssetDic()
    {
        var mChildrenListFieldInfo = mTarget.GetType().GetField("mAssetDic", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);
        var mChildrenList = mChildrenListFieldInfo.GetValue(mTarget) as Dictionary<string, UnityEngine.Object>;
		return mChildrenList;
    }

    private Dictionary<object, AsyncOperationHandle> Get_m_resultToHandle()
    {
        var m_Addressables = typeof(Addressables).GetProperty("m_Addressables", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        var m_resultToHandle = m_Addressables.PropertyType.GetField("m_resultToHandle", BindingFlags.NonPublic | BindingFlags.Instance);
        var m_Addressables_Value = m_Addressables.GetValue(null);
        var m_resultToHandle_Value = m_resultToHandle.GetValue(m_Addressables_Value) as Dictionary<object, AsyncOperationHandle>;

        Debug.Assert(m_resultToHandle_Value != null, "m_resultToHandle_Value == null");
		return m_resultToHandle_Value;
    }
}