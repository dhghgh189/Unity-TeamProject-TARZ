using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public int MonsterCount;

    [SerializeField] GameObject[] walls;
    [SerializeField] GameObject[] buffPrefab;   // 버프 프리팹

    [SerializeField] Buff speedBuff;

    private BagSkillManager bagSkillManager;

    private void Awake()
    {
        bagSkillManager = FindAnyObjectByType<BagSkillManager>();
    }

    public void CloseWall()
    {
        walls = walls.Where(x => !x.activeSelf).ToArray();
        foreach (var item in walls)
        {
            item.SetActive(true);
            item.GetComponent<PhaseController>().StartPhase(false);
        }
    }
    public void OpenWall()
    {
        foreach (var item in walls)
        {
            item.GetComponent<PhaseController>().StartPhase(true, item);
        }
        Instantiate(buffPrefab[Random.Range(0, buffPrefab.Length)], transform.position + Vector3.forward * 2, Quaternion.identity);
        Instantiate(speedBuff, transform.position, Quaternion.identity);
    }

    public void MonsterCountChange()
    {
        MonsterCount--;
        if (MonsterCount == 0)
        {
            OpenWall();
            bagSkillManager.OnChargeEvent?.Invoke();
            bagSkillManager.UpdateCharge();
            bagSkillManager.OnUIUpdateEvent?.Invoke();
        }
    }
}