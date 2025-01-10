using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    const string SOUND_PATH = "Managed/Sound";

    private SoundDataSO _soundData;
    public static SoundDataSO SoundData { get { return Instance._soundData; } private set { } }

    // BGM 소스
    private AudioSource bgmSource;
    public static AudioSource BGM { get { return Instance.bgmSource; } }

    // SFX 소스
    private AudioSource sfxSource;
    public static AudioSource SFX { get { return Instance.sfxSource; } }

    /// <summary>
    /// 사운드 매니저를 싱글톤으로 선언                                //추후 젠젝트로 뺄 것
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

        //bgmSource, sfxSource 불러오기
        sfxSource = gameObject.AddComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        _soundData = Resources.Load<SoundDataSO>($"{SOUND_PATH}/SoundData");
    }

    /// <summary>
    /// 배경 음악 교체 후 재생
    /// </summary>
    public static void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;

        if (BGM.isPlaying)
        {
            BGM.Stop();
        }

        BGM.clip = clip;
        BGM.Play();
    }

    /// <summary>
    /// 배경 음악 정지
    /// </summary>
    public static void StopBGM()
    {
        if (BGM.isPlaying == false)
            return;

        BGM.Stop();
    }

    /// <summary>
    /// 효과음 교체 후 재생
    /// </summary>
    public static void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        SFX.clip = clip;
        SFX.PlayOneShot(clip);
    }

    /// <summary>
    /// 효과음 정지
    /// </summary>
    public static void StopSFX()
    {
        if (SFX.isPlaying == false)
            return;

        SFX.Stop();
    }
}
