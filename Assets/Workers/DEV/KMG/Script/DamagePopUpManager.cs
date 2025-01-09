using UnityEngine;

public class DamagePopUpManager : MonoBehaviour
{
    [SerializeField] GameObject popUpPrefab;

    public void SetDamagePopUp(Vector3 pos, string text, Color color)
    {
        foreach (DamagePopUp item in GetComponentsInChildren<DamagePopUp>(true))
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
                item.PopUpInit(pos, text, color);
                return;
            }
        }
        Instantiate(popUpPrefab, pos, Quaternion.identity, transform).GetComponent<DamagePopUp>().PopUpInit(pos, text, color);
    }
}
