using UnityEngine;

public class Example_InteractionScript : MonoBehaviour, Interaction_Ibase_Activate
{
    public void Activate()
    {
        Debug.Log($"{gameObject.name} : 말을 걸었다");
    }
}
