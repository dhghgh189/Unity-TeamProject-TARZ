using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSoundScript : MonoBehaviour
{
    [SerializeField] int index;

    public void PlaySound()
    {
        if (index == -1) return;

        SoundManager.PlaySFX(SoundManager.SoundData_S.BluechipSounds[index].AudioClip);
    }
}
