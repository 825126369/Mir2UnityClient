using Mir2;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CrystalMir2TextureInfo)), CanEditMultipleObjects]
public class CrystalMir2TextureInfoEditor: Editor
{
    private CrystalMir2TextureInfo mTarget;
    private void OnEnable()
    {
        mTarget = target as CrystalMir2TextureInfo;
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
        if (GUILayout.Button("Get Info"))
        {
            mTarget.GetTextureInfo();
        }
    }
}
