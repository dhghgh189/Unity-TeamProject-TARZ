using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStart : MonoBehaviour, Base_InteractionOBJ
{
    [Inject] private Loading loadingObject;
    public void Activate()
    {
        loadingObject.StartLoading(Define.SceneType.Chapter1);
    }
}
