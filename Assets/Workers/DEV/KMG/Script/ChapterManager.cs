using System;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public StageInfo[] stageInfos;
}

[Serializable]
public class StageInfo
{
    public MonsterName BossName;
    public int RoomCount;
}
