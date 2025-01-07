using UnityEngine;
using Zenject;

public class TestShop : MonoBehaviour
{
    [Inject]
    private PlayerController playerController;

    [SerializeField] Buff[] potions;

    private void Start()
    {
        for (int i = 0; i < potions.Length; i++)
        {
            Debug.Log($"{i + 1} : {potions[i].Name}, {potions[i].Description}, {potions[i].Price} ");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Buy(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Buy(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Buy(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Buy(3);
        }
    }

    private void Buy(int index)
    {
        potions[index].Use(playerController);
    }
}
