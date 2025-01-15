using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, Interaction_Ibase_Activate
{
    private TrapRoomBehaviour trapRoomBehaviour;

    private void Awake()
    {
        trapRoomBehaviour = GetComponentInParent<TrapRoomBehaviour>();
    }

    private void Start()
    {
        EffectManager.instance.ParticlePlay("DustMotesLively", 600f, transform.position, Quaternion.identity);
    }

    public void Activate()
    {
        trapRoomBehaviour.Clear();
        Debug.Log("Activate!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }
}
