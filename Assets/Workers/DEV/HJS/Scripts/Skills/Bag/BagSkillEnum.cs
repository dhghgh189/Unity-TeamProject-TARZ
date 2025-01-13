using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagSkillEnum
{
    public enum BagIndexKey
    { 
        JunkFist, ScrapBurst, CompactCanon
    }

    public enum JunkFistDataType { DefaultDamage, IncreaseDamage, Angle, Range, OperationTime }

    public enum ScrapBurstDataType { DefaultDamage, Duration, Distance, WaitDelay }

    public enum CompactCanonDataType { DefaultDamage, Force, ChargeTime}
}
