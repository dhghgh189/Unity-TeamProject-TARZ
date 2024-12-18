using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrainManager : MonoBehaviour
{
    [SerializeField] private float maxRadius;
    [SerializeField] private float drainSpeed;
    [SerializeField] LayerMask whatIsTarget;
    [SerializeField] PlayerController player; 

    private SphereCollider col;
    private float minRadius;

    private Coroutine drainRoutine;

    public PlayerController Player => player;
    public float DrainSpeed => drainSpeed;

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        minRadius = col.radius;
    }

    public void StartDrain()
    {
        if (drainRoutine != null)
            return;

        drainRoutine = StartCoroutine(DrainRoutine());
    }

    IEnumerator DrainRoutine()
    {
        col.radius = minRadius;
        // �ݶ��̴��� �������� �̵�
        transform.localPosition = Vector3.zero;
        while (true)
        {
            if (col.radius < maxRadius)
            {
                col.radius = Mathf.MoveTowards(col.radius, maxRadius, drainSpeed * Time.deltaTime);
            }
            else
            {
                col.radius = maxRadius;
                yield break;
            }

            yield return null;
        }
    }

    public void StopDrain()
    {
        // �ݶ��̴��� ���� ���� �ܷ� ����������.
        // ��Ȱ��ȭ �ϴ°����δ� Exit ȣ���� �ȵǹǷ� ���������� ����� �ϱ� ����
        transform.localPosition = Vector3.up * 100f;

        if (drainRoutine != null)
            StopCoroutine(drainRoutine);

        drainRoutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDrainable drainable = other.GetComponent<IDrainable>();
        if (drainable != null)
        {
            drainable.DoDrain(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDrainable drainable = other.GetComponent<IDrainable>();
        if (drainable != null)
        {
            drainable.StopDrain(this);
        }
    }
}
