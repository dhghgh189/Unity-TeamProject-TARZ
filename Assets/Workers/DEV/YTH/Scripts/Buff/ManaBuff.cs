public class ManaBuff : Buff
{
    public override void Use(PlayerController player)
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.ManaBuff);
        player.Stat.CurrentMp += player.Stat.MaxMp / 4;
    }
}
