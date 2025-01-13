using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptables/SoundData/UI")]
public class UiSoundData : ScriptableObject
{
    [SerializeField] private Sound _sound;
    public AudioClip M_TakeDamage { get { return _sound.M_TakeDamage; } }
    



    [Serializable]
    public struct Sound
    {
        [Header("BGM")]
        public AudioClip lobbyBGM;

        [Header("SFX")]
        public AudioClip M_TakeDamage;

       
    }
}
