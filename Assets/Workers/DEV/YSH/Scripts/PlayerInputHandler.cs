using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerController controller;
    public List<bool> TryKeyDown = new();

    private PlayerInput input;
    public PlayerInput Input { get { return input; } }

    private Vector2 move;
    public Vector3 InputDir { get; private set; }
    public Vector2 InputLook { get; private set; }
    public bool TryJump { get; private set; }
    public bool TryThrow { get; private set; }
    public bool TryMelee { get; private set; }
    public bool TryDash { get; private set; }
    public bool TryDrain { get; private set; }
    public bool TryInteraction { get; set; }
    public bool TryManaSkill { get; private set; }
    public bool TryLockOnToggle { get; private set; }

    [HideInInspector] public bool[] UseKeyPressed;

    private void Start()
    {
        UseKeyPressed = new bool[Define.USEKEY_MAXCOUNT];
        
        controller = GetComponent<PlayerController>();
        input = GetComponent<PlayerInput>();

        TryKeyDown.Add(TryJump);
        TryKeyDown.Add(TryThrow);
        TryKeyDown.Add(TryMelee);
        TryKeyDown.Add(TryDash);
        TryKeyDown.Add(TryDrain);
        TryKeyDown.Add(TryInteraction);
    }

    void Update()
    {
        if (controller.IsGrabingInput)
        {
            GrabingInput();
            return;
        }

        if (controller.IsAnimStart)
        {
            AnimStart();
            return;
        }

        move = input.actions["Move"].ReadValue<Vector2>();
        InputDir = new Vector3(move.x, 0, move.y);

        InputLook = input.actions["Look"].ReadValue<Vector2>();
        TryThrow = input.actions["Throw"].WasPressedThisFrame();
        TryMelee = input.actions["Melee"].WasPressedThisFrame();
        TryManaSkill = input.actions["ManaSkillMode"].IsPressed();
        TryLockOnToggle = input.actions["LockOnToggle"].WasPressedThisFrame();

        if (!TryManaSkill)
        {
            TryDash = input.actions["Dash"].WasPressedThisFrame();
            TryJump = input.actions["Jump"].WasPressedThisFrame();
            TryDrain = input.actions["Drain"].IsPressed();
            TryInteraction = input.actions["Interact"].WasPressedThisFrame();
        }

        for (int i = 0; i < Define.USEKEY_MAXCOUNT; i++)
        {
            UseKeyPressed[i] = input.actions[$"Use{i + 1}"].WasPressedThisFrame();
        }
    }

    void GrabingInput()
    {
        move = input.actions["Move"].ReadValue<Vector2>();
        InputDir = new Vector3(move.x, 0, move.y);
        InputLook = input.actions["Look"].ReadValue<Vector2>();
        TryJump = input.actions["Jump"].WasPressedThisFrame();
        TryInteraction = input.actions["Interact"].WasPressedThisFrame();

        for (int i = TryKeyDown.Count - 1; i >= 0; i--)
        {
            if (TryKeyDown[i] == TryJump) continue;
            if (TryKeyDown[i] == TryInteraction) continue;
            TryKeyDown[i] = false;
        }
    }

    void AnimStart()
    {
        InputDir = Vector3.zero;
        InputLook = input.actions["Look"].ReadValue<Vector2>();

        for (int i = TryKeyDown.Count - 1; i >= 0; i--)
        {
            TryKeyDown[i] = false;
        }
    }
}
