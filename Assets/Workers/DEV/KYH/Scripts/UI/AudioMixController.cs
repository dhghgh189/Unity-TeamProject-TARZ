using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixController : MonoBehaviour
{
    // 오디오 믹서 참조
    [SerializeField] private AudioMixer audioMixer;

    // 사운드 조절 슬라이더
    [Header("<color=green>Sound Slider</color>")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    // 사운드 음소거 토글
    [Header("<color=orange>Mute Toggle</color>")]
    [SerializeField] private Toggle masterMute;
    [SerializeField] private Toggle bgmMute;
    [SerializeField] private Toggle sfxMute;

    [Header("<color=yellow>PlayerPrefs Keys</color>")]
    private string masterVolumeKey = "MasterVolume";
    private string bgmVolumeKey = "BGMVolume";
    private string sfxVolumeKey = "SFXVolume";
    private string masterMuteKey = "MasterMute";
    private string bgmMuteKey = "BGMMute";
    private string sfxMuteKey = "SFXMute";

    private void Start()
    {
        InitSlider(masterSlider, masterVolumeKey, "Master");
        InitSlider(bgmSlider, bgmVolumeKey, "BGM");
        InitSlider(sfxSlider, sfxVolumeKey, "SFX");

        InitToggle(masterMute, masterMuteKey, "Master");
        InitToggle(bgmMute, bgmMuteKey, "BGM");
        InitToggle(sfxMute, sfxMuteKey, "SFX");

        masterSlider.onValueChanged.AddListener(value => SetVolume(masterVolumeKey, "Master", value));
        bgmSlider.onValueChanged.AddListener(value => SetVolume(bgmVolumeKey, "BGM", value));
        sfxSlider.onValueChanged.AddListener(value => SetVolume(sfxVolumeKey, "SFX", value));

        masterMute.onValueChanged.AddListener(isMuted => SetMute(masterMuteKey, "Master", isMuted));
        bgmMute.onValueChanged.AddListener(isMuted => SetMute(bgmMuteKey, "BGM", isMuted));
        sfxMute.onValueChanged.AddListener(isMuted => SetMute(sfxMuteKey, "SFX", isMuted));
    }

    private void InitSlider(Slider slider, string prefsKey, string mixerPar)
    {
        float savedValue = PlayerPrefs.GetFloat(prefsKey, 0.75f);
        slider.value = savedValue;

        float volume = Mathf.Log10(Mathf.Max(savedValue, 0.0001f)) * 20f;
        audioMixer.SetFloat(mixerPar, volume);
    }

    private void InitToggle(Toggle toggle, string prefsKey, string mixerPar)
    {
        bool isMuted = PlayerPrefs.GetInt(prefsKey, 0) == 1;
        toggle.isOn = isMuted;

        if (isMuted)
        {
            audioMixer.SetFloat(mixerPar, -80f);
        }
    }

    private void SetVolume(string prefsKey, string mixerPar, float sliderValue)
    {
        float volume = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        audioMixer.SetFloat(mixerPar, volume);

        PlayerPrefs.SetFloat(prefsKey, sliderValue);
    }

    private void SetMute(string prefsKey, string mixerPar, bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat(mixerPar, -80f);
        }
        else
        {
            float savedValue = PlayerPrefs.GetFloat(prefsKey, 0.75f);
            float volume = Mathf.Log10(Mathf.Max(savedValue, 0.0001f)) * 20f;
            audioMixer.SetFloat(mixerPar, volume);
        }
        PlayerPrefs.SetInt(prefsKey, isMuted ? 1 : 0);
    }
    /*/// <summary>
    /// 마스터 볼륨값 조절
    /// </summary>
    public void MasterVolumeControl()
    {
        masterVolSave = masterSlider.value;

        if (masterVolSave == -40f) { audioMixer.SetFloat("Master", -80); }
        else { audioMixer.SetFloat("Master", masterVolSave); }

        PlayerPrefs.SetFloat("Master", masterVolSave);
        
    }

    /// <summary>
    /// BGM 볼륨값 조절
    /// </summary>
    public void BGMVolumeControl()
    {
        bgmVolSave = bgmSlider.value;

        if (bgmVolSave == -40f) { audioMixer.SetFloat("BGM", -80); }
        else { audioMixer.SetFloat("BGM", bgmVolSave); }

        PlayerPrefs.SetFloat("BGM", bgmVolSave);
    }

    /// <summary>
    /// SFX(효과음) 볼륨값 조절
    /// </summary>
    public void SFXVolumeControl()
    {
        sfxVolSave = sfxSlider.value;

        if (sfxVolSave == -40f) { audioMixer.SetFloat("SFX", -80); }
        else { audioMixer.SetFloat("SFX", sfxVolSave); }

        PlayerPrefs.SetFloat("SFX", sfxVolSave);
    }

    /// <summary>
    /// 마스터 볼륨 음소거
    /// </summary>
    public void MasterVolumeMute ()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    /// <summary>
    /// BGM 볼륨 음소거
    /// </summary>
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

    /// <summary>
    /// SHX(효과음) 볼륨 음소거
    /// </summary>
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
    }*/
}
