using UnityEngine;

public class Example_Interaction_GrabScript : MonoBehaviour, Base_InteractionOBJ_Grab
{
    public PlayerController playerController { get; set; }

    public void Activate_Grab()
    {
        if (playerController == null) return;

        if (playerController.IsGrabingInput == true)
        {
            Debug.Log($"{gameObject.name} : 활성화됨");
        }
        else if (playerController.IsGrabingInput == false)
        {
            Debug.Log($"{gameObject.name} : 비활성화됨");
        }
    }
}

