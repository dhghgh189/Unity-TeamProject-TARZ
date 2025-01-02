using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStart : MonoBehaviour, Base_InteractionOBJ
{
    // 로딩 상태를 보여주는 패널 오브젝트 클래스
    [Inject] private Loading loadingObject;

    /// <summary>
    /// 게임 씬으로 전환
    /// </summary>
    public void Activate()
    {
        loadingObject.StartLoading(Define.SceneType.Chapter1);
    }
}
