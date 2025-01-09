using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/SoundData")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>
    /// 몬스터 스킬 사운드
    /// </summary>
    public AudioClip MSkill_Bomb;
    public AudioClip MSkill_Mine;
    public AudioClip MSkill_StimPak;
    public AudioClip MSkill_WheelWind;
    public AudioClip MSkill_TrippleAttack;
    public AudioClip MSkill_JumpAttack;
    public AudioClip MSkill_DashAttack;
    public AudioClip MSkill_ElectricWall;
    public AudioClip MSkill_Thunder;

    /// <summary>
    /// 몬스터
    /// </summary>
    public AudioClip Arnold_Attack;
    public AudioClip Jake_Attack;
    public AudioClip Amber_Attack;
}
