using UnityEngine;

public class SpecialInteraction_Script : MonoBehaviour, Base_InteractionOBJ_Grab, IThrowChest
{
    public PlayerController playerController { get; set; }
    public Collider col;
    public bool isThrowing = false;
    public Child_SpecialTrigger trigger;


    void Awake() => Init();

    void Init()
    {
        trigger = new GameObject("UI_trigger").AddComponent<Child_SpecialTrigger>();
        trigger.transform.position = this.transform.position;
        trigger.transform.parent = this.transform;

        col = GetComponent<Collider>();
    }

    public void Activate_Grab()
    {
        if (playerController == null) return;
        if (playerController.IsGrabingInput == false) return;

        if (playerController.IsGrabingInput == true)
        {
            Debug.Log($"{gameObject.name} : 활성화됨");
            ThrowingReady(playerController.interactioner.GrabPos);
        }
    }

    void ThrowingReady(Transform curPos)
    {
        trigger = null;
        this.transform.DetachChildren();
        this.gameObject.layer = 0;

        col.enabled = false;
        this.gameObject.transform.parent = playerController.interactioner.gameObject.transform;
        this.transform.position = curPos.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrowing)
        {
            // TODO : 오브젝트 폭발이나 데미지 입히는 기능 구현
            // 임시적 데미지 구성. 추후 해당 스크립트를 인터페이스로 상속하여, 함수를 골라 호출할 수 있도록 할 수 있게 할 예정
            // 타입별로 한 함수에 구현하여 사용하는 방법?
            // 혹은 스크립드를 각자 따로 두어서 사용하는 방법? 어느게 좋을까
            ThrowingChest(collision.gameObject, (int)ChestType.Nomal, 10f);
        }
        else return;
    }

    public void ThrowingChest(GameObject OBJ, int type, float typeDamage)
    {
        // 추후 종류별로 필드에 선언해 사용 예정
        LayerMask layer = LayerMask.NameToLayer("Monster");

        if (OBJ.layer == layer)
        {
            OBJ.GetComponent<IDamagable>().TakeDamage(typeDamage);
        }

        Destroy(this.gameObject, 1.5f);
    }
}

// 인터페이스를 통해 구성할 예정의 함수
// SpecialInteraction_Script를 상속 클래스를 둔 스크립트에서 인터페이를 상속받아 사용해주는 형식을 차용할 예정
// 구성요소 : 인식된 오브젝트, 상자의 속성

// 임시적으로 구성한 체스트
public enum ChestType { Nomal, Bomb, Size }

public interface IThrowChest
{
    public void ThrowingChest(GameObject OBJ, int type, float typeDamage);
}

