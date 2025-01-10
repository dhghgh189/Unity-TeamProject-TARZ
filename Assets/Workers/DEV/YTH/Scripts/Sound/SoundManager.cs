using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    const string SOUND_PATH = "Managed/Sound";

    [SerializeField] SoundDataSO _soundData;
    public static SoundDataSO SoundData { get { return Instance._soundData; } private set { } }

    [SerializeField] private AudioSource bgmSource;     // BGM 소스
    [SerializeField] private AudioSource sfxSource;     // SFX 소스

    /// <summary>
    /// 사운드 매니저를 싱글톤으로 선언
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _soundData = Resources.Load<SoundDataSO>($"{SOUND_PATH}/SoundData");
    }

    /// <summary>
    /// 배경 음악 재생
    /// </summary>
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;

        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    /// <summary>
    /// 배경 음악 정지
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource.isPlaying == false)
            return;

        bgmSource.Stop();
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.clip = clip;
        sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 효과음 정지
    /// </summary>
    public void StopSFX()
    {
        if (sfxSource.isPlaying == false)
            return;

        sfxSource.Stop();
    }
}
