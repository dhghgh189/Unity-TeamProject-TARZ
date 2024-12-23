using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toggle과 연동되어서 사용되는 스크립트,
/// Toggle에서 요청한 내용을 적용한다.
/// </summary>
public class AblityAdapter : MonoBehaviour
{
    // 플레이어의 상태의 따른 충돌 여부를 담은 배열
    public bool[] PlayerCollisionToggle;
    // 던질 수 있는 물건의 프리팹
    public ThrowObject ThrowObjectPrefab;

    public Dictionary<string, IEnable> components;

    private void Awake()
    {
        // 플레이어의 상태의 갯수만큼 초기화
        PlayerCollisionToggle = new bool[(int)EState.Length];
        for (int i = 0; i < PlayerCollisionToggle.Length; i++) { PlayerCollisionToggle[i] = false; }
    }

    private void Start()
    {
        components = new Dictionary<string, IEnable>();

        // ThrowObject에 부착되어있는 활성화 가능한 스크립트 가져오기
        foreach (IEnable enable in ThrowObjectPrefab.GetComponents<IEnable>())
        {
            // 기본적으로 off
            enable.Enable = false;
            components.Add(enable.Name, enable);
        }

    }
    /// <summary>
    /// 상태 머신에서 가져올 충돌체 확인 여부
    /// </summary>
    /// <param name="state">확일한 상태</param>
    /// <returns>충돌 불가능 여부</returns>
    public bool IsPlayerCollision(EState state) => PlayerCollisionToggle[(int)state];

    public void SetEnable(string name)
    {
        if (components.TryGetValue(name, out IEnable enable))
        {
            enable.Enable = true;
        }
    }

    public void SetDisable(string name) 
    {
        if (components.TryGetValue(name, out IEnable enable))
        {
            enable.Enable = false;
        }
    }

    public void SetOnPlayerCollision(EState state, bool on) => PlayerCollisionToggle[(int)state] = on;
    public void SetOffPlayerCollision(EState state, bool on) => PlayerCollisionToggle[(int)state] = !on;


    /// <summary>
    /// ThrowObject에서의 사용가능 한 능력을 활성화 시키는 것
    /// </summary>
    /// <param name="name">컴포넌트의 이름</param>
    /// <returns>동작 여부</returns>
    /// <exception cref="System.Exception"></exception>
    public bool IsEnable(string name) {
        if (components.TryGetValue(name, out IEnable enable))
        {
            return enable.Enable;
        }
        else
        {
            throw new System.Exception($"{name}은 ThrowObject에 부착되어있지 않습니다");
        }
    }

}
