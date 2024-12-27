using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class DistanceChecker : Action
{
    [SerializeField] GameObject _player;

    private float _distance;

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
        return TaskStatus.Running;
    }
}