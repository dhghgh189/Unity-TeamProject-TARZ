using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

[CreateAssetMenu(menuName = "Scriptables/Base_ActiveSkill")]
public class ActiveSkillSO : ScriptableObject
{
    [Header("Skill_Defualt_Options")]
    [SerializeField] Target who;
    [SerializeField] RefeatType refeat;
    [SerializeField] ActConditionType when;

    [Header("SkillOptions")]
    public bool Create;

    [Space(2)]
    [Header("Settings")]
    [SerializeField, ShowIf("Create")] CreateSetting createSetting;

    #region 프로퍼티
    public Target Target => who;
    public RefeatType Refeat => refeat;
    public ActConditionType CollisionType => when;
    #endregion

    [Serializable]
    public class CreateSetting
    {
        public List<GameObject> CreateObject; // 생성 여부가 True일 때 -> 생성할 오브젝트 (ex. 폭발, 독구름)
    }
    public void Use(GameObject obj)
    {

        if (Create)
        {
            foreach (GameObject gameObject in createSetting.CreateObject)
            {
                Debug.Log(gameObject.name);
                GameObject game = Instantiate(gameObject, obj.transform.position + Vector3.up, Quaternion.identity);

                FloorSpawner flooring = game.GetComponent<FloorSpawner>();
                // 설치물이 장판이라면
                if (flooring is not null)
                {
                    flooring.SetTarget = obj.transform;
                    Debug.Log($"floor 부착! {obj.name}");
                }
                Debug.Log($"충돌한 {obj.name}의 위치에서 {game.name}을 생성하겠다!");
            }
        }
    }

}
