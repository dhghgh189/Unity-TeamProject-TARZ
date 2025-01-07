#if UNITY_EDITOR
//using UnityEditor;
//using UnityEngine;

//[CustomPropertyDrawer(typeof(ShowFlagsAttribute))]
//public class ShowFlags : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        ShowFlagsAttribute showFlags = (ShowFlagsAttribute)attribute;
//        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showFlags.ConditionField);

//        if(conditionProperty.intValue == 0)
//        {
//            // 경고 메시지 출력
//            position.height = EditorGUIUtility.singleLineHeight * 2;
//            EditorGUI.HelpBox(position, $"스킬의 종류 선택은 필수입니다.", MessageType.Error);
//        }
//        // 조건 확인: 조건 Enum의 비트가 설정되어 있는지 확인
//        else if (conditionProperty != null && (conditionProperty.intValue & showFlags.EnumValueIndex) != 0)
//        {
//            EditorGUI.PropertyField(position, property, label, true); // 필드 표시
//        }
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        ShowFlagsAttribute showFlags = (ShowFlagsAttribute)attribute;
//        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showFlags.ConditionField);

//        // 조건 확인: 조건 Enum의 비트가 설정되어 있는지 확인
//        if (conditionProperty != null && (conditionProperty.intValue & showFlags.EnumValueIndex) != 0)
//        {
//            return EditorGUI.GetPropertyHeight(property, label);
//        }

//        return EditorGUIUtility.singleLineHeight * 2;
//    }
//}

//public class ShowFlagsAttribute : PropertyAttribute
//{
//    public string ConditionField; // 조건 필드 이름
//    public int EnumValueIndex;    // 조건에 사용할 Enum 값

//    public ShowFlagsAttribute(int enumValueIndex, string conditionField)
//    {
//        this.EnumValueIndex = enumValueIndex;
//        this.ConditionField = conditionField;
//    }
//}
#endif
