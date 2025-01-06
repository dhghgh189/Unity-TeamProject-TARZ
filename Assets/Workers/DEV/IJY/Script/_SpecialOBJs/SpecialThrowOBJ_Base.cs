using UnityEngine;

public class SpecialThrowOBJ_Base : MonoBehaviour, Interaction_Ibase_GrabAct
{
    public Box_Type box_type { get; protected set; }
    public bool isDestroy = false;

    public Rigidbody rigidOBJ { get; set; }
    public PlayerController playerController { get; set; }
    public Collider col;
    public bool isThrowing = false;
    public Child_SpecialTrigger trigger;

    void Awake() => Init();

    void Init()
    {
        rigidOBJ = GetComponent<Rigidbody>();
        trigger = new GameObject("UI_trigger").AddComponent<Child_SpecialTrigger>();
        trigger.transform.position = this.transform.position;
        trigger.transform.parent = this.transform;

        col = GetComponent<Collider>();
    }

    public void Activate_Grab()
    {
        if (playerController == null) return;
        if (playerController.IsGrabingInput == false) return;

        if (playerController.IsGrabingInput == true)
        {
            Debug.Log($"{gameObject.name} : 활성화됨");
            ThrowingReady(playerController.interactioner.GrabPos);
        }
    }

    void ThrowingReady(Transform curPos)
    {
        Destroy(trigger.gameObject);
        trigger = null;

        this.gameObject.layer = 0;
        col.enabled = false;
        this.gameObject.transform.parent = playerController.interactioner.gameObject.transform;
        this.transform.position = curPos.position;
    }
}

