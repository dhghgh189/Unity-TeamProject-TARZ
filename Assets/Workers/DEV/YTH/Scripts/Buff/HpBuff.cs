using UnityEngine;
using Zenject;

public class HpBuff : Buff
{
    public override void Use(PlayerController player)
    {
        player.Stat.CurrentHp += player.Stat.MaxHp / 3;
    }
}
