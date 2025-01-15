using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSoundScript : MonoBehaviour
{
    [SerializeField] int index;

    public void PlaySound()
    {
        if (index < SoundManager.SoundData_S.BluechipSounds.Count || index >= SoundManager.SoundData_S.BluechipSounds.Count) { Debug.Log("스킬 효과음 index 범위 넘어감"); return; }
        SoundManager.PlaySFX(SoundManager.SoundData_S.BluechipSounds[index].AudioClip);
    }
}
