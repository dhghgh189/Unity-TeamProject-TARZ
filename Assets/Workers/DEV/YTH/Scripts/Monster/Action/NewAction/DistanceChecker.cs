using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class DistanceChecker :MonoBehaviour
{
    [SerializeField] GameObject _player;
    public GameObject Player { get { return _player; } private set { } }

    [SerializeField] float _distance;
    public float Distance { get { return _distance; } private set { } }

    public void Update()
    {
        Distance = Vector3.Distance(transform.position, Player.transform.position);
    }
  
}