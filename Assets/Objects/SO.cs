using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectIdAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
public class ScriptableObjectIdDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        if (string.IsNullOrEmpty(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
        }
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif

[System.Serializable]
public class SO : ScriptableObject
{
    [SerializeField]
    [ScriptableObjectId]
    protected string objectID;
    [SerializeField]
    protected string objectName;

    private void OnValidate()
    {
#if UNITY_EDITOR
        objectName = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    public string ObjectID { get { return objectID; } }
    public string ObjectName { get { return objectName; } }

}
