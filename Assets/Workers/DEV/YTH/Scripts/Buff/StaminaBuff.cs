using UnityEngine;
using Zenject;

public class StaminaBuff : Buff
{
    [SerializeField] float value; // 스태미너 무한 유지 시간

    public override void Use(PlayerController player)
    {
        player.InfStamina(value);
    }
}
