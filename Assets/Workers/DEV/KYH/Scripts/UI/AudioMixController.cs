using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Header("<color=green>Sound Slider</color>")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("<color=orange>Mute Toggle</color>")]
    [SerializeField] private Toggle masterMute;
    [SerializeField] private Toggle bgmMute;
    [SerializeField] private Toggle sfxMute;
    private float muteBGM = 0f;
    private float muteSFX = 0f;

    public void MasterVolumeControl()
    {
        float sound = masterSlider.value;

        if (sound == -40f) { audioMixer.SetFloat("Master", -80); }
        else { audioMixer.SetFloat("Master", sound); }
    }

    public void BGMVolumeControl()
    {
        float sound = bgmSlider.value;

        if (sound == -40f) { audioMixer.SetFloat("BGM", -80); }
        else { audioMixer.SetFloat("BGM", sound); }
    }

    public void SFXVolumeControl()
    {
        float sound = sfxSlider.value;

        if (sound == -40f) { audioMixer.SetFloat("SFX", -80); }
        else { audioMixer.SetFloat("SFX", sound); }
    }

    public void MasterVolumeMute ()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void BGMVolumeMute ()
    {
        if (bgmMute.isOn == true)
        {
            audioMixer.GetFloat("BGM", out muteBGM);
            audioMixer.SetFloat("BGM", -80f);
        }
        else
        {
            audioMixer.SetFloat("BGM", muteBGM);
        }
    }

    public void SFXVolumeMute()
    {
        if (sfxMute.isOn == true)
        {
            audioMixer.GetFloat("SFX", out muteSFX);
            audioMixer.SetFloat("SFX", -80f);
        }
        else
        {
            audioMixer.SetFloat("SFX", muteSFX);
        }
    }

    /*private void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }*/
}
