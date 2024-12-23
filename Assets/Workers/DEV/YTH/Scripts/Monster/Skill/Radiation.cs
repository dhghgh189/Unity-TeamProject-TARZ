using System.Collections;
using UnityEngine;

public class Radiation : MonoBehaviour
{
    [SerializeField] float _interval;
    public float Interaval {  get { return _interval; } set { _interval = value; } }

    [SerializeField] int _damage;
    public int Damage { get { return _damage; } set { _damage = value; } }

    IDamagable _damagable;

    private void OnTriggerStay(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        _damagable = damagable;
        if (damagable != null)
        {
            if (takeDOTRoutine == null)
            {
                takeDOTRoutine = StartCoroutine(TakeDOTRoutine(_damage));
            }
        }
    }

  
    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(takeDOTRoutine);
    }

    Coroutine takeDOTRoutine;
    public IEnumerator TakeDOTRoutine(int damage)
    {
            _damagable.TakeDamage(damage);
            yield return new WaitForSeconds(_interval);  // 기획분들이 정해주시면 딜레이 캐싱 해두기
        takeDOTRoutine = null;  
    }
}
