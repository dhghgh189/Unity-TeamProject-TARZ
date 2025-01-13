using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptables/SoundData/Monster")]
public class MonsterSoundData : ScriptableObject
{
    [SerializeField] private Sound _sound;

    /// <summary>
    /// 몬스터 스킬 사운드
    /// </summary>
    public AudioClip Bomb { get { return _sound.Bomb; } }
    public AudioClip Mine { get { return _sound.Mine; } }
    public AudioClip StimPak_Bomber { get { return _sound.StimPak_Bomber; } }
    public AudioClip StimPak_Jack { get { return _sound.StimPak_Jack; } }
    public AudioClip WheelWind { get { return _sound.WheelWind; } }
    public AudioClip TrippleAttack { get { return _sound.TrippleAttack; } }
    public AudioClip JumpAttack { get { return _sound.JumpAttack; } }
    public AudioClip DashAttack { get { return _sound.DashAttack; } }
    public AudioClip ElectricWall { get { return _sound.ElectricWall; } }
    public AudioClip ElectricWall_2 { get { return _sound.ElectricWall_2; } }
    public AudioClip Thunder { get { return _sound.Thunder; } }
    public AudioClip Roar { get { return _sound.Roar; } }

    [Serializable]
    public struct Sound
    {
        [Header("Skill")]
        public AudioClip Bomb;
        public AudioClip Mine;
        public AudioClip StimPak_Bomber;
        public AudioClip StimPak_Jack;
        public AudioClip WheelWind;
        public AudioClip TrippleAttack;
        public AudioClip JumpAttack;
        public AudioClip DashAttack;
        public AudioClip ElectricWall;
        public AudioClip ElectricWall_2;
        public AudioClip Thunder;
        public AudioClip Roar;
    }

    [SerializeField] List<SoundInfo> soundInfos = new List<SoundInfo>();
    public List<SoundInfo> SoundInfos { get { return soundInfos; } private set { } }

    [Serializable]
    public struct SoundInfo
    {
        public int ID;
        public AudioClip Clip;
    }
}

