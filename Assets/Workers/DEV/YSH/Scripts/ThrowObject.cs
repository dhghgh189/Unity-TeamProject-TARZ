using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowObject : MonoBehaviour, IDrainable
{
    [SerializeField] private LayerMask whatIsTarget;

    private bool isCollected;
    private Rigidbody rigid;
    private PlayerController owner;

    private int damage;

    public bool IsCollected { get { return isCollected; } set { isCollected = value; } }

    Coroutine drainRoutine;

    private List<IEffect> throwEffects;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        throwEffects = new List<IEffect>();
    }

    private void OnDisable()
    {
        if (drainRoutine != null)
        {
            StopCoroutine(drainRoutine);
            drainRoutine = null;
        }
    }

    public void AddEffect(IEffect effect)
    {
        throwEffects.Add(effect);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
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

        owner = player;
        player.AddObjectStack(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        rigid.velocity = Vector3.zero;

        // 부딪힌 오브젝트가 target이 아니면
        if (((1 << other.gameObject.layer) & whatIsTarget.value) == 0)
        {
            isCollected = false;

            if (throwEffects.Count > 0)
                throwEffects.Clear();

            return;
        }

        // effect 발동
        ActiveThrowEffects(other.gameObject);

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(damage);
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

    public void ActiveThrowEffects(GameObject target)
    {
        foreach(var effect in throwEffects)
        {
            effect.Activate(owner.gameObject, target);
        }

        throwEffects.Clear();
    }
}
