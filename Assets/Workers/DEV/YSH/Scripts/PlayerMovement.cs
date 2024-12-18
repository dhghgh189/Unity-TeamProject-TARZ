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
        // ���� ī�޶� ������ �������� �̵��� �����Ѵ�.
        Vector3 velocity = (mainCamTrf.right * moveVelocity.x) + (mainCamTrf.forward * moveVelocity.z);
        rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);

        if (velocity != Vector3.zero)
        {
            LookRotation(velocity.normalized);
        }

        //rigid.velocity = new Vector3(moveVelocity.x, rigid.velocity.y, moveVelocity.z);

        //if (moveVelocity != Vector3.zero)
        //{
        //    LookRotation(moveVelocity.normalized);
        //}
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
        // velocity�� -�� ��Ȳ���� ������ ���� �Ǵ� ������ �����ϱ� ����
        // velocity �ʱ�ȭ 1ȸ ����
        rigid.velocity = Vector3.zero;

        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void GroundCheck()
    {
        isGrounded = Physics.CheckBox(transform.position + transform.up * 0.05f, new Vector3(0.5f, 0.1f, 0.5f), Quaternion.identity, whatIsGround);
    }
}
