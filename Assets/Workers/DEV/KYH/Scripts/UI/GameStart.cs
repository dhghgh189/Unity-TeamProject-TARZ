using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStart : MonoBehaviour, Base_InteractionOBJ
{
    [Inject] private Loading loadingObject;
    public void Activate()
    {
        //Util.ChangeScene(Define.SceneType.Game);
        loadingObject.StartLoading(Define.SceneType.Game);
    }
}
