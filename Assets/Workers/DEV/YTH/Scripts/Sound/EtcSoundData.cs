using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptables/SoundData/Etc")]
public class EtcSoundData : ScriptableObject
{
    [SerializeField] private Sound _sound;

    /// <summary>
    /// BGM
    /// </summary>
    public AudioClip TitleBGM { get { return _sound.TitleBGM; } }
    public AudioClip LobbyBGM { get { return _sound.LobbyBGM; } }
    public AudioClip FieldBGM { get { return _sound.FieldBGM; } }
    public AudioClip GameOver { get { return _sound.GameOver; } }
    public AudioClip GameClear { get { return _sound.GameClear; } }
    public AudioClip HPLowBGM { get { return _sound.HPLowBGM; } }
    public AudioClip StoreBGM { get { return _sound.StoreBGM; } }

    /// <summary>
    /// UI 사운드
    /// </summary>
    public AudioClip OnUI { get { return _sound.OnUI; } }
    public AudioClip OffUI { get { return _sound.OffUI; } }
    public AudioClip MoveUI { get { return _sound.MoveUI; } }
    public AudioClip SelectUI { get { return _sound.SelectUI; } }
    public AudioClip ApplyUI { get { return _sound.ApplyUI; } }

    /// <summary>
    /// 장비 사운드
    /// </summary>
    public AudioClip OnEquip { get { return _sound.OnEquip; } }
    public AudioClip OffEquip { get { return _sound.OffEquip; } }
    public AudioClip DecompositEquip { get { return _sound.DecompositEquip; } }

    /// <summary>
    /// 상점 사운드
    /// </summary>
    public AudioClip OnShop { get { return _sound.OnShop; } }
    public AudioClip OffShop { get { return _sound.OffShop; } }
    public AudioClip Buy { get { return _sound.Buy; } }
    public AudioClip Reroll { get { return _sound.Reroll; } }
    public AudioClip UpSucces { get { return _sound.UpSucces; } }
    public AudioClip UpFaild { get { return _sound.UpFaild; } }

    /// <summary>
    /// Object
    /// </summary>
    public AudioClip GetBlueChip { get { return _sound.GetBlueChip; } }
    public AudioClip GetRedChip { get { return _sound.GetRedChip; } }
    public AudioClip GetEquipment { get { return _sound.GetEquipment; } }
    public AudioClip GetMoney { get { return _sound.GetMoney; } }
    public AudioClip HPBuff { get { return _sound.HPBuff; } }
    public AudioClip StaminaBuff { get { return _sound.StaminaBuff; } }
    public AudioClip ManaBuff { get { return _sound.ManaBuff; } }
    public AudioClip ObejectBuff { get { return _sound.ObejectBuff; } }
    public AudioClip ObejectBroken { get { return _sound.ObejectBroken; } }
    public AudioClip Bomb { get { return _sound.Bomb; } }
    public AudioClip BombTimer { get { return _sound.BombTimer; } }

    

    [Serializable]
    public struct Sound
    {
        [Header("BGM")]
        public AudioClip TitleBGM;
        public AudioClip LobbyBGM;
        public AudioClip FieldBGM;
        public AudioClip GameOver;
        public AudioClip GameClear;
        public AudioClip HPLowBGM;
        public AudioClip StoreBGM;

        [Header("SFX")]
        [Header("UI")]
        public AudioClip OnUI;               // UI 켜기
        public AudioClip OffUI;              // UI 닫기
        public AudioClip MoveUI;             // UI 이동
        public AudioClip SelectUI;           // UI 선택
        public AudioClip ApplyUI;            // UI 적용

        [Header("Equipment")]
        public AudioClip OnEquip;            // 장착
        public AudioClip OffEquip;           // 분리
        public AudioClip DecompositEquip;    // 분해

        [Header("Shop")]
        public AudioClip OnShop;             // 상점 켜기
        public AudioClip OffShop;            // 상점 닫기
        public AudioClip Buy;                // 구매
        public AudioClip Reroll;             // 리롤
        public AudioClip UpSucces;             // 리롤
        public AudioClip UpFaild;             // 리롤

        [Header("Object")]
        public AudioClip GetBlueChip;
        public AudioClip GetRedChip;
        public AudioClip GetEquipment;
        public AudioClip GetMoney;
        public AudioClip HPBuff;
        public AudioClip StaminaBuff;
        public AudioClip ManaBuff;
        public AudioClip ObejectBuff;
        public AudioClip ObejectBroken;
        public AudioClip Bomb;
        public AudioClip BombTimer;
    }
}
