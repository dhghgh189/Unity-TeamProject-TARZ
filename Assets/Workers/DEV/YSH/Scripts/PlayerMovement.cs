using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float dashTime;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxSlope = 50f;

    public float JumpForce { get { return jumpForce; } }
    public float DashTime { get { return dashTime; } set { dashTime = value; Debug.Log("Dash가 변경되었다!"); } }
    public bool IsGrounded { get { return isGrounded; } }
    public Vector3 CurrentVelocity => rigid.velocity;

    private Transform mainCamTrf;

    public Rigidbody Rigid => rigid;

    RaycastHit slopeHit;
    private bool isSlope;
    public bool IsSlope { get { return isSlope; } }

    private PlayerController player;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
        mainCamTrf = Camera.main.transform;
    }

    private void Update()
    {
        GroundCheck();
        SlopeCheck();

        // 유효한 경사면에 서있는 경우
        if (isGrounded && isSlope)
        {
            // 중력을 off
            rigid.useGravity = false;
        }
        else
        {
            rigid.useGravity = true;
        }
    }

    #region Anim Event
    public void FootStep()
    {
        // 플레이어 Move시 발생하는 이벤트
        // 사운드 재생 (임시)
        SoundManager.PlaySFX(SoundManager.SoundData_P.FootStep_Grass);
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
        if (velocity != Vector3.zero)
        {
            LookRotation(velocity.normalized);
        }

        // 유효한 경사면에 서 있는 경우
        // Jump를 하자마자 isGrounded가 바뀌지 않기 때문에 State로 체크한다. 
        if (player.Fsm.CurrentState.type != EState.Jump
            && player.Fsm.CurrentState.type != EState.Fall
            && isSlope)
        {
            // 기존의 방향을 경사면의 방향에 맞춰 투영시킨다.
            Vector3 dir = Vector3.ProjectOnPlane(velocity.normalized, slopeHit.normal).normalized;
            velocity = dir * 5f;
            rigid.velocity = velocity;
        }
        else
        {
            // 기존 방향대로 이동
            rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);
        }
    }

    public void LookRotation(Vector3 dir)
    {
        rigid.rotation = Quaternion.Lerp(rigid.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
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
        isGrounded = Physics.CheckBox(transform.position + transform.up * 0.05f, new Vector3(0.25f, 0.1f, 0.25f), Quaternion.identity, whatIsGround);
    }

    // 플레이어 경사 처리
    public void SlopeCheck()
    {
        //Debug.DrawRay(transform.position + transform.up * 0.05f, Vector3.down * 0.2f, Color.red);

        // 플레이어 위치에서 아래방향으로 Raycast 진행
        if (!Physics.Raycast(transform.position + transform.up * 0.05f, Vector3.down, out slopeHit, 0.2f, whatIsGround))
            isSlope = false;    // 닿는게 없으면 false

        // 충돌한 물체의 법선 벡터와 월드기준 위 방향의 각도를 계산
        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

        // 계산한 각도가 maxSlope 내에 있으면 유효한 경사면에 있는것으로 판정
        isSlope = angle != 0 && maxSlope <= 50f;
    }

    public void Stop()
    {
        rigid.velocity = Vector3.zero;
    }
}
