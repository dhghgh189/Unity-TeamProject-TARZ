using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptables/SoundData/Monster")]
public class MonsterSoundData : ScriptableObject
{
    [SerializeField] private Sound _sound;

    /// <summary>
    /// 몬스터 기본 사운드
    /// </summary>
    public AudioClip TakeDamage { get { return _sound.TakeDamage; } }
    public AudioClip Idle { get { return _sound.Idle; } }
    public AudioClip Move { get { return _sound.Move; } }
    public AudioClip MeleeAttack { get { return _sound.MeleeAttack; } }
    public AudioClip RangeAttack { get { return _sound.RangeAttack; } }
    public AudioClip Die { get { return _sound.Die; } }
    

    /// <summary>
    /// 몬스터 스킬 사운드
    /// </summary>
    public AudioClip Bomb { get { return _sound.Bomb; } }
    public AudioClip Mine { get { return _sound.Mine; } }
    public AudioClip StimPak { get { return _sound.StimPak; } }
    public AudioClip WheelWind { get { return _sound.WheelWind; } }
    public AudioClip TrippleAttack { get { return _sound.TrippleAttack; } }
    public AudioClip JumpAttack { get { return _sound.JumpAttack; } }
    public AudioClip DashAttack { get { return _sound.DashAttack; } }
    public AudioClip ElectricWall { get { return _sound.ElectricWall; } }
    public AudioClip Thunder { get { return _sound.Thunder; } }
    public AudioClip Roar { get { return _sound.Roar; } }

    /// <summary>
    /// 몬스터 특수 사운드
    /// </summary>
     public AudioClip ArnoldSpawn { get { return _sound.ArnoldSpawn; } }

    [Serializable]
    public struct Sound
    {
        [Header("Basic")]
        [Header("Male")]
        public AudioClip TakeDamage;
        public AudioClip Idle;
        public AudioClip Move;
        public AudioClip MeleeAttack;
        public AudioClip RangeAttack;
        public AudioClip Die;

        [Header("Skill")]
        public AudioClip Bomb;
        public AudioClip Mine;
        public AudioClip StimPak;
        public AudioClip WheelWind;
        public AudioClip TrippleAttack;
        public AudioClip JumpAttack;
        public AudioClip DashAttack;
        public AudioClip ElectricWall;
        public AudioClip Thunder;
        public AudioClip Roar;

        [Header("Etc")]
        public AudioClip ArnoldSpawn;
    }
}
