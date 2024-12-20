using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public bool IsAutoLockOn;
    [SerializeField] public Transform target;
    [SerializeField] private Transform lookAt;
    [Space(5f)]
    [SerializeField] private float sensitivity;
    [SerializeField] private Vector3 delta;
    private float mainCamYPos;
    private float yAngle;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        transform.position = lookAt.position;

        mainCam.transform.position = transform.position + delta;
        mainCam.transform.SetParent(transform);
        mainCamYPos = mainCam.transform.position.y;

        IsAutoLockOn = false;
    }

    void LateUpdate()
    {
        Rotate();
        Move(Vector3.zero);
    }

    public void Move(Vector3 r)
    {
        transform.position = lookAt.position;
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCamYPos, mainCam.transform.position.z);

        //transform.position = lookAt.position;
        //Vector3 camPosition = new Vector3(mainCam.transform.position.x, mainCamYPos, mainCam.transform.position.z);
        //mainCam.transform.position = Vector3.SmoothDamp(mainCam.transform.position, camPosition, ref r, smoothTime);
    }

    public void Rotate()
    {
        if (IsAutoLockOn == false)
        {
            //yAngle += Input.GetAxisRaw("Mouse X") * 2f;
            //transform.rotation = Quaternion.Euler(0, yAngle, 0);

            yAngle += Input.GetAxisRaw("Mouse X");
            Vector3 dir = lookAt.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, yAngle, 0), sensitivity * Time.deltaTime);
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
