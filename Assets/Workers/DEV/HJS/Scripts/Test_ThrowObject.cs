using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ThrowObject : MonoBehaviour
{
    [SerializeField] public PlayerSkillHandler handler;

    // 유도와 같은 적을 우선 선별 해야할 때
    [SerializeField] Transform target;

    private void Start()
    {
        handler.Use(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 적일 때 대미지 입히기
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            // 데미지 입히기
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
        {
            GetComponent<GuidedFuncion>().enabled = false;
        }
    }

    private void OnDestroy()
    {
        // 파괴가 되었을 때 ~한다느게 좋을 거 같다
        handler.ThrowObjectCollision(gameObject);
    }
}
