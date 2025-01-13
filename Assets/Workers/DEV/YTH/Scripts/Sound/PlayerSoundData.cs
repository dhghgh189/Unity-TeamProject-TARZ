using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/SoundData/Player")]
public class PlayerSoundData : ScriptableObject
{
    [SerializeField] private Sound _sound;
    public AudioClip M_TakeDamage { get { return _sound.M_TakeDamage; } }





    [Serializable]
    public struct Sound
    {
        [Header("SFX")]
        public AudioClip M_TakeDamage;
    }
}
