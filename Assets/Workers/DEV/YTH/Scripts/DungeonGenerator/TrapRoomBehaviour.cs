using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrapRoomBehaviour : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] BagSkillManager bagSkillManager;
    [SerializeField] List<ISwitchable> traps = new();

    private void Awake()
    {
        bagSkillManager = FindAnyObjectByType<BagSkillManager>();

        foreach (ISwitchable child in GetComponentsInChildren<ISwitchable>())
        {
            traps.Add(child);
        }
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
    }

    public void Clear()
    {
        foreach (ISwitchable item in traps)
        {
            if (item.IsActive)
            {
                item.Deactivate();
            }
        }
        OpenWall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        CloseWall();
        transform.GetComponent<BoxCollider>().enabled = false;
    }
}