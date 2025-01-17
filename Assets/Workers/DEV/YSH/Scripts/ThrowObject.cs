using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowObject : MonoBehaviour, IDrainable
{
    [SerializeField] private RandomModeling setModeling;
    [SerializeField] private LayerMask whatIsTarget;

    [HideInInspector] public AblityAdapter adapter;
    [HideInInspector] public PlayerSkillHandler handler;

    [SerializeField] private bool isCollected;
    private Rigidbody rigid;
    private PlayerController owner;

    private float damage;

    public ThrowObjectUpgrade Upgrade { get; private set; }

    public bool IsCollected { get { return isCollected; } set { isCollected = value; } }

    Coroutine drainRoutine;

    private List<IEffect> throwEffects;

    private BoxCollider coll;

    private AudioClip hitClip;

    private void Awake()
    {
        setModeling = FindAnyObjectByType<RandomModeling>();
        setModeling.SetRandom(this.gameObject, setModeling.ThrowObjectOBJs);

        Upgrade = GetComponent<ThrowObjectUpgrade>();
        throwEffects = new List<IEffect>();
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        adapter = FindAnyObjectByType<AblityAdapter>(FindObjectsInactive.Include);
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

    public void SetInfo(float damage, AudioClip clip = null)
    {
        this.damage = damage;
        hitClip = clip;
    }

    public void Throw(Vector3 dir, float throwForce)
    {
        foreach (var item in adapter.GetFunctionList())
        {
            switch (item)
            {
                case SkillEnum.UniqueFunctionType.GuidedFuncion:
                    GetComponent<GuidedFuncion>().StartCheckTarget();
                    break;
            }
        }

        handler.Use(gameObject);
        rigid.rotation = Quaternion.identity;
        rigid.AddForce(dir * throwForce, ForceMode.Impulse);
    }

    public void Get(PlayerController player)
    {
        if (drainRoutine != null)
            StopDrain(null);

        owner = player;
        handler = owner.SkillHandler;
        player.AddObjectStack(this);

        // 던질 때 플레이어랑 부딪히는 문제 방지
        Debug.Log("충돌 해제");
        rigid.excludeLayers |= (1 << owner.gameObject.layer); 

        isCollected = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        // 획득 전에 다른 오브젝트에 닿아서 처리되는 것을 방지
        if (!isCollected)
            return;

        rigid.velocity = Vector3.zero;

        // 무시했던 충돌을 다시 적용
        Debug.Log("충돌 적용");
        rigid.excludeLayers = 0;

        // 부딪힌 오브젝트가 target이 아니면
        if (((1 << other.gameObject.layer) & whatIsTarget.value) == 0)
        {
            // 스택에 들어가는 과정에서 Throw Object끼리 충돌하여
            // isCollected가 초기화 되는 것을 방지
            if (gameObject.transform.parent == null)
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
            if (gameObject.layer.Equals(LayerMask.NameToLayer("ThrowObject")))
            {
                owner.Stat.CurrentMp += owner.Stat.GetMpGain(EMpAmountType.Throw);
                owner.SkillHandler.ThrowObjectCollision(gameObject, other.gameObject);
            }

            if (hitClip != null)
            {
                SoundManager.PlaySFX(hitClip);
            }
        }

        Destroy(gameObject);
    }

    public void DoDrain(DrainManager owner)
    {
        if (drainRoutine != null)
            return;

        bool any = true;

        foreach(var item in adapter.GetFunctionList())
        {
            switch(item)
            {
                case SkillEnum.UniqueFunctionType.ThrowObjectConvertMine:
                    ThrowObjectConvertMine mine = GetComponent<ThrowObjectConvertMine>();
                    mine.Change(owner.Player.Stat.DefaultPowerPer);
                    any = false;
                    break;
                case SkillEnum.UniqueFunctionType.ThrowObjectUpgrade:
                    GetComponent<ThrowObjectUpgrade>().IsUpgraded = true;
                    any = false;
                    break;
            }
        }


        if (!any) return;

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
        foreach (var effect in throwEffects)
        {
            effect.Activate(owner.gameObject, target);
        }

        throwEffects.Clear();
    }
}
