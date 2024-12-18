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
    /// ��� ���� ���
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
    /// ��� ���� ����
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource.isPlaying == false)
            return;

        bgmSource.Stop();
    }

    /// <summary>
    /// ȿ���� ���
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// ȿ���� ����
    /// </summary>
    public void StopSFX()
    {
        if (sfxSource.isPlaying == false)
            return;

        sfxSource.Stop();
    }
}
