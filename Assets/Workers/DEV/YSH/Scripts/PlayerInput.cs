using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 InputDir { get; private set; }
    public bool TryJump { get; private set; }
    public bool TryThrow { get; private set; }
    public bool TryMelee { get; private set; }
    public bool TryDash { get; private set; }
    public bool TryDrain { get; private set; }

    void Update()
    {
        InputDir = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // 패드 지원되도록 구성해야 함
        TryDash = Input.GetKeyDown(KeyCode.LeftShift);
        TryJump = Input.GetKeyDown(KeyCode.Space);
        TryThrow = Input.GetMouseButtonDown(0);
        TryMelee = Input.GetKeyDown(KeyCode.F);
        TryDrain = Input.GetKey(KeyCode.LeftControl);
    }
}
