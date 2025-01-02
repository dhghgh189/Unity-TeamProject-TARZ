using UnityEngine;

public class Example_Interaction_GrabScript : MonoBehaviour, Base_InteractionOBJ_Grab
{
    public PlayerController playerController { get; set; }
    public Collider col;
    public bool isThrowing = false;


    void Start() => Init();

    void Init()
    {
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
        col.enabled = false;
        this.gameObject.transform.parent = playerController.interactioner.gameObject.transform;
        this.transform.position = curPos.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrowing)
        {
            Destroy(this.gameObject);
        }
        else return;
    }
}

