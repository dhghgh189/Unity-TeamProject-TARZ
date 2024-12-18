using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get { return SoundManager.Instance; } private set { } }

    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

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

    /// <summary>
    /// 배경 음악 재생
    /// </summary>
    public void PlayBGM(AudioClip clip)
    {
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
