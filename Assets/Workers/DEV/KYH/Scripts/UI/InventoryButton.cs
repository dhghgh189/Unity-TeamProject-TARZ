using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class InventoryButton : MonoBehaviour
{
    [Inject] ChangeInput input;

    [SerializeField] Button inventoryButton;
    [SerializeField] Button bluechipButton;

    [SerializeField] GameObject inventory;
    [SerializeField] GameObject bluechip;

    [SerializeField] Button selectedItem;

    private void Start()
    {
        // 인벤토리 카테고리 선택 버튼 클릭 이벤트 구독
        inventoryButton.onClick.AddListener(OnClickInventoryButton);
        bluechipButton.onClick.AddListener(OnClickBluechipButton);
    }

    /// <summary>
    /// 인벤토리 카테고리 버튼 클릭
    /// </summary>
    public void OnClickInventoryButton()
    {
        // 인벤토리 카테고리 패널 출력
        inventory.SetActive(true);
        bluechip.SetActive(false);
        selectedItem.Select();
    }

    /// <summary>
    /// 스킬 카테고리 버튼 클릭
    /// </summary>
    public void OnClickBluechipButton()
    {
        // 버튼 카테고리 패널 출력
        inventory.SetActive(false);
        bluechip.SetActive(true);
        //selectedSkill.Select();
    }

    /// <summary>
    /// 이벤트 구독 해제
    /// </summary>
    private void OnDestroy()
    {        
        inventoryButton.onClick.RemoveListener(OnClickInventoryButton);
        bluechipButton.onClick.RemoveListener(OnClickBluechipButton);
    }
}
