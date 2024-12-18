using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform lookAt;
    [SerializeField] private Vector3 delta;

    float yAngle;

    Camera mainCam;
    private float mainCamYPos;

    private void Start()
    {
        mainCam = Camera.main;
        transform.position = lookAt.position;
        mainCam.transform.position = transform.position + delta;
        mainCam.transform.SetParent(transform);
        mainCamYPos = mainCam.transform.position.y;
    }

    void LateUpdate()
    {
        Rotate();
        Move();
    }

    public void Move()
    {
        transform.position = lookAt.position;
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCamYPos, mainCam.transform.position.z);
    }

    public void Rotate()
    {
        yAngle += Input.GetAxisRaw("Mouse X") * 2f;
        transform.rotation = Quaternion.Euler(0, yAngle, 0);
    }
}
