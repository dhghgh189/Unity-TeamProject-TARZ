using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public List<Vector3> TryInputDown = new();
    public Vector3 InputDir { get; private set; }
    public bool TryJump { get; private set; }
    public bool TryThrow { get; private set; }
    public bool TryMelee { get; private set; }
    public bool TryDash { get; private set; }
    public bool TryDrain { get; private set; }
    public bool TryInteraction { get; private set; }
    public bool TryManaSkill { get; private set; }

    [HideInInspector] public bool[] UseKeyPressed;

    private void Start()
    {
        TryInputDown.Add(InputDir);
        UseKeyPressed = new bool[Define.USEKEY_MAXCOUNT];
    }

    void Update()
    {
        InputDir = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // 패드 지원되도록 구성해야 함    
        TryThrow = Input.GetButtonDown("Throw");
        TryMelee = Input.GetButtonDown("Melee");      
        TryManaSkill = Input.GetButton("TryManaSkill");

        if (!TryManaSkill)
        {
            TryDash = Input.GetButtonDown("Dash");
            TryJump = Input.GetButtonDown("Jump");
            TryDrain = Input.GetButton("Drain");
            TryInteraction = Input.GetButtonDown("Interaction");
        }

        for (int i = 0; i < Define.USEKEY_MAXCOUNT; i++)
        {
            UseKeyPressed[i] = Input.GetButtonDown($"UseKey {i + 1}");
        }
    }
}
