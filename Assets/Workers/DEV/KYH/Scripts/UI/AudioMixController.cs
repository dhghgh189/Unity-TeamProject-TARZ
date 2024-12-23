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

    /*private void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }*/
}
