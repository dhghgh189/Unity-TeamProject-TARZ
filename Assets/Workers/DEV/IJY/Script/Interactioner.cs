using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Interactioner : MonoBehaviour
{
    public bool IsGrabing = false;
    [SerializeField] public Transform GrabPos;
    [SerializeField] public Example_Interaction_GrabScript exampleScript;

    private int interactionLayer;
    private int interactionGrabLayer;
    private Coroutine RoutineCheck;
    private List<GameObject> interactionOBJs = new();

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject target;
    [Header("인식 범위")]
    [SerializeField] float range;
    [SerializeField] float angle;

    private void Start() => Init();

    void Init()
    {
        playerController = GetComponentInParent<PlayerController>();
        interactionLayer = LayerMask.NameToLayer("Is_Interaction");
        interactionGrabLayer = LayerMask.NameToLayer("Is_Interaction_Grab");
    }

    private void Update()
    {
        if (playerController.PInput.TryInteraction)
        {
            if (exampleScript != null)
            {
                Grab_KickDown();
                return;
            }

            target = SelectInteraction(interactionOBJs);
            if (target == null || !target.activeSelf) return;

            Vector3 dir = new Vector3
                (target.transform.position.x, transform.parent.position.y, target.transform.position.z) - transform.parent.position;
            if (RoutineCheck == null) RoutineCheck = StartCoroutine(RotateTransform(transform.parent, dir));

            if (exampleScript != null)
            {
                Grab_KickDown();
                return;
            }

            target.GetComponent<Base_InteractionOBJ>().Activate();
            target = null;
        }

        if (IsGrabing && playerController.PInput.TryThrow)
        {
            IsGrabing = false;
        }
    }

    GameObject SelectInteraction(List<GameObject> targets)
    {
        targets = CheckInteraction();
        if (targets.Count <= 0) return null;

        var target = from targeting in targets
                     orderby Vector3.Distance(targeting.transform.position, transform.position) ascending
                     select targeting;
        targets = target.ToList();

        if (targets.First().layer == interactionGrabLayer)
        {
            exampleScript = targets.First().GetComponent<Example_Interaction_GrabScript>();
            exampleScript.playerController = playerController;
        }

        return targets.First();
    }

    List<GameObject> CheckInteraction()
    {
        List<GameObject> _targets = new List<GameObject>();
        Collider[] collider = Physics.OverlapSphere(transform.position, range);

        // 인식할 몬스터 각도의 범위 설정
        foreach (Collider _col in collider)
        {
            if (_col.gameObject.layer != interactionLayer && _col.gameObject.layer != interactionGrabLayer) continue;

            Vector3 source = transform.position; source.y = 0;
            Vector3 destination = _col.transform.position; destination.y = 0;
            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);

            if (targetAngle > angle * 0.5f) continue;
            _targets.Add(_col.gameObject);
        }

        return _targets;
    }

    void Grab_KickDown()
    {
        if (IsGrabing) return;

        IsGrabing = true;
        StartCoroutine(CheckGrabing());
        target.GetComponent<Base_InteractionOBJ_Grab>().Activate_Grab();

        if (IsGrabing != true)
        {
            GrabEnding();
            target = null;
        }
    }

    IEnumerator RotateTransform(Transform player, Vector3 target)
    {
        playerController.IsAnimStart = true;

        Vector3 targetRotate = Quaternion.LookRotation(target).eulerAngles;
        while (Vector3.Distance(player.eulerAngles, targetRotate) > 1f)
        {
            playerController.PInput.TryInteraction = playerController.PInput.Input.actions["Interact"].WasPressedThisFrame();
            player.rotation = Quaternion.Slerp(player.rotation, Quaternion.LookRotation(target), Time.deltaTime * 10f);
        }

        playerController.IsAnimStart = false;
        RoutineCheck = null;
        yield break;
    }

    IEnumerator CheckGrabing()
    {
        while (IsGrabing)
        {
            //playerController.Stat.MoveSpeed = playerController.Stat.MoveSpeed / 3f;
            if (target == null || !target.activeSelf) IsGrabing = false;
            yield return null;
        }

        Debug.Log("코루틴 끝!");
        this.transform.DetachChildren();
        GrabEnding();
        yield break;
    }

    void GrabEnding()
    {
        exampleScript.AddComponent<Rigidbody>();
        exampleScript.isThrowing = true;
        exampleScript.col.enabled = true;
        exampleScript.playerController = null;
        exampleScript = null;
    }

    private void OnDrawGizmos()
    {
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
    }

    private void OnDisable()
    {
        if (exampleScript != null)
        {
            this.transform.DetachChildren();
            GrabEnding();
        }
    }
}
