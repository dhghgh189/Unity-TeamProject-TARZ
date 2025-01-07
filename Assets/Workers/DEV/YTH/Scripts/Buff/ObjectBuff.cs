using UnityEngine;

public class ObjectBuff : Buff
{
    [SerializeField] ThrowObject throwObjectPrefab;

    [SerializeField] float value;

    public override void Use(PlayerController player)
    {
        ThrowObject throwObject;

        if (player.Attack.MaxObjectCount - player.Attack.ObjectCount < value)
        {
            value = player.Attack.MaxObjectCount - player.Attack.ObjectCount;
        }

        for (int i = 0; i < value; i++)
        {
            throwObject = Instantiate(throwObjectPrefab, transform.forward * 5f, Quaternion.identity);
            throwObject.Get(player);
        }
    }
}
