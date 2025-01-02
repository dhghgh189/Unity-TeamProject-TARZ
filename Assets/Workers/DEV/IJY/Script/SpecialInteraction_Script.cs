using UnityEngine;

public class SpecialInteraction_Script : MonoBehaviour, Base_InteractionOBJ_Grab
{
    public PlayerController playerController { get; set; }
    public Collider col;
    public bool isThrowing = false;
    public Child_SpecialTrigger trigger;


    void Awake() => Init();

    void Init()
    {
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
        trigger = null;
        this.transform.DetachChildren();

        col.enabled = false;
        this.gameObject.transform.parent = playerController.interactioner.gameObject.transform;
        this.transform.position = curPos.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrowing)
        {
            // TODO : 오브젝트 폭발이나 데미지 입히는 기능 구현
            Destroy(this.gameObject, 3f);
        }
        else return;
    }
}

