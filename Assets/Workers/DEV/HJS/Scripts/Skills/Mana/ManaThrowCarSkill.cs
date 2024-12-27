using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManaThrowCarDataType { FlightSpeed, HitDamage, ExplosionDamage, ExplosionRange }
public class ManaThrowCarSkill : IManaSkill
{
    private const string KEY_NAME = "ManaThrowCar";
    private ManaSkillDataSO skillData;
    public LinkedList<BaseManaState> Acts { get; private set; }
    public ManaSkillDataSO SkillData { get => skillData; set => skillData = value; }



    public ManaThrowCarSkill(PlayerController owner)
    {
        Acts = new LinkedList<BaseManaState>();
        Acts.AddLast(new ManaThrowCar_1(owner, this));
    }

    public void SetInit(ManaSkillHandler manaSkillHandler)
    {
        manaSkillHandler.ActList = Acts;
        skillData = manaSkillHandler.GetData(KEY_NAME);
    }

    /// <summary>
    /// 1번 동작 : 차량 집어들기
    /// </summary>
    public class ManaThrowCar_1 : BaseManaState
    {
        private readonly ManaThrowCarSkill parent;
        private string animName = "ManaThrowCar_1";

        private GameObject tmp;
        private GameObject carInstance;

        public ManaThrowCar_1(PlayerController owner, ManaThrowCarSkill parent) : base(owner)
        {
            this.parent = parent;
            tmp = Resources.Load("Unmanaged/Car") as GameObject;
        }

        public override void OnEnter()
        {
            // 손을 드는 애니메이션 실행
            owner.Anim.CrossFade(Animator.StringToHash(animName), 0.01f);
        }

        public override void OnUpdate()
        {
            // 차량 생성을 완료 했다면 다음 스탭
            if(carInstance is not null)
            {
                owner.ManaSkillHandler.NextStep();
            }
        }

        public override void OnAction()
        {
            carInstance = Object.Instantiate(tmp, owner.gameObject.transform.position + Vector3.up * 5f, Quaternion.identity);
        }
    }

    /// <summary>
    /// 2번 동작 : 차량 투척
    /// </summary>
    public class ManaThrowCar_2 : BaseManaState
    {
        private readonly ManaThrowCarSkill parent;
        private string animName = "ManaThrowCar_2";

        private float animTimer = 999f;

        public ManaThrowCar_2(PlayerController owner, ManaThrowCarSkill parent) : base(owner)
        {
            this.parent = parent;
        }

        public override void OnEnter()
        {
            // 차량 투척 애니메이션 실행
            owner.Anim.CrossFade(Animator.StringToHash(animName), 0.01f);
            owner.StartCoroutine(AnimRoutine());
        }
        private IEnumerator AnimRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            animTimer = owner.GetCurrentAnimTime();
        }

        public override void OnUpdate()
        {
            if (animTimer <= 0)
            {
                owner.ManaSkillHandler.NextStep();
            }

            animTimer -= Time.deltaTime;
        }

        public override void OnAction()
        {
            // TODO: 차량이 날아가게 실행
        }

    }
}

