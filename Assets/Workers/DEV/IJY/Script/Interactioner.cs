using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Interactioner : MonoBehaviour
{
    private LayerMask interactionLayer;
    private Coroutine UpdateCoroutine;
    private List<GameObject> interactionOBJs = new();
    [SerializeField] private PlayerController PlayerController;
    [SerializeField] private GameObject target;
    [Header("인식 범위")]
    [SerializeField] float range;
    [SerializeField] float angle;

    private void Start() => interactionLayer = LayerMask.GetMask("Is_Interaction");

    private void Update()
    {
        if (PlayerController.PInput.TryInteraction)
        {
            target = SelectInteraction(interactionOBJs);

            if (target == null || !target.activeSelf) return;

            // 상호작용 대상을 바라보는 코드. 추후 자연스럽게 수정 예정
            Vector3 dir = new Vector3(target.transform.position.x, transform.parent.position.y, target.transform.position.z) - transform.parent.position;
            transform.parent.rotation = Quaternion.LookRotation(dir).normalized;

            //transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
            //transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);
            //transform.parent.rotation = Quaternion.LookRotation(dir);

            target.GetComponent<Base_InteractionOBJ>().Activate();
            target = null;
        }
    }

    GameObject SelectInteraction(List<GameObject> targets)
    {
        targets = CheckMonsters();
        if (targets.Count <= 0) return null;

        var target = from targeting in targets
                     orderby Vector3.Distance(targeting.transform.position, transform.position) ascending
                     select targeting;
        targets = target.ToList();
        
        return targets.First();
    }

    List<GameObject> CheckMonsters()
    {
        List<GameObject> _targets = new List<GameObject>();
        Collider[] collider = Physics.OverlapSphere(transform.position, range, interactionLayer);

        // 인식할 몬스터 각도의 범위 설정
        foreach (Collider _col in collider)
        {
            Vector3 source = transform.position; source.y = 0;
            Vector3 destination = _col.transform.position; destination.y = 0;
            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);

            if (targetAngle > angle * 0.5f) continue;
            _targets.Add(_col.gameObject);
        }

        return _targets;
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
