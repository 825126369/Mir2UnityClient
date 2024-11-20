using Mir2;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMapMgr))]
public class TileMapMgrEditor : Editor
{
    private TileMapMgr mTarget;
    private void OnEnable()
    {
        mTarget = target as TileMapMgr;
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
        if (GUILayout.Button("Draw Map"))
        {
            mTarget.LoadMapTest();
        }
    }

}
