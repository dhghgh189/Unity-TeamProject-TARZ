using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static UnityEngine.UI.GridLayoutGroup;

/// <summary>
/// 플레이어의 마나 스킬을 사용하게 해주는 핸들러
/// </summary>
public class ManaSkillHandler : MonoBehaviour
{
    [Inject] private StatModel stat;
    [Header("Init")]
    [SerializeField] public List<ManaSkillDataSO> skillData;
    [SerializeField] IManaSkill[] manaSkill;                    // 마나 스킬의 슬롯
    [Header("ManaSkill")]
    private Dictionary<string, ManaSkillDataSO> skillDataDictionary;    // 스킬 데이터를 담아두는 딕셔너리
    [SerializeField] LinkedList<BaseManaState> actList;         // 해당 마나스킬의 진행 순서
    [SerializeField] int selectIndex;                           // 사용할 마나스킬
    private LinkedListNode<BaseManaState> curNode;              // 현재 동작
    private bool isEnd;                                         // 동작이 끝이 났는지 확인

    public bool ActionEnd { get => isEnd; set => isEnd = value; }
    public LinkedList<BaseManaState> ActList { get { return actList; } set { actList = value; } }
    public LinkedListNode<BaseManaState> CurNode { get { return curNode; }}

    public void Awake()
    {
        skillDataDictionary = new Dictionary<string, ManaSkillDataSO>();

        foreach(ManaSkillDataSO data in skillData)
        {
            skillDataDictionary.Add( data.skillName, Instantiate(data));
        }
    }

    private void Start()
    {
        selectIndex = -1;
        manaSkill = new IManaSkill[4];
        manaSkill[0] = new ManaRushSkill(GetComponent<PlayerController>());
        manaSkill[1] = new ManaThrowCarSkill(GetComponent<PlayerController>());
        manaSkill[2] = null;
        manaSkill[3] = null;
    }

    public ManaSkillDataSO GetData(string KeyName)
    {
        if (skillDataDictionary.TryGetValue(KeyName, out ManaSkillDataSO data))
        {
            return data; 
        }
        else
        {
            throw new Exception("데이터 해당하는 이름의 데이터 셋이 없습니다!");
        }
    }

    /// <summary>
    /// 마나 스킬을 사용할 때 호출하는 함수
    /// </summary>
    /// <param name="index">요청한 마나 스킬의 번호</param>
    public bool UseManaSkill(int index)
    {
        // 마나 스킬의 조건이 충족하는지 확인
        if (stat.CurrentMp >= 0 * index)
        {
            // 사용하는 마나 만큼 차감
            stat.CurrentMp -= 100 * index;
            // Todo -> 해당하는 스킬 사용
            selectIndex = index;
            manaSkill[selectIndex]?.SetInit(this);
            curNode = actList.First;
            return true;
        }
        else
        {
            // Todo -> 사용이 불가능하다고 UI 보여주기(Popup)
            Debug.Log("<color=Red>마나가 부족하여 사용이 불가능합니다</color>");
            return false;
        }
    }

    /// <summary>
    /// 애니메이션에서 다음 스탭으로 넘어갈 때 요청
    /// </summary>
    /// <param name="nextIndex">넘어갈 횟수, 기본은 한번 -> 여러번 요청할 수 있음</param>
    public void NextStep(int nextIndex = 1)
    {
        if(nextIndex <= 0) { Debug.LogError($"{selectIndex}의 스킬에서 애니메이션이 잘못된 숫자 {nextIndex}를 넘겨줬다!."); return; }
        
        for(int i = 0; i < nextIndex; i++)
        {
            if(curNode.Next == null)
            {
                Debug.Log("end");
                isEnd = true;
                return;
            }
            else
            {
                curNode = curNode.Next;
            }
        }
        curNode.Value.OnEnter();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (selectIndex < 0) return;
        
        if(curNode.Value.OnCollisionAction(other))
        {
            NextStep();
        }
    }

    /// <summary>
    /// 스킬이 끝나 행동을 초기화하는 작업
    /// </summary>
    public void End()
    {
        isEnd = false;
        curNode = null;
        actList = null;
        selectIndex = -1;
    }

    public void OnAction()
    {
        curNode.Value.OnAction();
    }

}
