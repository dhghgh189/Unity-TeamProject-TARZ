using System;
using UnityEngine;
using static SoundDataSO;

[CreateAssetMenu(menuName = "Scriptables/SoundData")]
public class SoundDataSO : ScriptableObject
{
    [Serializable]
    public struct Sound
    {
        public AudioClip MSkill_Bomb;
        public AudioClip MSkill_Mine;
        public AudioClip MSkill_StimPak;
        public AudioClip MSkill_WheelWind;
        public AudioClip MSkill_TrippleAttack;
        public AudioClip MSkill_JumpAttack;
        public AudioClip MSkill_DashAttack;
        public AudioClip MSkill_ElectricWall;
        public AudioClip MSkill_Thunder;

        public AudioClip Arnold_Attack;
        public AudioClip Jake_Attack;
        public AudioClip Amber_Attack;
    }

    [SerializeField] private Sound _sound;

    /// <summary>
    /// 몬스터 스킬 사운드
    /// </summary>
    public AudioClip MSkill_Bomb { get { return _sound.MSkill_Bomb; } }
    public AudioClip MSkill_Mine { get { return _sound.MSkill_Mine; } }
    public AudioClip MSkill_StimPak { get { return _sound.MSkill_StimPak; } }
    public AudioClip MSkill_WheelWind { get { return _sound.MSkill_WheelWind; } }
    public AudioClip MSkill_TrippleAttack { get { return _sound.MSkill_TrippleAttack; } }
    public AudioClip MSkill_JumpAttack { get { return _sound.MSkill_JumpAttack; } }
    public AudioClip MSkill_DashAttack { get { return _sound.MSkill_DashAttack; } }
    public AudioClip MSkill_ElectricWall { get { return _sound.MSkill_ElectricWall; } }
    public AudioClip MSkill_Thunder { get { return _sound.MSkill_Thunder; } }

    /// <summary>
    /// 몬스터
    /// </summary>
    public AudioClip Arnold_Attack { get { return _sound.Arnold_Attack; } }
    public AudioClip Jake_Attack { get { return _sound.Jake_Attack; } }
    public AudioClip Amber_Attack { get { return _sound.Amber_Attack; } }
}
