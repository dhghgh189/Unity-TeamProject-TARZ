using UnityEngine;

public class StoreSound : MonoBehaviour
{
    AudioClip tempClip;
    private void OnTriggerEnter(Collider other)
    {
        if (SoundManager.BGM.clip == SoundManager.SoundData_UI.StoreBGM)
            return;
        tempClip = SoundManager.BGM.clip;
        SoundManager.PlayBGM(SoundManager.SoundData_UI.StoreBGM);
    }
    private void OnTriggerExit(Collider other)
    {
        SoundManager.PlayBGM(tempClip);
    }
}
