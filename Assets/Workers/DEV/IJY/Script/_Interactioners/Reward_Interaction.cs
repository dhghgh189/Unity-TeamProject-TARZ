using UnityEngine;

public class Reward_Interaction : InteractionOBJ_Base, Interaction_Ibase_Activate
{
    private ObjectPool_other pool;

    [Header("보상 드랍 상자")]
    [SerializeField] private int Reward_DropCount;
    [SerializeField] private float Reward_DropSpred;

    void Start() => Init();

    void Init()
    {
        pool = FindObjectOfType<ObjectPool_other>();
    }

    public void Activate()
    {
        DropBlackChips(3f);
    }

    void DropBlackChips(float DestroyTime)
    {
        Vector3 position = transform.position;

        while (Reward_DropCount >= 1)
        {
            Reward_DropCount--;
            position.x += Reward_DropSpred * Random.value - Reward_DropSpred / 2;
            position.z += Reward_DropSpred * Random.value - Reward_DropSpred / 2;
            if (position.y <= 0f) position.y = 0.1f;

            pool.DropChipItem(Random.Range(1, 50), position);
        }

        Destroy(this.gameObject, DestroyTime);
    }
}
