using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTrigger : MonoBehaviour
{
    [SerializeField] SphereCollider col;

    [SerializeField] EnemyController owner;

    private void Start()
    {
        StartCoroutine(JustRoutine());
    }

    public void EnableJustTrigger(float range)
    {
        col.radius = range;
        col.enabled = true;
    }

    public void DisableJustTrigger()
    {
        col.enabled = false;
    }

    IEnumerator JustRoutine()
    {
        float elapsedTime = 0;
        int timer = 3;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1f)
            {
                if (timer == 0)
                {
                    //Debug.Log("Monster JustTrigger Activated!");
                    EnableJustTrigger(2f);
                    yield return new WaitForSeconds(0.25f);
                    DisableJustTrigger();

                    timer = 3;
                }
                else
                {
                    //Debug.Log($"Monster Charge : {timer}");
                    timer--;
                }

                elapsedTime = 0;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        Debug.Log($"Just Trigger Enter : {other.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        if (player.Fsm.CurrentState.type == EState.Dash)
        {
            // test
            Debug.Log("Just Success!");

            // 내적을 통한 각도 체크 필요
        }
        else
        {
            Debug.Log($"Just Failed...");
        }
    }
}
