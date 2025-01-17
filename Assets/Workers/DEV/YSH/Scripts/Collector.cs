using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        ThrowObject tobj = other.gameObject.GetComponent<ThrowObject>();
        if (tobj != null
            && tobj.IsCollected == false
            && player.Attack.ObjectCount < player.Attack.MaxObjectCount)
        {
            //Debug.Log($"Get Throw Object! : {tobj.name}");
            SoundManager.PlaySFX(SoundManager.SoundData_UI.GetEquipment);
            tobj.Get(player);
        }

        Buff buff = other.GetComponent<Buff>();
        if (buff != null)
        {
            buff.Use(player);

            if(string.Compare(buff.Name, "SpeedBuff") != 0)
                Destroy(buff.gameObject);
        }
    }
}
