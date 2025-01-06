using System;
using UnityEngine;
using Zenject;

public class ChapterManager : MonoBehaviour
{
    public StageInfo[] stageInfos;
    [Inject] public InGameSaveData saveData;
}

[Serializable]
public class StageInfo
{
    public MonsterName[] BossName;
    public int RoomCount;
}
