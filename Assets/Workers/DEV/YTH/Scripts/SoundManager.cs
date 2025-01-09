using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    [SerializeField] SoundDataSO _soundData;

    [SerializeField] private AudioSource bgmSource;     // BGM 소스
    [SerializeField] private AudioSource sfxSource;     // SFX 소스
    
    public static SoundManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    /// <summary>
    /// 사운드 매니저를 싱글톤으로 선언
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
