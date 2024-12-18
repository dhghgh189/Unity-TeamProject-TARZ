using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum EMachineType { Movement, Attack }

public class PlayerController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private DrainManager drainManager;
    [SerializeField] Renderer render;

    public EState currentStateView;

    public Color BaseColor { get; private set; }

    public PlayerFSM Fsm { get; private set; }
    public Animator Anim { get { return anim; } }
    public PlayerInput PInput { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerStat Stat { get; private set; }
    public PlayerAttack Attack { get; private set; }
    public DrainManager Drain { get { return drainManager; } }
    public Renderer Render { get { return render; } }

    void Awake()
    {
        anim = GetComponent<Animator>();

        PInput = GetComponent<PlayerInput>();
        Movement = GetComponent<PlayerMovement>();
        Stat = GetComponent<PlayerStat>();
        Attack = GetComponent<PlayerAttack>();

        Fsm = new PlayerFSM(this);

        BaseColor = render.material.color;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Fsm.OnUpdate();
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

    public void ChangeState(EState state)
    {
        Fsm.ChangeState(state);
    }

    public BaseState<PlayerController> GetCurrentState()
    {
        return Fsm.CurrentState;
    }
}
