using System;
using UnityEngine;
using static SoundDataSO;

[CreateAssetMenu(menuName = "Scriptables/SoundData")]
public class SoundDataSO : ScriptableObject
{
    [Serializable]
    public struct Sound
    {
        public AudioClip M_TakeDamage;

       /* public AudioClip MSkill_Bomb;
        public AudioClip MSkill_Mine;
        public AudioClip MSkill_StimPak;
        public AudioClip MSkill_WheelWind;
        public AudioClip MSkill_TrippleAttack;
        public AudioClip MSkill_JumpAttack;
        public AudioClip MSkill_DashAttack;
        public AudioClip MSkill_ElectricWall;
        public AudioClip MSkill_Thunder;
       */
    }

    [SerializeField] private Sound _sound;

    public AudioClip M_TakeDamage { get { return _sound.M_TakeDamage; } }

    /// <summary>
    /// 몬스터 스킬 사운드
    /// </summary>
    /* public AudioClip MSkill_Bomb { get { return _sound.MSkill_Bomb; } }
     public AudioClip MSkill_Mine { get { return _sound.MSkill_Mine; } }
     public AudioClip MSkill_StimPak { get { return _sound.MSkill_StimPak; } }
     public AudioClip MSkill_WheelWind { get { return _sound.MSkill_WheelWind; } }
     public AudioClip MSkill_TrippleAttack { get { return _sound.MSkill_TrippleAttack; } }
     public AudioClip MSkill_JumpAttack { get { return _sound.MSkill_JumpAttack; } }
     public AudioClip MSkill_DashAttack { get { return _sound.MSkill_DashAttack; } }
     public AudioClip MSkill_ElectricWall { get { return _sound.MSkill_ElectricWall; } }
     public AudioClip MSkill_Thunder { get { return _sound.MSkill_Thunder; } }*/
}
