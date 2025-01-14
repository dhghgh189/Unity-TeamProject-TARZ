using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/SoundData/Player")]
public class PlayerSoundData : ScriptableObject
{
    [SerializeField] private Sound _sound;

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
    public AudioClip[] Throws { get { return _sound.Throws; } }
    public AudioClip ThrowHit { get { return _sound.ThrowHit; } }
    public AudioClip JumpThrow { get { return _sound.JumpThrow; } }
    public AudioClip JumpThrowHit { get { return _sound.JumpThrowHit; } }

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
        public AudioClip[] Throws;              // 원거리 공격 (1~3), 순서 정확히 맞출 것
        public AudioClip ThrowHit;              // 원거리 공격 히트 시
        public AudioClip JumpThrow;             // 점프 원거리 공격 발동
        public AudioClip JumpThrowHit;          // 점프 원거리 공격 히트 시
    }
}
