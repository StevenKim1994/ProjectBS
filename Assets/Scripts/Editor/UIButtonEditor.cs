using System.Reflection;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using BS.UI;

[CustomEditor(typeof(UIButton), true)]
[CanEditMultipleObjects]
public class UIButtonEditor : SelectableEditor
{
    SerializedProperty _pressEventIntervalProp;
    SerializedProperty _buttonSelectSoundProp;

    protected override void OnEnable()
    {
        base.OnEnable();

        _pressEventIntervalProp = serializedObject.FindProperty("_pressEventInterval");
        _buttonSelectSoundProp = serializedObject.FindProperty("_buttonSelectSound");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("BS UIButton", EditorStyles.boldLabel);

        if (_pressEventIntervalProp != null)
            EditorGUILayout.PropertyField(_pressEventIntervalProp, new GUIContent("Press Event Interval (s)"));
        if (_buttonSelectSoundProp != null)
            EditorGUILayout.PropertyField(_buttonSelectSoundProp, new GUIContent("Button Select Sound"));

        using (new EditorGUI.DisabledScope(true))
        {
            var uiButton = (UIButton)target;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Runtime Info", EditorStyles.boldLabel);

            bool isPressed = false;
            try
            {
                var fi = typeof(UIButton).GetField("_isPressed", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fi != null)
                {
                    object val = fi.GetValue(uiButton);
                    if (val is bool b) isPressed = b;
                }
            }
            catch { }

            EditorGUILayout.Toggle("Is Pressed", isPressed);
            EditorGUILayout.FloatField("Current Press Duration (unscaled)", uiButton.CurrentPressDurationUnscaled);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public override bool RequiresConstantRepaint()
    {
        return EditorApplication.isPlaying;
    }
}
