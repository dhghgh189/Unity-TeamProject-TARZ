using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrainable
{
    void DoDrain(DrainManager owner);
    void StopDrain(DrainManager owner);
}
