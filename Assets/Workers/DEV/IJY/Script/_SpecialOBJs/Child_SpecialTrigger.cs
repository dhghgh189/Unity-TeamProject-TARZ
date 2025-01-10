using UnityEngine;
using Zenject;

public class Child_SpecialTrigger : MonoBehaviour
{
    private PlayerView player;

    private SpecialThrowOBJ_Base SpecialOBJ;
    private SphereCollider col;
    private LayerMask playerLayer;

    private bool IsPlayerIn;

    // 해당 bool형을 통해 UI 온오프를 판단
    // 해당 bool형을 통해 한 번 UI가 on 상태가 되면, 해당 위치에 UI를 띄운 후 나머지에는 UI를 띄우지 않도록 함수 최상단에서 activeSelf를 통해 return 하도록 한다.
    public bool isPlayerIn { get { return IsPlayerIn;} set { IsPlayerIn = value; } }


    void Start() => Init();

    void Init()
    {
        player = FindAnyObjectByType<PlayerView>();

        IsPlayerIn = false;

        SpecialOBJ = GetComponentInParent<SpecialThrowOBJ_Base>();
        playerLayer = LayerMask.NameToLayer("Player");

        col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = 3.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayerIn == true) return;

        if (other.gameObject.layer == playerLayer)
        {
            Debug.Log(IsPlayerIn);
            IsPlayerIn = true;
            player.SetActiveUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayerIn == false) return;

        if (other.gameObject.layer == playerLayer)
        {
            Debug.Log(IsPlayerIn);
            IsPlayerIn = false;
            player.HideUI();
        }
    }

    private void OnDestroy()
    {
        player.HideUI();
    }
}
