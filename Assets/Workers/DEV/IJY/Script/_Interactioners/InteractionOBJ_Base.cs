using UnityEngine;

public class InteractionOBJ_Base : MonoBehaviour
{
    public Child_SpecialTrigger trigger;

    void Awake() => Init();

    void Init()
    {
        trigger = new GameObject("UI_trigger").AddComponent<Child_SpecialTrigger>();
        trigger.transform.position = this.transform.position;
        trigger.transform.parent = this.transform;
    }
}
