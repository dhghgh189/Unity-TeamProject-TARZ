using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Zenject;
using static SkillEnum;

public enum EMachineType { Movement, Attack }

public enum EMpAmountType { Melee, Throw, Length }

public class PlayerController : MonoBehaviour, IDamagable
{
    [Inject] private StatModel stat;

    private Animator anim;

    [SerializeField] private AblityAdapter adapter;
    [SerializeField] private DrainManager drainManager;
    [SerializeField] Renderer render;

    public EState currentStateView;

    public Color BaseColor { get; private set; }

    public PlayerSkillHandler SkillHandler;
    public PlayerFSM Fsm { get; private set; }
    public Animator Anim { get { return anim; } }
    public PlayerInput PInput { get; private set; }
    public StatModel Stat { get { return stat; } }
    public PlayerMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }
    public DrainManager Drain { get { return drainManager; } }
    public Renderer Render { get { return render; } }

    void Awake()
    {
        anim = GetComponent<Animator>();

        PInput = GetComponent<PlayerInput>();
        Movement = GetComponent<PlayerMovement>();
        Attack = GetComponent<PlayerAttack>();

        SkillHandler = GetComponent<PlayerSkillHandler>();

        Fsm = new PlayerFSM(this, GetComponent<AblityAdapter>());

        BaseColor = render.material.color;

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // test
    bool isLocked;
    private void Update()
    {
        Fsm.OnUpdate();

        // test
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Cursor.visible = isLocked;
            Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
            isLocked = !isLocked;
        }

        return;
    }

    private void FixedUpdate()
    {
        Fsm.OnFixedUpdate();
        return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + transform.up * 0.06f, new Vector3(0.5f, 0.1f, 0.5f));
    }

    public void AddObjectStack(ThrowObject tobj)
    {
        Attack.AddObjectStack(tobj);
    }

    public void ChangeState(EState state)
    {
        Fsm.ChangeState(state);
    }

    public BaseState<PlayerController> GetCurrentState()
    {
        return Fsm.CurrentState;
    }

    public void TakeDamage(float damage)
    {
        Stat.CurrentHp -= damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            SkillHandler.PlayerCollision((ActTimingType)Enum.Parse(typeof(ActTimingType), currentStateView.ToString()), collision.gameObject);
        }
    }

}
