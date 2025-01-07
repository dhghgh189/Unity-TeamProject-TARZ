using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Interactioner : MonoBehaviour
{
    #region 특수 오브젝트 스크립트별 참조
    private Box_Type boxType;
    private ThrowBox_Bomb bomb;
    #endregion

    [SerializeField] public Transform GrabPos;
    [SerializeField] public SpecialThrowOBJ_Base SpecialOBJ;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject RangeCircle;

    public bool IsGrabing = false;
    private int interactionLayer;
    private int interactionGrabLayer;
    private Coroutine GrabRoutineCheck;
    private Coroutine RotationRoutineCheck;

    private PlayerController playerController;
    private LineRenderer lineRenderer;
    private List<GameObject> interactionOBJs = new();

    [Header("인식 범위")]
    [SerializeField] float range;
    [SerializeField] float angle;
    [Header("특수 오브젝트 사용 시 관련된 수치 목록")]
    [SerializeField] float throwForce = 10f;
    [SerializeField] float arc = 0.05f;


    private void Start() => Init();

    void Init()
    {
        boxType = Box_Type.None;
        playerController = GetComponentInParent<PlayerController>();
        lineRenderer = GetComponentInParent<LineRenderer>();
        interactionLayer = LayerMask.NameToLayer("Is_Interaction");
        interactionGrabLayer = LayerMask.NameToLayer("Is_Interaction_Grab");
        lineRenderer.enabled = false;

        RangeCircle.SetActive(false);
    }


    private void Update()
    {
        if (playerController.PInput.TryInteraction)
        {
            // 이미 특수 오브젝트를 들고 있는 상황에서는 다른 물체와 상호작용이 불가능하다.
            if (SpecialOBJ != null)
            {
                Grab_KickDown();
                return;
            }

            // 타겟 설정
            target = SelectInteraction(interactionOBJs);
            if (target == null || !target.activeSelf) return;

            // 상호작용한 오브젝트를 바라보는 코드
            Vector3 dir = new Vector3
                (target.transform.position.x, transform.parent.position.y, target.transform.position.z) - transform.parent.position;
            if (RotationRoutineCheck == null) RotationRoutineCheck = StartCoroutine(RotateTransform(transform.parent, dir));


            // 위에서 설정된 타겟이 특수 오브젝트일 경우, 해당 오브젝트를 습득하는 함수를 실행한다.
            if (SpecialOBJ != null)
            {
                Grab_KickDown();
                return;
            }

            // 타겟 내부의 Activate 함수를 통해, 타겟과만 상호작용을 수행한다.
            target.GetComponent<Interaction_Ibase_Activate>().Activate();
            // 상호작용이 수행되면 타겟을 비워, 바로 다음 타겟을 설정할 수 있도록 구성한다.
            target = null;
        }

        // 특수 오브젝트를 집은 상태에서 기본 공격을 수행할 경우, 물체를 던질 수 있다.
        if (IsGrabing && playerController.PInput.Input.actions["Throw"].WasPressedThisFrame())
        {
            IsGrabing = false;
        }
    }

    /// <summary>
    /// 상호작용 수행 시 플레이어가 상호작용한 오브젝트를 향해 자연스럽게 회전하는 코루틴.
    /// 실행되는 동안 플레이어는 다른 동작을 수행할 수 없다.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="target"></param>
    /// <returns></returns>
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
        RotationRoutineCheck = null;
        yield break;
    }


    /// <summary>
    /// 오버랩 스피어로 확인한 오브젝트들을 거리순으로 재정렬하고, 그 중 플레이어와 가장 가까운 게임 오브젝트를 반환한다.
    /// 만약 가장 가까운 오브젝트가 특수 오브젝트 레이어일 경우, 플레이어가 인지 범위 내에 들어와 있을 때 해당 오브젝트를 집어들 수 있다.
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
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
            SpecialOBJ = targets.First().GetComponent<SpecialThrowOBJ_Base>();

            if (SpecialOBJ.trigger == null || !SpecialOBJ.trigger.gameObject.activeSelf) return null;
            if (SpecialOBJ.trigger.IsPlayerIn == false)
            {
                SpecialOBJ = null;
                return null;
            }

            SpecialOBJ.playerController = playerController;
        }

        return targets.First();
    }


    /// <summary>
    /// 오버랩 스피어를 통해, 범위 내에 존재하는 상호작용 가능한 오브젝트들을 리스트에 저장하고 반환한다.
    /// </summary>
    /// <returns></returns>
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


    /// <summary>
    /// 특수 오브젝트 습득 시 특수 오브젝트를 들고 있는 상태로 진입하기 위한 함수.
    /// </summary>
    void Grab_KickDown()
    {
        // 이미 특수 오브젝트를 잡고 있을 경우, 아래 코드를 실행하지 않는다.
        if (IsGrabing) return;

        // 새로운 특수 오브젝트를 습득함과 동시에, 해당 상태를 유지하는 코루틴을 실행한다.
        IsGrabing = true;
        GrabRoutineCheck = StartCoroutine(CheckGrabing());
        // Activate_Grab을 실행해 물체를 interactioner의 자식으로 두어 함께 이동이 가능하도록 한다.
        target.GetComponent<Interaction_Ibase_GrabAct>().Activate_Grab();

        if (IsGrabing != true)
        {
            GrabEnding();
            SpecialOBJ = null;
            target = null;
        }
    }


    /// <summary>
    /// 특수 오브젝트를 들고 있는 상태를 유지하기 위한 코루틴.
    /// 도중에 습득한 오브젝트가 사라질 경우, isGrabing을 false 하여 코루틴 종료 후 다른 오브젝트와 상호작용이 가능하도록 구성했다.
    /// 
    /// 해당 코루틴이 실행되는 동안 실행되는 기능 목록은 다음과 같다.
    /// 1. 플레이어의 스피드는 기존 스피드의 1/3이 된다.
    /// 2. 포물선을 통해, 오브젝트가 던져질 경우의 오브젝트의 움직임을 확인할 수 있다.
    /// 3. 포물선 끝은 던져진 오브젝트가 도달하는 위치로, 오브젝트 종류에 따라 특수 기능 발동의 범위를 시각적으로 확인할 수 있다.
    /// 
    /// 해당 코루틴이 끝날 때 실행되는 기능은 다음과 같다. : 던지기 기능
    /// 1. 플레이어는 기본 공격 키를 통해 오브젝트를 던질 수 있다.
    /// 2. 던져진 오브젝트는 어딘가에 부딫힐 때 특수 기능을 발동한다. (종류에 따라 범위 공격이 가해질 때도 있다.)
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckGrabing()
    {
        lineRenderer.enabled = true;
        Destroy(SpecialOBJ.rigidOBJ);

        // 플레이어의 스피드 = 기존의 1/3
        float curSpeed = playerController.Stat.MoveSpeed;
        playerController.Stat.MoveSpeed = curSpeed / 3f;

        while (IsGrabing)
        {
            if (target == null || !target.activeSelf) IsGrabing = false;
            Check_BoxPath(arc);
            yield return null;
        }

        // 오브젝트를 던졌을 때, 플레이어의 속도는 다시 원래대로 돌아온다.

        playerController.Stat.MoveSpeed = curSpeed;
        GrabRoutineCheck = null;
        lineRenderer.enabled = false;
        // 오브젝트가 독립적으로 움직일 수 있도록 자식 종속성을 해제한다.
        this.transform.DetachChildren();
        GrabEnding();

        // 리지드바디를 통해 물체를 던지는 함수 호출
        ThrowSpeOBJ(SpecialOBJ.GetComponent<Rigidbody>());
        SpecialOBJ = null;
        yield break;
    }


    /// <summary>
    /// 특수 오브젝트를 습득 후 던지는 것에 성공하였을 때 일어나는 코드.
    /// 특수 오브젝트의 Rigidbody를 추가해 물리 작용이 가능하도록 구성하였다.
    /// </summary>
    void GrabEnding()
    {
        SpecialOBJ.AddComponent<Rigidbody>();
        SpecialOBJ.isThrowing = true;
        SpecialOBJ.col.enabled = true;
        SpecialOBJ.playerController = null;
        bomb = null;
        boxType = Box_Type.None;
        if (RangeCircle.activeSelf) RangeCircle.SetActive(false);
    }


    /// <summary>
    /// 특수 오브젝트가 던져졌을 때의 경로를 파악하기 위한 함수.
    /// 라인 렌더러가 갖고 있는 position 수 만큼 반복하여, 포물선을 이루도록 한다. position의 수가 많을 수록 곡선은 더 자연스러워진다.
    /// i 수치에 곱해지는 소수값이 작을수록 포물선은 촘촘해지나, 그만큼 짧아진다.
    /// i 수치에 곱해지는 소수값이 클 수록 포물선은 길어지나, 그만큼 촘촘하지 않아 각지게 출력된다.
    /// 
    /// 현재 카메라가 y축으로 움직이지 않아 범위 확인이 어렵다. 추후 개선이 필요해보임
    /// </summary>
    void Check_BoxPath(float t)
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            // 포물선 운동 공식을 응용하여 식을 작성하였다.
            Vector3 point = 0.5f * Physics.gravity * Mathf.Pow(i * t, 2) +
                (playerController.transform.forward + (playerController.transform.up * 0.3f)) * throwForce * (i * t);
            // 선은 붙잡은 오브젝트의 위치에서부터 시작됨
            point += GrabPos.position;
            lineRenderer.SetPosition(i, point);
        }

        boxType = SpecialOBJ.box_type;
        switch (boxType)
        {
            case Box_Type.Nomal: break;
            case Box_Type.Bomb:
                {
                    if (bomb == null) bomb = SpecialOBJ.GetComponent<ThrowBox_Bomb>();
                    if (RangeCircle.activeSelf != true) RangeCircle.SetActive(true);
                    bomb.CheckPath(RangeCircle, lineRenderer.GetPosition(lineRenderer.positionCount - 7));
                    break;
                }
            default:
                boxType = Box_Type.None;
                break;
        }
    }


    /// <summary>
    /// 특수 오브젝트가 던져졌을 때 실행되는 코드.
    /// </summary>
    void ThrowSpeOBJ(Rigidbody rigid)
    {
        rigid.AddForce
            ((playerController.transform.forward + (playerController.transform.up * 0.3f))
            * throwForce, ForceMode.Impulse);
    }

    //========================================================================


    /// <summary>
    /// 유니티 Scene 상에서 상호작용 인식범위를 확인하기 위한 OnDrawGizmos
    /// </summary>
    private void OnDrawGizmos()
    {
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
    }

    /// <summary>
    /// 코루틴 실행 도중 게임이 종료되었을 때를 대비한 OnDisable 함수
    /// </summary>
    void OnDisable()
    {
        if (RotationRoutineCheck != null)
        {
            StopCoroutine(RotationRoutineCheck);
            RotationRoutineCheck = null;
        }
        if (GrabRoutineCheck != null)
        {
            StopCoroutine(GrabRoutineCheck);
            GrabRoutineCheck = null;
        }
        if (lineRenderer.enabled != false)
        {
            lineRenderer.enabled = false;
        }
        if (boxType != Box_Type.None)
        {
            boxType = Box_Type.None;
        }
        if (bomb != null)
        {
            bomb = null;
        }
    }
}
