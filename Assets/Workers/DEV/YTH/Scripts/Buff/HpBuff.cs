using UnityEngine;
using Zenject;

public class HpBuff : Buff
{
    public override void Use(PlayerController player)
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.HPBuff);
        player.Stat.CurrentHp += player.Stat.MaxHp / 3;
    }
}
