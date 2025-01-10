using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// 가방 데이터
/// 가방 스킬을 담아놓는 매니저
/// 가방 스킬의 사용 여부를 확인한다
/// </summary>
public class BagSkillManager : MonoBehaviour
{
    /// <summary>
    /// 방을 클리어 했을 때 가방 스킬들에게 충전하라고 알려주는 이벤트
    /// </summary>
    [HideInInspector] public UnityEvent OnChargeEvent;

    private PlayerController owner;
    [HideInInspector] public PlayerController Owner { get { return owner; } set { owner = value; } }

    /// <summary>
    /// 가방 스킬을 담아놓는 배열
    /// </summary>
    private IBagAct[] skillArr;

    public IBagAct[] SkillArray { get { return skillArr; } }

    private void Awake()
    {
        OnChargeEvent = new UnityEvent();
        skillArr ??= new IBagAct[4];
    }

    private void Start()
    {
        SceneManager.sceneLoaded -= LoadedsceneEvent;
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    public bool IsCanUse(int index)
    {
        // 만약 skillArray가 없거나 비어있다 -> 스킬이 없다
        if (skillArr == null || skillArr[index] == null) return false;

        // 스킬이 있으면 해당 스킬의 조건에 부합하는지 확인
        return skillArr[index].IsCanUse();
    }

    /// <summary>
    /// 스킬을 장착하는 함수
    /// </summary>
    /// <param name="skillName">장착하려는 스킬의 이름</param>
    /// <param name="index">장착하려는 슬롯</param>
    public void AddSkill(IBagAct bagSkill, int index)
    {
        if (skillArr[index] is not null)
        {
            OnChargeEvent.RemoveListener(skillArr[index].Charge);
        }
        skillArr[index] = bagSkill;
        OnChargeEvent.AddListener(skillArr[index].Charge);
    }

    // TODO: 씬이 변경되었을 때 -> Manager안의 스킬들 순회
    // 그래서 버프와 같이 스탯이 증가한 애들을 다시 원복시켜주는 함수 실행
    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        // 혹시 모르는 예외상황 처리
        if (skillArr == null) return;

        // 장착한 마나 스킬의 초기화 함수를 돌아본다
        foreach(IBagAct act in skillArr)
        {
            if (act is null) continue;
   
            act.RetrunFeature();
        }
    }

    private void OnDestroy()
    {
        if(OnChargeEvent is not null) OnChargeEvent.RemoveAllListeners();
    }
}
