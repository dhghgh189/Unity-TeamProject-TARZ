using System.Linq;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    public int MonsterCount;

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

    public void MonsterCountChange()
    {
        MonsterCount--;
        if (MonsterCount == 0)
        {
            OpenWall();
        }
    }
}