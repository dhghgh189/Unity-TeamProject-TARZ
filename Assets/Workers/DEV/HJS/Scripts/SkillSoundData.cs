using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/SoundData/Skill")]
public class SkillSoundData : ScriptableObject
{
    [SerializeField] private Sound _sound;

    public List<SoundStruct> ManaSkillSounds_1 => _sound.ManaSkillSounds_1;
    public List<SoundStruct> ManaSkillSounds_2 => _sound.ManaSkillSounds_2;
    public List<SoundStruct> ManaSkillSounds_3 => _sound.ManaSkillSounds_3;
    public List<SoundStruct> ManaSkillSounds_4 => _sound.ManaSkillSounds_4;
    public List<SoundStruct> BagSkillSounds_1 => _sound.BagSkillSounds_1;
    public List<SoundStruct> BagSkillSounds_2 => _sound.BagSkillSounds_2;
    public List<SoundStruct> BagSkillSounds_3 => _sound.BagSkillSounds_3;

    [Serializable]
    public struct Sound
    {
        [Header("마나 스킬")]
        [Header("마나 1스킬")]
        public List<SoundStruct> ManaSkillSounds_1;
        [Header("마나 2스킬")]
        public List<SoundStruct> ManaSkillSounds_2;
        [Header("마나 3스킬")]
        public List<SoundStruct> ManaSkillSounds_3;
        [Header("마나 4스킬")]
        public List<SoundStruct> ManaSkillSounds_4;

        [Header("가방 스킬")]
        [Header("가방 1스킬")]
        public List<SoundStruct> BagSkillSounds_1;
        [Header("가방 2스킬")]
        public List<SoundStruct> BagSkillSounds_2;
        [Header("가방 3스킬")]
        public List<SoundStruct> BagSkillSounds_3;
    }

    [Serializable]
    public struct SoundStruct
    {
        public string IndexName;
        public AudioClip AudioClip;
    }
}
