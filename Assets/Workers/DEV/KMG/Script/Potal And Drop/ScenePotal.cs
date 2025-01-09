using UnityEngine;

public class ScenePotal : MonoBehaviour
{
    [SerializeField] Define.SceneType sceneType;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        FindAnyObjectByType<SaveManager>().Save();
        Util.ChangeScene(sceneType);
    }
    public void SetScene(Define.SceneType sceneType)
    {
        this.sceneType = sceneType;
    }
}
