using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController controller;
    public List<bool> TryKeyDown = new();
    public Vector3 InputDir { get; private set; }
    public bool TryJump { get; private set; }
    public bool TryThrow { get; private set; }
    public bool TryMelee { get; private set; }
    public bool TryDash { get; private set; }
    public bool TryDrain { get; private set; }
    public bool TryInteraction { get; private set; }

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        TryKeyDown.Add(TryJump);
        TryKeyDown.Add(TryThrow);
        TryKeyDown.Add(TryMelee);
        TryKeyDown.Add(TryDash);
        TryKeyDown.Add(TryDrain);
        TryKeyDown.Add(TryInteraction);
    }

    void Update()
    {
        if (controller.IsAnimStart)
        {
            InputDir = Vector3.zero;

            for (int i = TryKeyDown.Count - 1; i >= 0; i--)
            {
                TryKeyDown[i] = false;
            }

            return;
        }

        InputDir = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // 패드 지원되도록 구성해야 함
        TryDash = Input.GetButtonDown("Dash");
        TryJump = Input.GetButtonDown("Jump");
        TryThrow = Input.GetButtonDown("Throw");
        TryMelee = Input.GetButtonDown("Melee");
        TryDrain = Input.GetButton("Drain");
        TryInteraction = Input.GetKeyDown(KeyCode.E);
    }
}
