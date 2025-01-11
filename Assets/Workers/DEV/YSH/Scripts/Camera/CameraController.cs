using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject.SpaceFighter;

public class CameraController : MonoBehaviour
{
    private PlayerController player;
    private PlayerInputHandler playerInput;

    [SerializeField] public bool IsAutoLockOn;
    [SerializeField] public Transform target;

    private Transform lookAt;

    [Space(5f)]
    [SerializeField] private float sensitivity;
    public float Sensitivity { get { return sensitivity; } set { sensitivity = value; } }
    [SerializeField] private Vector3 delta;

    // 카메라가 가려지면 안되는 오브젝트의 레이어를 설정
    [SerializeField] private LayerMask whatIsWall;

    private float yAngle;
    private Camera mainCam;

    private RaycastHit hit;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Include);
        playerInput = FindAnyObjectByType<PlayerInputHandler>(FindObjectsInactive.Include);

        lookAt = player.cameraLookPos;

        mainCam = Camera.main;
        transform.position = lookAt.position;

        mainCam.transform.position = transform.position + delta;
        mainCam.transform.SetParent(transform);

        IsAutoLockOn = false;
    }

    private void Update()
    {
        yAngle += playerInput.InputLook.x * sensitivity * Time.deltaTime;
    }

    void LateUpdate()
    {
        Rotate();
        Move(Vector3.zero);
        if (!AvoidWall())
        {
            // 가리는 벽이 없는 경우 카메라는 controller의 로컬 방향을 기준으로 delta만큼 떨어진 곳에 위치한다. 
            mainCam.transform.position = transform.position + (transform.right * delta.x) + (transform.up * delta.y) + (transform.forward * delta.z);
        }
    }

    private bool AvoidWall()
    {
        Vector3 targetPos = transform.position + (transform.forward * delta.z) + (transform.right * delta.x) + (transform.up * delta.y);
        Vector3 toTarget = targetPos - transform.position;
        if (!Physics.Raycast(transform.position, toTarget.normalized, out hit, toTarget.magnitude, whatIsWall))
            return false;

        // 가리는 벽이 있는 경우 해당 벽위치로 이동한다.
        mainCam.transform.position = hit.point;
        return true;
    }

    public void Move(Vector3 r)
    {
        transform.position = lookAt.position;
    }

    public void Rotate()
    {
        if (IsAutoLockOn == false)
        {
            transform.rotation = Quaternion.Euler(0, yAngle, 0);
        }
        else
        {
            if (target == null || !target.gameObject.activeSelf)
            {
                IsAutoLockOn = false;
                return;
            }
            Vector3 dir = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), sensitivity * Time.deltaTime);

            //transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
    }
}
