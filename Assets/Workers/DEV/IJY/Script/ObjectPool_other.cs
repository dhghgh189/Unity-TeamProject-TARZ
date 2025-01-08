using UnityEngine;
using Zenject;

public class ObjectPool_other : MonoBehaviour
{
    [Inject] [SerializeField] Transform dropPool;
    [SerializeField] private GameObject DropChip;

    public void DropChipItem(float random, Vector3 curPos)
    {
        foreach (var item in dropPool.GetComponentsInChildren<DropChip>(true))
        {
            if (!item.gameObject.activeSelf)
            {
                item.SetDropChip(random, true);
                item.transform.position = curPos;
                item.gameObject.SetActive(true);
                return;
            }
        }
        DropChip curDropChip = Instantiate(DropChip, curPos, transform.rotation, dropPool.transform).GetComponent<DropChip>();
        curDropChip.SetDropChip(random, true);
    }
}
