using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/SoundData/Player")]
public class PlayerSoundData : ScriptableObject
{
    [SerializeField] private Sound _sound;

    public Dictionary<EMultiActionType, AudioClip>[] ThrowCache;
    public Dictionary<EMultiActionType, AudioClip>[] ThrowHitsCache;

    // FootStep
    public AudioClip FootStep_Dirt { get { return _sound.FootStep_Dirt; } }
    public AudioClip FootStep_Concrete { get { return _sound.FootStep_Concrete; } }
    public AudioClip FootStep_Grass { get { return _sound.FootStep_Grass; } }
    public AudioClip FootStep_Wood { get { return _sound.FootStep_Wood; } }
    public AudioClip FootStep_Metal { get { return _sound.FootStep_Metal; } }
    
    // Damaged
    public AudioClip TakeDamage_Normal { get { return _sound.TakeDamage_Normal; } }
    public AudioClip TakeDamage_Danger { get { return _sound.TakeDamage_Danger; } }

    // States
    public AudioClip Jump { get { return _sound.Jump; } }
    public AudioClip Dash { get { return _sound.Dash; } }
    public AudioClip AirDash { get { return _sound.AirDash; } }
    public AudioClip Dead { get { return _sound.Dead; } }

    // Melee
    public AudioClip[] Melees { get { return _sound.Melees; } }
    public AudioClip[] MeleeHits { get { return _sound.MeleeHits; } }

    // Throw
    public ThrowSoundInfo[] Throws { get { return _sound.Throws; } }
    public ThrowSoundInfo[] ThrowHits { get { return _sound.ThrowHits; } }

    // 저스트 회피
    public AudioClip JustSuccess { get { return _sound.JustSuccess; } }
    public AudioClip CounterThrow { get { return _sound.CounterThrow; } }
    public AudioClip CounterThrowHit { get { return _sound.CounterThrowHit; } }
    public AudioClip EliteCounter { get { return _sound.EliteCounter; } }
    public AudioClip EliteCounterHit { get { return _sound.EliteCounterHit; } }

    // 경고음
    public AudioClip Alert { get { return _sound.Alert; } }
    public AudioClip StaminaZeroAlert { get { return _sound.StaminaZeroAlert; } }


    [Serializable]
    public struct Sound
    {
        [Header("SFX")]
        [Header("FootStep")]
        public AudioClip FootStep_Dirt;         // 발소리 (흙)
        public AudioClip FootStep_Concrete;     // 발소리 (콘크리트)
        public AudioClip FootStep_Grass;        // 발소리 (풀)
        public AudioClip FootStep_Wood;         // 발소리 (나무)
        public AudioClip FootStep_Metal;        // 발소리 (철)

        [Header("Damaged")]
        public AudioClip TakeDamage_Normal;     // 피격 (최대 체력의 50% 미만 데미지)
        public AudioClip TakeDamage_Danger;     // 피격 (최대 체력의 50% 이상 데미지)

        [Header("States")]
        public AudioClip Jump;                  // 점프
        public AudioClip Dash;                  // 대쉬
        public AudioClip AirDash;               // 공중에서 대쉬
        public AudioClip Dead;                  // 사망

        [Header("Melee (타수 별 사운드 순서 맞춰주세요)")]
        public AudioClip[] Melees;              // 근거리 공격 (1~3), 순서 정확히 맞출 것
        public AudioClip[] MeleeHits;           // 근거리 공격 히트 시 (1~3), 순서 정확히 맞출 것

        [Header("Throw (타수 별 사운드 순서 맞춰주세요)")]
        public ThrowSoundInfo[] Throws;         // 원거리 공격 (1~3), 순서 정확히 맞출 것
        public ThrowSoundInfo[] ThrowHits;      // 원거리 공격 히트 시 (1~3), 순서 정확히 맞출 것

        [Header("저스트 회피")]
        public AudioClip JustSuccess;           // 저스트 회피 성공 시
        public AudioClip CounterThrow;          // 노멀 반격 (던지기)
        public AudioClip CounterThrowHit;       // 노멀 반격 hit 시
        public AudioClip EliteCounter;          // 엘리트 이상 반격 시
        public AudioClip EliteCounterHit;       // 엘리트 이상 반격 hit 시
        
        [Header("경고음")]
        public AudioClip Alert;                 // 스테미나, 마나, 쿨타임 부족할 때 사용 시
        public AudioClip StaminaZeroAlert;                 // 스테미나, 마나, 쿨타임 부족할 때 사용 시
    }

    public void Init()
    {
        // 타수별 MultiAction의 Throw 클립을 캐싱하기 위한 딕셔너리 배열
        ThrowCache = new Dictionary<EMultiActionType, AudioClip>[Throws.Length];
        for (int i = 0; i < ThrowCache.Length; i++)
        {
            // 현재 타수에 MultiAction이 없으면 pass
            if (Throws[i].MultiActions.Length <= 0)
                continue;

            // 현재 타수에 대한 정보를 캐싱할 딕셔너리 생성
            ThrowCache[i] = new Dictionary<EMultiActionType, AudioClip>();

            foreach (var item in Throws[i].MultiActions)
            {
                // 현재 타수의 딕셔너리에 Type을 키값으로 Clip 캐싱
                ThrowCache[i].Add(item.ActionType, item.Clip);
            }
        }

        // 타수별 MultiAction의 ThrowHit 클립을 캐싱하기 위한 딕셔너리 배열
        ThrowHitsCache = new Dictionary<EMultiActionType, AudioClip>[ThrowHits.Length];
        for (int i = 0; i < ThrowHitsCache.Length; i++)
        {
            // 현재 타수에 MultiAction이 없으면 pass
            if (ThrowHits[i].MultiActions.Length <= 0)
                continue;

            // 현재 타수에 대한 정보를 캐싱할 딕셔너리 생성
            ThrowHitsCache[i] = new Dictionary<EMultiActionType, AudioClip>();

            foreach (var item in ThrowHits[i].MultiActions)
            {
                // 현재 타수의 딕셔너리에 Type을 키값으로 Clip 캐싱
                ThrowHitsCache[i].Add(item.ActionType, item.Clip);
            }
        }

        Debug.Log("Player Sound Init OK");
    }
}

[System.Serializable]
public class ThrowSoundInfo
{
    public MultiActionSoundInfo[] MultiActions;
    public AudioClip Clip;
}

[System.Serializable]
public class MultiActionSoundInfo
{
    public EMultiActionType ActionType;
    public AudioClip Clip;
}