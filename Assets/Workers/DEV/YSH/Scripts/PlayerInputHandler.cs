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

     public bool IsCanControl;

    [HideInInspector] public bool[] UseKeyPressed;

    private void Awake()
    {
        IsCanControl = true;
    }

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

        move = IsCanControl ? input.actions["Move"].ReadValue<Vector2>() : Vector2.zero;
        InputDir = new Vector3(move.x, 0, move.y);

        InputLook = IsCanControl ? input.actions["Look"].ReadValue<Vector2>() : Vector2.zero;
        TryThrow = IsCanControl && input.actions["Throw"].WasPressedThisFrame();
        TryMelee = IsCanControl && input.actions["Melee"].WasPressedThisFrame();
        TryManaSkill = IsCanControl && input.actions["ManaSkillMode"].IsPressed();
        TryLockOnToggle = IsCanControl && input.actions["LockOnToggle"].WasPressedThisFrame();

        if (!TryManaSkill)
        {
            TryDash = IsCanControl && input.actions["Dash"].WasPressedThisFrame();
            TryJump = IsCanControl && input.actions["Jump"].WasPressedThisFrame();
            TryDrain = IsCanControl && input.actions["Drain"].IsPressed();
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
    }

    void AnimStart()
    {
        InputDir = Vector3.zero;
        InputLook = input.actions["Look"].ReadValue<Vector2>();
    }
}
