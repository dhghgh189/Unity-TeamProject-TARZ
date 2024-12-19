using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SkillEnum;

//[CustomEditor(typeof(BaseSkillSO))]
public class BaseSkillSOEditor : Editor
{
    private List<bool> effectFoldoutStates = new List<bool>();
    private List<bool> passiveFoldoutStates = new List<bool>();

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BaseSkillSO targetScript = (BaseSkillSO)target;

        // 기본 속성 표시
        EditorGUILayout.LabelField("Skill Info", EditorStyles.boldLabel);
        targetScript.Name = EditorGUILayout.TextField("Name", targetScript.Name);
        targetScript.Description = EditorGUILayout.TextField("Description", targetScript.Description);
        targetScript.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", targetScript.Icon, typeof(Sprite), false);
        targetScript.Timing = (ActTimingType)EditorGUILayout.EnumPopup("Timing", targetScript.Timing);

        EditorGUILayout.Space(10);

        // Effect_Active 리스트 표시
        EditorGUILayout.LabelField("Effect Active", EditorStyles.boldLabel);
        DrawSkillList(targetScript.EffectSkills, effectFoldoutStates, typeof(ActiveSkillSO));

        EditorGUILayout.Space(10);

        // Effect_Passive 리스트 표시
        EditorGUILayout.LabelField("Effect Passive", EditorStyles.boldLabel);
        DrawSkillList(targetScript.PassiveSkills, passiveFoldoutStates, typeof(PassiveSkillSO));

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSkillList<T>(List<T> skillList, List<bool> foldoutStates, System.Type type) where T : ScriptableObject
    {
        if (skillList == null) { return; }  

        // 리스트 항목 그리기
        for (int i = 0; i < skillList.Count; i++)
        {
            if (foldoutStates.Count <= i)
                foldoutStates.Add(false);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"E {i}");

            if (foldoutStates[i])
            {
                if (skillList[i] != null)
                {
                    SerializedObject serializedSO = new SerializedObject(skillList[i]);
                    serializedSO.Update();

                    SerializedProperty prop = serializedSO.GetIterator();
                    prop.NextVisible(true);

                    while (prop.NextVisible(false))
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }

                    serializedSO.ApplyModifiedProperties();
                }
                else
                {
                    EditorGUILayout.HelpBox("No ScriptableObject assigned.", MessageType.Warning);
                }
            }

            // 삭제 버튼
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                skillList.RemoveAt(i);
                foldoutStates.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        // 항목 추가 버튼
        if (GUILayout.Button("Add New"))
        {
            T newSkill = ScriptableObject.CreateInstance(type) as T;

            string path = $"Assets/New_{type.Name}_{skillList.Count}.asset";
            AssetDatabase.CreateAsset(newSkill, path);
            AssetDatabase.SaveAssets();

            skillList.Add(newSkill);
            foldoutStates.Add(true);
        }
    }
}

