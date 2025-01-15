using UnityEngine;

public class StoreSound : MonoBehaviour
{
    AudioClip tempClip;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")|| SoundManager.BGM.clip == SoundManager.SoundData_UI.StoreBGM)
            return;
        tempClip = SoundManager.BGM.clip;
        SoundManager.PlayBGM(SoundManager.SoundData_UI.StoreBGM);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        SoundManager.PlayBGM(tempClip);
    }
}
