using UnityEngine;

public class Example_Interaction_GrabScript : MonoBehaviour, Base_InteractionOBJ_Grab
{
    public PlayerController playerController { get; set; }
    [SerializeField] public bool GrabOnOff { get; set; }

    void Start() => GrabOnOff = false;

    public void Activate_Grab()
    {
        if (GrabOnOff == true)
        {
            Debug.Log($"{gameObject.name} : 활성화됨");
        }
        else if (GrabOnOff == false)
        {
            Debug.Log($"{gameObject.name} : 비활성화됨");
        }
    }
}

