public class ManaBuff : Buff
{
    public override void Use(PlayerController player)
    {
        player.Stat.CurrentMp += player.Stat.MaxMp / 4;
    }
}
