using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour, Base_InteractionOBJ
{
    [SerializeField] private Loading loadingObject;
    public void Activate()
    {
        //Util.ChangeScene(Define.SceneType.Game);
        loadingObject.StartLoading(Define.SceneType.Game);
    }
}
