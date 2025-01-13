using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static BagSkillEnum;

/// <summary>
/// 플레이어의 가방 스킬을 사용하게 해주는 핸들러
/// 가방 스킬 매니저에서 사용 여부를 가져오면 해당 스킬을 사용한다
/// 그런데 어떻게 사용할 수 있냐?
/// </summary>
public class BagSkillHandler : MonoBehaviour
{
    [Inject] BagSkillManager manager;
    [Inject] BagSkillContainerSO container;
    [SerializeField] PlayerController player;
    [SerializeField] LinkedList<BaseBagState> actList;
    [SerializeField] int selectIndex;
    private LinkedListNode<BaseBagState> curNode;                      // 현재 동작
    private bool isEnd;

    private Dictionary<BagIndexKey, BagSkill> dic;

    [Header("JunkFist")]
    public JunkFistObject[] fists;

    #region 프로퍼티
    public bool ActionEnd { get => isEnd; set => isEnd = value; }       // 가방 스킬이 끝났는지 확인하는 변수
    public LinkedListNode<BaseBagState> CurNode { get { return curNode; } }                        // 현재 행동 노드
    #endregion

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        selectIndex = -1;
    }

    private void Start()
    {
        dic = new Dictionary<BagIndexKey, BagSkill>()
        {
            { BagIndexKey.JunkFist, new BagJunkFistSkill(player, container) },
            { BagIndexKey.ScrapBurst, new BagScrapBurstSkill(player, container) },
        };
        manager.Owner = player;

        LoadBagSkill();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            AddBagSkill(BagIndexKey.JunkFist, 0);
            AddBagSkill(BagIndexKey.ScrapBurst, 1);
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            Debug.Log($"가방 스킬 0번 사용! 성공 여부 : {UseBagSkill(0)}");
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            Debug.Log($"가방 스킬 1번 사용! 성공 여부 : {UseBagSkill(1)}");
        }
    }

    public void AddBagSkill(BagIndexKey key, int index)
    {
        // BagSkillManager -> key로 Dictionary에 있는 new 이 친구를 찾아서 index에 넣는다
        if (dic.TryGetValue(key, out var bagAct))
        {
            manager.AddSkill(bagAct, index);
        }
        else { Debug.Log($"가방 딕셔너리에 {key}가 없습니다!"); return; }
    }


    // 가방 스킬을 불러오는 부분
    public void LoadBagSkill()
    {
        for (int i = 0; i < manager.SaveBagSkillArray.Length; i++)
        {
            if (manager.SaveBagSkillArray[i].Item1 == -1) continue;

            if (dic.TryGetValue(manager.SaveBagSkillArray[i].Item2, out var value))
            {
                value.CurGauge = manager.SaveBagSkillArray[i].Item1;
                manager.AddSkill(value, i);
            }
        }
    }

    /// <summary>
    /// 가방 스킬 사용 가능 여부
    /// </summary>
    /// <param name="index">사용을 요청한 슬롯의 번호</param>
    /// <returns>사용 가능 여부</returns>
    public bool UseBagSkill(int index)
    {
        if (manager.IsCanUse(index))
        {
            Debug.Log("사용 가능");
            // 여기서 게이지 감소
            // 상태에 시작
            actList = manager.SkillArray[index].Acts;
            curNode = actList.First;
            // 상태 넘겨주기
            player.ChangeState(EState.BagUse);
            return true;
        }
        else
        {
            Debug.Log($"{index} 슬롯의 가방 스킬 사용 불가능!");
            return false;
        }
    }

    public void NextStep(int nextIndex = 1)
    {
        if (nextIndex <= 0) { Debug.LogError($"{selectIndex}의 스킬에서 애니메이션이 잘못된 숫자 {nextIndex}를 넘겨줬다!."); return; }

        for (int i = 0; i < nextIndex; i++)
        {
            if (curNode.Next == null)
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

    /// <summary>
    /// 실직적으로 애니메이션의 이벤트를 실행시켜주는 함수
    /// </summary>
    public void OnAction()
    {
        if (selectIndex < 0) return;

        curNode.Value.OnAction();
    }

}
