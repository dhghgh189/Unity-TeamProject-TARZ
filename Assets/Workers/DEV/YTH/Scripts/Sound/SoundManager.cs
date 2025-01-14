using System.Collections.Generic;
using UnityEngine;
using static MonsterSoundData;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    const string SOUND_PATH = "Managed/Sound";

    private MonsterSoundData _monsterSoundData;
    public static MonsterSoundData SoundData_M { get { return Instance._monsterSoundData; } private set { } }

    private PlayerSoundData _playerSoundData;
    public static PlayerSoundData SoundData_P { get { return Instance._playerSoundData; } private set { } }

    private EtcSoundData _etcSoundData;
    public static EtcSoundData SoundData_UI { get { return Instance._etcSoundData; } private set { } }

    public Dictionary<int, AudioClip> monsterSoundDic = new Dictionary<int, AudioClip>();


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
        _monsterSoundData = Resources.Load<MonsterSoundData>($"{SOUND_PATH}/MonsterSoundData");
        _playerSoundData = Resources.Load<PlayerSoundData>($"{SOUND_PATH}/PlayerSoundData");
        _etcSoundData = Resources.Load<EtcSoundData>($"{SOUND_PATH}/EtcSoundData");

        for (int i = 0; i < _monsterSoundData.SoundInfos.Count; i++)
        {
            monsterSoundDic.Add(_monsterSoundData.SoundInfos[i].ID, _monsterSoundData.SoundInfos[i].Clip);
        }

        _playerSoundData.Init();
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
