using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour, Base_InteractionOBJ
{
    public void Activate()
    {
        Util.ChangeScene(Define.SceneType.Game);
    }
}
