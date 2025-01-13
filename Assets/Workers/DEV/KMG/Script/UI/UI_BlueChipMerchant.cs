using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_BlueChipMerchant : MonoBehaviour
{
    [Inject] StatModel skillModel;
    [Inject] PlayerSkillHandler skillHandler;
    [Inject] InGameSaveData saveData;

    [SerializeField] SkillSpecDatabase skillSpecDatabase;

    [SerializeField] Button buyButton;
    [SerializeField] TMP_Text sellSkillText;

    private float price;

    private BaseSkillSO sellSkill;

    private bool isAdditionalSell;

    private void Awake()
    {
        buyButton.onClick.AddListener(BuySkill);
        sellSkillSet();
    }


    private void BuySkill()
    {
        if (skillModel.BlackChip < price)
            return;

        skillHandler.AddSkill(sellSkill.Name);
        if (saveData.chapterSaveData.StageNum > 0 && Util.IsRandom(50f) && !isAdditionalSell)
        {
            isAdditionalSell = true;
            sellSkillSet();
            return;
        }
        sellSkillText.text = "매진이야";
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

        sellSkillText.text = $"{sellSkill.Name}을 {price}에 살꺼야?";
    }
}
