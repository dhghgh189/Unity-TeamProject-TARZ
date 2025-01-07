using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnim : MonoBehaviour
{
    private int Hash_Move = Animator.StringToHash("Revive_Walk");
    private int Hash_Crawl = Animator.StringToHash("Revive_Crawl");
    private int Hash_Idle = Animator.StringToHash("Revive_Idle");


    private Animator _animator;
    private MonsterData _monsterData;
    private Rigidbody _rigid;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _monsterData = GetComponent<MonsterData>();
        _rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_monsterData.IsMoving)
        {
            if (_monsterData.MonsterTyPe == MonsterData.MonsterType.Revive && _monsterData.CurHp <= _monsterData.MaxHp * 0.5f)
            {
                _animator.CrossFade(Hash_Crawl, 0.05f);
            }
            else
            {
                _animator.CrossFade(Hash_Move, 0.05f);
            }
        }
        else
        {
            _animator.CrossFade(Hash_Idle, 0.05f);
        }

      /*  if (_monsterData.MonsterTyPe == MonsterData.MonsterType.Revive && _monsterData.CurHp <= _monsterData.MaxHp * 0.5f)
        {
            _animator.CrossFade(Hash_Crawl, 0.05f);
        }*/
    }

}
