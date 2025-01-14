using System.Linq;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public int MonsterCount;

    [SerializeField] GameObject[] walls;
    [SerializeField] GameObject[] buffPrefab;   // 버프 프리팹

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
        }
    }
    public void OpenWall()
    {
        foreach (var item in walls)
        {
            item.SetActive(false);
        }
        Instantiate(buffPrefab[Random.Range(0, buffPrefab.Length)], transform.position + (Vector3.up * 4), Quaternion.identity);
    }

    public void MonsterCountChange()
    {
        MonsterCount--;
        if (MonsterCount == 0)
        {
            OpenWall();
            bagSkillManager.OnChargeEvent?.Invoke();
            bagSkillManager.OnUIUpdateEvent?.Invoke();
        }
    }
}