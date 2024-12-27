using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaFistGateObject : MonoBehaviour
{
    [SerializeField] MegaFistObject fist;

    private void Start()
    {
    }

    private IEnumerator tesRoutine()
    {
        yield return new WaitForSeconds(1);
        fist.MoveToward();
    }

    public void Init(ManaSkillDataSO data)
    {
        
    }
}
