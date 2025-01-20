using UnityEngine;
using Zenject;

public class BossRoomBehaviour : MonoBehaviour
{
    [SerializeField] GameObject movePotal;

    [SerializeField] GameObject scenePotal;

    [SerializeField] GameObject _redChip;
    
    public int BossCount;

    private ChapterManager chapterManager;
    private InGameSaveData saveData;
    private MonsterView _monsterView;

    [Inject] Transform dropPool;

    private void Start()
    {
        chapterManager = FindAnyObjectByType<ChapterManager>();
        _monsterView = FindAnyObjectByType<MonsterView>(FindObjectsInactive.Include);
        saveData = chapterManager.saveData;
    }

    public void BossClear()
    {
        // 스테이지 ++, 만약에 3스테이지면 로비로, 맵으로 가는 포탈 하나랑 다음 스테이지로 가는 포탈 하나
        if (++saveData.chapterSaveData.StageNum == 3)
        {
            // 클리어 사운드 재생
            SoundManager.PlaySFX(SoundManager.SoundData_UI.GameClear);
            saveData.chapterSaveData = new();
            scenePotal.GetComponent<ScenePotal>().SetScene(Define.SceneType.Lobby);
        }
        else
        {
            scenePotal.GetComponent<ScenePotal>().SetScene(Define.SceneType.Chapter1);
        }

        Instantiate(scenePotal, transform.position + Vector3.back * 10 + Vector3.up * 2, Quaternion.identity);

        Instantiate(movePotal, transform.position + Vector3.forward * 10 + Vector3.up * 2, Quaternion.identity).GetComponent<MovePotal>().SetTarget((Vector3.forward + Vector3.right) * 7);

        Instantiate(_redChip, transform.position + Vector3.up * 2, transform.rotation, dropPool).GetComponent<DropRedChip>().DropChipInit();

        if (_monsterView != null)
        {
            _monsterView.gameObject.SetActive(false);
        }
    }

    public void BossCountChange()
    {
        BossCount--;
        if (BossCount == 0)
        {
            BossClear();
        }
    }
}
