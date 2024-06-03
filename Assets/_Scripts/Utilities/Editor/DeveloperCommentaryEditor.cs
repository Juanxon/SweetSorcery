#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeveloperCommentary))]
public class DeveloperCommentaryEditor : Editor
{
    private GUIStyle textAreaStyle;
    private SerializedProperty textLengthProperty;
    private SerializedProperty fontSizeProperty;

    private void OnEnable()
    {

        textLengthProperty = serializedObject.FindProperty("textLength");
        fontSizeProperty = serializedObject.FindProperty("fontSize");
    }

    public override void OnInspectorGUI()
    {
        textLengthProperty = serializedObject.FindProperty("textLength");
        serializedObject.Update();

        DeveloperCommentary commentary = (DeveloperCommentary)target;

        EditorGUI.BeginChangeCheck();

        // Display the DeveloperDescription field as a text area with custom style
        commentary.DeveloperDescription = EditorGUILayout.TextArea(
            commentary.DeveloperDescription, GUILayout.Height(commentary.textLength));

        EditorGUILayout.PropertyField(textLengthProperty, new GUIContent("Lenght"));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
        
    }
}
#endif