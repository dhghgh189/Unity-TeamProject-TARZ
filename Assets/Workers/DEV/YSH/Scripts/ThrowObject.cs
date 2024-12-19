using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowObject : MonoBehaviour, IDrainable
{
    private bool isCollected;
    private Rigidbody rigid;

    // test
    public bool IsCollected { get { return isCollected; } set { isCollected = value; } }

    Coroutine drainRoutine;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        if (drainRoutine != null)
        {
            StopCoroutine(drainRoutine);
            drainRoutine = null;
        }
    }

    public void Throw(Vector3 dir, float throwForce)
    {
        rigid.AddForce(dir * throwForce, ForceMode.Impulse);
    }

    public void Get(PlayerController player)
    {
        isCollected = true;

        if (drainRoutine != null)
            StopDrain(null);

        player.AddObjectStack(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        rigid.velocity = Vector3.zero;

        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            isCollected = false;
            return;
        }

        Destroy(gameObject);
    }

    public void DoDrain(DrainManager owner)
    {
        if (drainRoutine != null)
            return;

        rigid.useGravity = false;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        drainRoutine = StartCoroutine(DrainRoutine(owner));
    }

    private IEnumerator DrainRoutine(DrainManager owner)
    {
        Vector3 toPlayer;
        while (true)
        {
            toPlayer = (owner.Player.transform.position + (Vector3.up * 0.5f))
                - transform.position;

            rigid.velocity = toPlayer.normalized * owner.DrainSpeed;

            yield return null;
        }
    }

    public void StopDrain(DrainManager owner)
    {
        if (drainRoutine == null)
            return;

        StopCoroutine(drainRoutine);
        rigid.useGravity = true;
        rigid.velocity = Vector3.zero;
        rigid.constraints = RigidbodyConstraints.None;
        drainRoutine = null;
    }
}
