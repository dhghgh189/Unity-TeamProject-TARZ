using UnityEngine;

public class Example_InteractionScript : MonoBehaviour, Base_InteractionOBJ
{
    public void Activate()
    {
        Debug.Log($"{gameObject.name} : 말을 걸었다");
    }
}
