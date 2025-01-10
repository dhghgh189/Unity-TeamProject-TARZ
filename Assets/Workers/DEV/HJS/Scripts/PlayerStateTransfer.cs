using System.Linq;
using UnityEngine;

/// <summary>
/// 플레이어 상태 전이 가능 여부
/// </summary>
public class PlayerStateTransfer : MonoBehaviour
{
    [SerializeField] bool[] canTransStates;
    public bool[] CanTransState => canTransStates;

    private void Awake()
    {
        canTransStates = new bool[(int)EState.Length];
        for(int i = 0; i < (int)EState.Length; i++) canTransStates[i] = true;
    }

    public void OnEnableState(EState[] changeArr)
    {
        foreach (EState item in changeArr)
        {
            canTransStates[(int)item] = true;
        }
    }

    public void OnDisableState(EState[] changeArr)
    {
        foreach (EState item in changeArr)
        {
            canTransStates[(int)item] = false;
        }
    }
}
