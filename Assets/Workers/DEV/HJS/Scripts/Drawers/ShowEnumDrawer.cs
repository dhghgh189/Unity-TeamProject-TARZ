using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowEnumAttribute))]
public class ShowEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowEnumAttribute showEnum = (ShowEnumAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showEnum.ConditionField);

        if (conditionProperty != null &&  showEnum.EnumValueIndex == conditionProperty.enumValueIndex)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowEnumAttribute showEnum = (ShowEnumAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showEnum.ConditionField);

        if (conditionProperty != null && showEnum.EnumValueIndex == conditionProperty.enumValueIndex)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        return 0f;
    }
}

public class ShowEnumAttribute : PropertyAttribute
{
    public string ConditionField;
    public int EnumValueIndex;

    public ShowEnumAttribute(int enumValueIndex, string conditionField)
    {
        this.EnumValueIndex = enumValueIndex;
        this.ConditionField = conditionField;
    }

}
