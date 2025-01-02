using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float dashTime;
    [SerializeField] private float jumpForce;

    public float JumpForce { get { return jumpForce; } }
    public float DashTime { get { return dashTime; } set { dashTime = value; Debug.Log("Dash가 변경되었다!"); } }
    public bool IsGrounded { get { return isGrounded; } }
    public Vector3 CurrentVelocity => rigid.velocity;

    private Transform mainCamTrf;

    public Rigidbody Rigid => rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        mainCamTrf = Camera.main.transform;
    }

    private void Update()
    {
        GroundCheck();
    }

    #region Anim Event
    public void FootStep()
    {
        // 플레이어 Move시 발생하는 이벤트
    }

    public void DashBoostPlay()
    {
        // 대쉬 초반의 부스트 시작 시 발생하는 이벤트
    }

    public void StampDustPlay()
    {
        // 대쉬 막바지의 착지 시 발생하는 이벤트
    }
    #endregion

    public void Move(Vector3 moveVelocity)
    {
        Vector3 ForwardDir = new Vector3(mainCamTrf.forward.x, 0f, mainCamTrf.forward.z).normalized;
        Vector3 RightDir = new Vector3(mainCamTrf.right.x, 0f, mainCamTrf.right.z).normalized;
        Vector3 moveDir = ForwardDir * moveVelocity.z + RightDir * moveVelocity.x;

        // 현재 카메라 방향을 기준으로 이동을 진행한다.
        Vector3 velocity = (mainCamTrf.right * moveVelocity.x) + (mainCamTrf.forward * moveVelocity.z);
        rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);

        if (velocity != Vector3.zero)
        {
            LookRotation(velocity.normalized);
        }
    }

    public void LookRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
    }

    public void LookAt(Vector3 dir)
    {
        transform.forward = dir;
    }

    public void Jump(float jumpForce)
    {
        // velocity가 -인 상황에서 점프가 진행 되는 문제를 방지하기 위해
        // velocity 초기화 1회 진행
        rigid.velocity = Vector3.zero;

        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void GroundCheck()
    {
        isGrounded = Physics.CheckBox(transform.position + transform.up * 0.05f, new Vector3(0.5f, 0.1f, 0.5f), Quaternion.identity, whatIsGround);
    }
}
