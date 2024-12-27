using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactioner : MonoBehaviour
{
    private LayerMask interactionLayer;
    private LayerMask interactionGrabLayer;
    private Example_Interaction_GrabScript exampleScript;

    private Coroutine UpdateCoroutine;
    private List<GameObject> interactionOBJs = new();
    [SerializeField] private PlayerController PlayerController;
    [SerializeField] private GameObject target;
    [Header("인식 범위")]
    [SerializeField] float range;
    [SerializeField] float angle;

    private void Start() => Init();

    void Init()
    {
        interactionLayer = LayerMask.GetMask("Is_Interaction");
        interactionGrabLayer = LayerMask.GetMask("Is_Interaction_Grab");
    }

    private void Update()
    {
        if (PlayerController.PInput.TryInteraction)
        {
            if (exampleScript != null)
            {
                Grab_KickDown();
                return;
            }

            Debug.Log("1 이건 또 무슨 버그야 미친것");
            target = SelectInteraction(interactionOBJs);
            if (target == null || !target.activeSelf) return;

            Debug.Log("2 이건 또 무슨 버그야 미친것");
            // 상호작용 대상을 바라보는 코드. 추후 자연스럽게 수정 예정
            Vector3 dir = new Vector3
                (target.transform.position.x, transform.parent.position.y, target.transform.position.z) - transform.parent.position;
            transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, Quaternion.LookRotation(dir), 2f);

            if (exampleScript != null)
            {
                Grab_KickDown();
                return;
            }

            Debug.Log("3 이건 또 무슨 버그야 미친것");
            target.GetComponent<Base_InteractionOBJ>().Activate();
            target = null;

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
            Debug.Log("4 이건 또 무슨 버그야 미친것");
            exampleScript = targets.First().GetComponent<Example_Interaction_GrabScript>();
            exampleScript.playerController = PlayerController;
        }

        Debug.Log("5 이건 또 무슨 버그야 미친것");
        return targets.First();
    }

    List<GameObject> CheckInteraction()
    {
        List<GameObject> _targets = new List<GameObject>();
        Collider[] collider = Physics.OverlapSphere(transform.position, range);

        // 인식할 몬스터 각도의 범위 설정
        foreach (Collider _col in collider)
        {
            // 이거에 걸러지는거니
            if (_col.gameObject.layer != interactionLayer || _col.gameObject.layer != interactionGrabLayer) continue;
            Debug.Log("라");

            Vector3 source = transform.position; source.y = 0;
            Vector3 destination = _col.transform.position; destination.y = 0;
            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);

            if (targetAngle > angle * 0.5f) continue;
            _targets.Add(_col.gameObject);
        }

        foreach (GameObject _col in _targets)
        {
            // 이상하다 이거 왜이러지?
            Debug.Log($"뭐가 있나요 : {_col.name}");
        }

        return _targets;
    }

    void Grab_KickDown()
    {
        if (target == null || !target.activeSelf)
        {
            Debug.Log("6 이건 또 무슨 버그야 미친것");
            exampleScript.GrabOnOff = false;
            exampleScript = null;
            return;
        }

        Debug.Log("7 이건 또 무슨 버그야 미친것");
        exampleScript.GrabOnOff = !exampleScript.GrabOnOff;
        target.GetComponent<Base_InteractionOBJ_Grab>().Activate_Grab();

        if (exampleScript.GrabOnOff != true)
        {
            Debug.Log("8 이건 또 무슨 버그야 미친것");
            exampleScript = null;
            target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
    }
}
