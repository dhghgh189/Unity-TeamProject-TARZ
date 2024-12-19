using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SkillTable : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] List<Test_SkillSlot> slots;
    [SerializeField] bool isRandom;
    [Header("Skill_List")]
    [SerializeField] List<BaseSkillSO> _skills;
    [Header("Skill_Handler")]
    [SerializeField] PlayerSkillHandler _handler;



    private void Start()
    {
        foreach(Test_SkillSlot slot in slots)
        {
            slot.playerSkillHandler = _handler;
        }
    }

    // 역할
    public void GetSkill()
    {
        // 1. 랜덤으로 스킬 가져오기
        // 2. 스킬 핸들러에게 장착 요청하기
        if (CheckEmpty(out int index))
        {
            BaseSkillSO newSkill = (isRandom) ? Instantiate(_skills[Random.Range(0, _skills.Count)]) : Instantiate(_skills[index]);
            slots[index].SetSlot(newSkill);
            _handler.AddSkill(newSkill); 
        }
    }

    // 3. 스킬 슬롯이 꽉차면 더 이상 받기 못하기
    private bool CheckEmpty(out int index)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Empty)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

}
