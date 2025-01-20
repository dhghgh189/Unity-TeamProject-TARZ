using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class Inventory : MonoBehaviour
{
    [Inject] InGameSaveData saveData;

    [Inject] PlayerController playerController;

    // 인벤토리 슬롯들을 보관할 배열 12개임
    [Inject] UI_InventorySlots[] inventorySlots;

    // 인벤토리 캔버스
    [SerializeField] GameObject canvas;

    // 장비들의 기본 능력치로 지정된 베이스 장비가 담길 배열
    [SerializeField] Gear[] baseGears = new Gear[(int)Part.Size];

    // 선택된 장비의 선택지들
    [SerializeField] GameObject selectPanel;
    private Button[] SelectButtons;

    // 선택된 버튼 위치를 기억
    private Button selectedButton;

    private InputAction inventoryAction;

    [SerializeField] Sprite[] gearSprite;

    [HideInInspector] public UI_GearChange GearChange;

    private void Start()
    {
        GearChange = GetComponent<UI_GearChange>();
        SelectButtons = selectPanel.GetComponentsInChildren<Button>();
        foreach (var item in saveData.InventoryGears)
        {
            Gear saveGear = ScriptableObject.CreateInstance<Gear>();
            saveGear.Part = item.Part;
            saveGear.Tier = item.Tier;
            saveGear.GearName = item.GearName;
            saveGear.Abilities = item.Abilities;
            EmptySlot().SetInventorySlots(saveGear);
        }

        inventoryAction = InputSystem.actions.FindAction("Inventory");
    }

    // 티어와 부위를 지정해 장비를 인벤토리에 저장하는 함수
    public bool GetGear(Part part, int tier, float pValue)
    {
        UI_InventorySlots slot = EmptySlot();
        if (!slot) return false;

        // 해당 부위의 베이스 장비를 가져옴
        Gear gear = Instantiate(baseGears.Where(x => x.Part == part).First());

        // 티어 부여
        tier = Mathf.Clamp(tier, 1, 3);
        gear.Tier = tier;

        // 장갑은 4개중 하나의 기본 능력치를 가지므로 능력치 3개를 삭제
        if (part == Part.장갑)
        {
            for (int i = 0; i < 3; i++)
            {
                gear.Abilities.RemoveAt(Random.Range(0, gear.Abilities.Count));
            }
        }

        // 랜덤한 능력치를 랜덤 확률로 상승
        if (Util.IsRandom(pValue))
            gear.Abilities.Add(new GearAbility() { ability = (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size), value = 10 });
        if (Util.IsRandom(pValue))
            gear.Abilities.Add(new GearAbility() { ability = (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size), value = 10 });

        // 이름 변경
        gear.SetName();

        // 베이스 능력치에 티어를 곱하기
        foreach (var item in gear.Abilities)
        {
            item.value *= tier;
        }

        slot.SetInventorySlots(gear);
        SoundManager.PlaySFX(SoundManager.SoundData_UI.GetEquipment);
        return true;
    }

    public bool GetGear(Gear gear)
    {
        UI_InventorySlots slot = EmptySlot();
        if (!slot) return false;

        // 이름 변경
        gear.SetName();

        // 베이스 능력치에 티어를 곱하기
        foreach (var item in gear.Abilities)
        {
            item.value *= gear.Tier;
        }

        slot.SetInventorySlots(gear);
        return true;
    }

    // 빈 인벤토리 슬롯을 반환하는 함수
    private UI_InventorySlots EmptySlot()
    {
        foreach (var item in inventorySlots)
        {
            if (item.IsEmpty) return item;
        }
        return null;
    }

    public void InventorySave()
    {
        saveData.InventoryGears.Clear();
        foreach (var item in inventorySlots)
        {
            GearSaveData gearSaveData = item.SaveInventoyGear();
            if (gearSaveData == null) continue;
            saveData.InventoryGears.Add(gearSaveData);
        }
    }
    public void SelectSlot(UI_InventorySlots slots)
    {
        if (slots.IsEmpty) return;
        // 선택한 슬롯 위치를 저장
        selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        // 선택지를 활성화
        selectPanel.SetActive(true);

        // 이벤트 삭제
        foreach (var item in SelectButtons)
            item.onClick.RemoveAllListeners();

        // 이벤트 지정
        SelectButtons[0].onClick.AddListener(() => { slots.GearSell(); SelectButtonReset(); });
        SelectButtons[1].onClick.AddListener(() => { slots.EquipGear(); SelectButtonReset(); });

        SelectButtons[0].Select();
    }

    // 교체 혹은 분해 후 SelectPanel 리셋 함수
    private void SelectButtonReset()
    {
        selectPanel.SetActive(false);
        // selectPanel닫고 selectedButton이 있다면 해당 버튼을 선택 아니면 첫 번째 버튼을 선택
        if (selectedButton)
            selectedButton.Select();
        else
            GetComponentInChildren<Button>()?.Select();
        selectedButton = null;
    }

    private void Update()
    {
        if (inventoryAction.WasPressedThisFrame())
        {
            if (canvas.activeSelf)
            {
                canvas.SetActive(false);
                selectPanel.SetActive(false);
                playerController.PInput.IsCanControl = true;
                return;
            }
            playerController.PInput.IsCanControl = false;
            canvas.SetActive(true);
            GetComponentInChildren<Button>(true).Select();
        }
    }

    public void Clear()
    {
        foreach (UI_InventorySlots slot in inventorySlots)
        {
            slot.ClearSlot();
        }
    }

    public Sprite GetSprite(int index)
    {
        return index > gearSprite.Length - 1 ? null : gearSprite[index];
    }

    public Gear StoreGear()
    {
        float random = Random.Range(1, 101);
        int stage = saveData.chapterSaveData.StageNum + 1;

        Part part = (Part)Random.Range(0, (int)Part.Size);
        Gear gear = Instantiate(baseGears.Where(x => x.Part == part).First());
        gear.Tier = random > 100 - (10 * (stage == 3 ? 5 : stage)) ? 3 : 2;

        // 장갑은 4개중 하나의 기본 능력치를 가지므로 능력치 3개를 삭제
        if (part == Part.장갑)
        {
            for (int i = 0; i < 3; i++)
            {
                gear.Abilities.RemoveAt(Random.Range(0, gear.Abilities.Count));
            }
        }

        // 랜덤한 능력치를 랜덤 확률로 상승
        if (Util.IsRandom(50))
            gear.Abilities.Add(new GearAbility() { ability = (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size), value = 10 });
        if (Util.IsRandom(50))
            gear.Abilities.Add(new GearAbility() { ability = (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size), value = 10 });

        // 이름 변경
        gear.SetName();

        // 베이스 능력치에 티어를 곱하기
        foreach (var item in gear.Abilities)
        {
            item.value *= gear.Tier;
        }
        return gear;
    }
}
