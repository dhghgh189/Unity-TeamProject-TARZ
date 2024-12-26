using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 InputDir { get; private set; }
    public bool TryJump { get; private set; }
    public bool TryThrow { get; private set; }
    public bool TryMelee { get; private set; }
    public bool TryDash { get; private set; }
    public bool TryDrain { get; private set; }
    public bool TryInteraction { get; private set; }

    void Update()
    {
        InputDir = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // 패드 지원되도록 구성해야 함
        TryDash = Input.GetButtonDown("Dash");
        TryJump = Input.GetButtonDown("Jump");
        TryThrow = Input.GetButtonDown("Throw");
        TryMelee = Input.GetButtonDown("Melee");
        TryDrain = Input.GetKey(KeyCode.LeftControl);
        TryInteraction = Input.GetKeyDown(KeyCode.E);
    }
}
