using System;
using UnityEngine;

/// <summary>
/// 보스는 Instantiate로 생성해서 따로 관리 고려중..
/// </summary>
public class PooledObject : MonoBehaviour, IKnockBack
{
    private ObjectPool _returnPool; //반납 위치
    public ObjectPool ReturnPool { get { return _returnPool; } set { _returnPool = value; } }

    [SerializeField] GameObject _gear;

    [SerializeField] GameObject _chip;

    private MonsterData _monsterData;

    public event Action OnDie;

    private AutoLockOn _autoLockOn;

    [SerializeField] GameObject _player;

    private void Start()
    {
        _autoLockOn = _player.GetComponent<AutoLockOn>();
        _monsterData = GetComponent<MonsterData>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
        }
    }
    private void OnEnable()
    {
        OnDie += Die;
    }

    private void OnDisable()
    {
        OnDie -= Die;
    }

    public void TakeDamage(float damage)
    {
        _monsterData.CurHp -= damage;
        _monsterData.Attacked_First = true;
        // 맞는 애니메이션 재생
        // 넉백 적용

        if (_monsterData.CurHp <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void Die()
    {
        _autoLockOn.action?.Invoke();
        ReturnPool.ReturnPool(this);
        //죽는애니메이션 재생
        GameObject gear = Instantiate(_gear, transform.position, transform.rotation);
        gear.GetComponent<DropGear>().SetDropItem(Part.신발, 1, true, true);
    }

    public void KnockBack(GameObject attacker)
    {
       
    }
}
