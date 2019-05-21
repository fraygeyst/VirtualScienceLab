using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (VRNode3DSynchronization))]
public class VRNode3DSynchronizationEditor : Editor
{
    private static bool showAdvanced = false; //declare outside of function

    public override void OnInspectorGUI()
    {
        var targetScript = target as VRNode3DSynchronization;
        EditorGUI.indentLevel++;
        SerializedProperty prop = serializedObject.FindProperty("m_Script");
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
        showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced");
        if (showAdvanced)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NodeName"), new GUIContent("vrNode3D name"));
        }
    }
}
