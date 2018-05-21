using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var flagSettings = (EnumFlagsAttribute)attribute;
        var targetEnum = (Enum)Enum.ToObject(fieldInfo.FieldType, property.intValue);

        var propName = flagSettings.name;
        if (string.IsNullOrEmpty(propName))
            propName = ObjectNames.NicifyVariableName(property.name);

        EditorGUI.BeginChangeCheck();
        EditorGUI.BeginProperty(position, label, property);

        var enumNew = EditorGUI.EnumFlagsField(position, propName, targetEnum);

        if (!property.hasMultipleDifferentValues || EditorGUI.EndChangeCheck())
            property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());

        EditorGUI.EndProperty();
    }
}