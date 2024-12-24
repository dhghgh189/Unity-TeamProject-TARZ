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

    private float masterVol;
    private float bgmVol;
    private float sfxVol;

    [Header("<color=orange>Mute Toggle</color>")]
    [SerializeField] private Toggle masterMute;
    [SerializeField] private Toggle bgmMute;
    [SerializeField] private Toggle sfxMute;

    private float muteBGM;
    private float muteSFX;

    private void Awake()
    {
        // 볼륨 초기값 로드 (기본값 0 dB)
        masterSlider.value = PlayerPrefs.GetFloat("Master", masterVol);
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", bgmVol);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", sfxVol);
    }

    private void Start()
    {
        // 음소거 상태 로드 및 초기화
        masterMute.isOn = PlayerPrefs.GetInt("MasterMute", 0) == 1;
        bgmMute.isOn = PlayerPrefs.GetInt("BGMMute", 0) == 1;
        sfxMute.isOn = PlayerPrefs.GetInt("SFXMute", 0) == 1;

        if (masterMute.isOn) AudioListener.volume = 0;
        if (bgmMute.isOn) audioMixer.SetFloat("BGM", -80f);
        if (sfxMute.isOn) audioMixer.SetFloat("SFX", -80f);

        audioMixer.SetFloat("Master", masterVol);
        audioMixer.SetFloat("BGM", bgmVol);
        audioMixer.SetFloat("SFX", sfxVol);
    }

    public void MasterVolumeSliderChanged()
    {
        masterVol = masterSlider.value;
        audioMixer.SetFloat("Master", masterVol == -40f ? -80f : masterVol); // 최소 볼륨 -80 dB로 설정
        PlayerPrefs.SetFloat("Master", masterVol);
        PlayerPrefs.Save();
    }

    public void BGMVolumeSliderChanged()
    {
        bgmVol = bgmSlider.value;
        audioMixer.SetFloat("BGM", bgmVol == -40f ? -80f : bgmVol);
        PlayerPrefs.SetFloat("BGM", bgmVol);
        PlayerPrefs.Save();
    }

    public void SFXVolumeSliderChanged()
    {
        sfxVol = sfxSlider.value;
        audioMixer.SetFloat("SFX", sfxVol == -40f ? -80f : sfxVol);
        PlayerPrefs.SetFloat("SFX", sfxVol);
        PlayerPrefs.Save();
    }

    public void MasterMuteToggleChanged()
    {
        bool isMuted = masterMute.isOn;
        AudioListener.volume = isMuted ? 0 : 1;
        PlayerPrefs.SetInt("MasterMute", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void BGMMuteToggleChanged()
    {
        if (bgmMute.isOn)
        {
            audioMixer.GetFloat("BGM", out muteBGM); // 현재 볼륨 저장
            audioMixer.SetFloat("BGM", -80f);       // 음소거
            PlayerPrefs.SetInt("BGMMute", 1);
        }
        else
        {
            audioMixer.SetFloat("BGM", muteBGM);    // 이전 볼륨 복원
            PlayerPrefs.SetInt("BGMMute", 0);
        }
        PlayerPrefs.Save();
    }

    public void SFXMuteToggleChanged()
    {
        if (sfxMute.isOn)
        {
            audioMixer.GetFloat("SFX", out muteSFX);
            audioMixer.SetFloat("SFX", -80f);
            PlayerPrefs.SetInt("SFXMute", 1);
        }
        else
        {
            audioMixer.SetFloat("SFX", muteSFX);
            PlayerPrefs.SetInt("SFXMute", 0);
        }
        PlayerPrefs.Save();
    }
}
