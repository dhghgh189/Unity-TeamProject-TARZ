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
    [SerializeField] private Vector3 delta;

    private float yAngle;
    private Camera mainCam;

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
