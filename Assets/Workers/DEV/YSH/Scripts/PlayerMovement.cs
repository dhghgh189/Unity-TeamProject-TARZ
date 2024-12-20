using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    public Vector3 CurrentVelocity => rigid.velocity;

    private Transform mainCamTrf;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        mainCamTrf = Camera.main.transform;
    }

    private void Update()
    {
        GroundCheck();
    }

    public void Move(Vector3 moveVelocity)
    {
        Vector3 ForwardDir = new Vector3(mainCamTrf.forward.x, 0f, mainCamTrf.forward.z).normalized;
        Vector3 RightDir = new Vector3(mainCamTrf.right.x, 0f, mainCamTrf.right.z).normalized;
        Vector3 moveDir = ForwardDir * moveVelocity.z + RightDir * moveVelocity.x;

        // 현재 카메라 방향을 기준으로 이동을 진행한다.
        Vector3 velocity = (mainCamTrf.right * moveVelocity.x) + (mainCamTrf.forward * moveVelocity.z);
        rigid.velocity = new Vector3(velocity.x + moveDir.x, rigid.velocity.y, velocity.z + moveDir.z);

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
