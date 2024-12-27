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

    public bool[] ManaKeyPressed;
    public KeyCode[] ManaSkillKey;

    private void Start()
    {
        TryInputDown.Add(InputDir);
        ManaKeyPressed = new bool[Define.MANASKILL_MAXCOUNT];
        ManaSkillKey = new KeyCode[Define.MANASKILL_MAXCOUNT];
        for (int i = 0; i < ManaSkillKey.Length; i++)
        {
            ManaSkillKey[i] = KeyCode.Alpha1 + i;
        }
    }

    void Update()
    {
        InputDir = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // 패드 지원되도록 구성해야 함
        TryDash = Input.GetButtonDown("Dash");
        TryJump = Input.GetButtonDown("Jump");
        TryThrow = Input.GetButtonDown("Throw");
        TryMelee = Input.GetButtonDown("Melee");
        TryDrain = Input.GetButton("Drain");
        TryInteraction = Input.GetKeyDown(KeyCode.E);
        TryManaSkill = Input.GetKey(KeyCode.F9);
        if (TryManaSkill)
        {
            for (int i = 0; i < ManaSkillKey.Length; i++)
            {
                ManaKeyPressed[i] = Input.GetKeyDown(ManaSkillKey[i]);
            }
        }
    }
}
