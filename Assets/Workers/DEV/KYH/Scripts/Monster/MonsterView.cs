using UnityEngine;

public class MonsterView : MonoBehaviour
{
    [SerializeField] private GameObject hpGauge;

    public void AddGauge(MonsterData data)
    {
        GameObject instance = Instantiate(hpGauge, gameObject.transform);
        BossHPGauge fill = instance.GetComponent<BossHPGauge>();
        fill.SetInfo(data);
    }
}
