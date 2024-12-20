using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTester : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] float _inAirTime; // 체공 시간

    [SerializeField] float _jumpHeight; // Y축 점프 높이

    [SerializeField] float _jumpDistance; // Z축 점프 거리

    private float _elapsedTime = 0;

    private Vector3 _jumpStartPosition;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           if (jumpRoutine == null)
            {
                jumpRoutine = StartCoroutine(JumpRoutine());
            }
        }
    }

    Coroutine jumpRoutine;
    IEnumerator JumpRoutine()
    {
        _jumpStartPosition = transform.position;

        while (_elapsedTime < _inAirTime)
        {
            float yOffset = Mathf.Sin((_elapsedTime / _inAirTime) * Mathf.PI) * _jumpHeight;
            float zOffset = (_elapsedTime / _inAirTime) * _jumpDistance;

            transform.position = new Vector3(_jumpStartPosition.x, _jumpStartPosition.y + yOffset, _jumpStartPosition.z + zOffset);

            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(_jumpStartPosition.x, _jumpStartPosition.y, _jumpStartPosition.z + _jumpDistance);
        jumpRoutine = null;
        _elapsedTime = 0;
    }
}
