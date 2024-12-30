public interface IDamageUpgrade
{
    public float IncreaseDamage { get; set; }
    public void UpgradeDamage(in float curDamage, out float Resultdamage);

}
