using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActMoveToLastPlayerTransform : MonoBehaviour
{
    [SerializeField] CondCanMove _condCanMove;

    private Transform _lastPlayerTransform; // 플레이어가 시야각에서 사라진 마지막 위치

    [SerializeField] GameObject _player;

    void Start()
    {
        StartCoroutine(GetLasPlayerTransformRoutine());
    }

   
    void Update()
    {
        
    }

    /// <summary>
    /// 플레이어가 시야에서 사라졌을때 마지막 플레이어 위치 기억
    /// </summary>
    Coroutine getLasPlayerTransformRoutine;
    IEnumerator GetLasPlayerTransformRoutine()
    {
        if (_condCanMove.IsPlayerWithinSight(_player) == false)
        {
            _lastPlayerTransform = _player.transform;
        }
        yield return null;
        getLasPlayerTransformRoutine = null;
    }
}
