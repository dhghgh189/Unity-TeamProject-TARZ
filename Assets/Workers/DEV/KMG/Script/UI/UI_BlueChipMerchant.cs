using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_BlueChipMerchant : MonoBehaviour
{
    [Inject] PlayerController playerController;
    [Inject] StatModel skillModel;
    [Inject] PlayerSkillHandler skillHandler;
    [Inject] InGameSaveData saveData;

    [SerializeField] SkillSpecDatabase skillSpecDatabase;

    [SerializeField] Image skillIcon;

    [SerializeField] Button buyButton;
    [SerializeField] Button closeButton;

    [SerializeField] TMP_Text sellSkillName;
    [SerializeField] TMP_Text sellSkillInfo;
    [SerializeField] TMP_Text chipText;

    private float price;

    private BaseSkillSO sellSkill;

    private bool isAdditionalSell;

    private void Awake()
    {
        buyButton.onClick.AddListener(BuySkill);
        closeButton.onClick.AddListener(ClosePanel);
        sellSkillSet();
    }


    private void BuySkill()
    {
        if (skillModel.BlackChip < price)
            return;

        SoundManager.PlaySFX(SoundManager.SoundData_UI.Buy);

        skillHandler.AddSkill(sellSkill.Name);
        if (saveData.chapterSaveData.StageNum > 0 && Util.IsRandom(50f) && !isAdditionalSell)
        {
            isAdditionalSell = true;
            sellSkillSet();
            return;
        }
        chipText.text = "매진";
        buyButton.gameObject.SetActive(false);
    }

    private void sellSkillSet()
    {
        int tier = 0;
        switch (saveData.chapterSaveData.StageNum)
        {
            case 0:
                tier = Util.IsRandom(1) ? 1 : Util.IsRandom(33) ? 2 : 3; 
                break;
            case 1:
                tier = Util.IsRandom(10) ? 1 : Util.IsRandom(30) ? 2 : 3;
                break;
            case 2:
                tier = Util.IsRandom(20) ? 1 : Util.IsRandom(40) ? 2 : 3;
                break;
        }

        sellSkill = skillSpecDatabase.GetSkill(tier);
        price = (4 - sellSkill.SkillTier) * 70f;

        sellSkillName.text = $"{sellSkill.Name}";
        sellSkillInfo.text = $"{sellSkill.Description}";
        chipText.text = $"{price} 블랙 데이터 칩";
        skillIcon.sprite = sellSkill.Icon;
    }

    private void ClosePanel()
    {
        GetComponentInParent<UI_Merchant>().Temp(UI_Merchant.EMerchant.Chip);
    }
    private void OnEnable()
    {
        buyButton.Select();
    }
}
