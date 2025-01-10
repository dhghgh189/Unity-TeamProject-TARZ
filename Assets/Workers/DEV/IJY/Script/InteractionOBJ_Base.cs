using UnityEngine;

public class InteractionOBJ_Base : MonoBehaviour, Interaction_Ibase_Activate
{
    public Child_SpecialTrigger trigger;

    void Awake() => Init();

    void Init()
    {
        trigger = new GameObject("UI_trigger").AddComponent<Child_SpecialTrigger>();
        trigger.transform.position = this.transform.position;
        trigger.transform.parent = this.transform;
    }

    public void Activate()
    {
        Debug.Log($"{gameObject.name} : 말을 걸었다");
    }
}
