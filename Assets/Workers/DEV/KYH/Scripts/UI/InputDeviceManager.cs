using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

public class InputDeviceManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown inputDeviceDropdown;
    [SerializeField] private Image keyboardImage;
    [SerializeField] private Image sideKeyboardImage;
    [SerializeField] private Image gamepadImage;
    [SerializeField] private LocalizedStringTable localTable;

    public enum DeviceType { KeyboardMouse, Gamepad, Both }     // 입력 기기 종류

    private DeviceType currentDevice;                           // 현재 선택된 입력 기기

    private const string SaveDevicePath = "InputDevice.json";

    [System.Serializable]
    private class DeviceConfig
    {
        public int selectedDevice;
    }

    private void Start()
    {
        UpdateDropdownOptions();

        LoadDeviceConfig();
        inputDeviceDropdown.onValueChanged.AddListener(OnDeviceDropdownChange);

        // 현재 연결된 게임패드가 없을 경우 키보드&마우스로 설정
        if (Gamepad.current == null)
        {
            currentDevice = DeviceType.KeyboardMouse;
            keyboardImage.gameObject.SetActive(true);
            sideKeyboardImage.gameObject.SetActive(false);
            gamepadImage.gameObject.SetActive(false);
            inputDeviceDropdown.interactable = false;
        }
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    /// <summary>
    /// 입력 기기 선택 드롭다운 항목 선택 변경값 설정
    /// </summary>
    /// <param name="index"></param>
    private void OnDeviceDropdownChange(int index)
    {
        currentDevice = (DeviceType)index;
        SaveDevice();

        // 현재 입력된 기기에 따른 기기 선택 함수 호출
        switch (currentDevice)
        {
            case DeviceType.KeyboardMouse:
                EnableKeyboardMouse();
                break;
            case DeviceType.Gamepad:
                EnableGamepad();
                break;
            case DeviceType.Both:
                EnableBoth();
                break;
        }
    }

    /// <summary>
    /// 드롭다운 선택지(옵션) 업데이트
    /// </summary>
    private void UpdateDropdownOptions()
    {
        inputDeviceDropdown.ClearOptions();

        localTable.GetTableAsync().Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                var stringTable = handle.Result;

                var options = new System.Collections.Generic.List<string>
                {
                    stringTable["InputKeyboard_Key"].LocalizedValue,
                    stringTable["InputGamepad_Key"].LocalizedValue,
                    stringTable["InputBoth_Key"].LocalizedValue
                };

                inputDeviceDropdown.AddOptions(options);
            }
        };
    }

    /// <summary>
    /// 키보드&마우스 사용
    /// </summary>
    private void EnableKeyboardMouse()
    {
        // 연결된 게임패드가 있을 경우 사용하지 않음 처리
        if (Gamepad.current != null)
        {
            InputSystem.DisableDevice(Gamepad.current);
        }
        
        // 키보드&마우스 사용 처리
        InputSystem.EnableDevice(Keyboard.current);
        InputSystem.EnableDevice(Mouse.current);
        keyboardImage.gameObject.SetActive(true);
        gamepadImage.gameObject.SetActive(false);
        sideKeyboardImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 게임패드 사용
    /// </summary>
    private void EnableGamepad()
    {
        // 연결된 게임패드가 있을 경우 사용 처리
        if (Gamepad.current != null)
        {
            InputSystem.EnableDevice(Gamepad.current);
        }

        // 키보드&마우스 사용하지 않음 처리
        InputSystem.DisableDevice(Keyboard.current);
        InputSystem.DisableDevice(Mouse.current);
        keyboardImage.gameObject.SetActive(false);
        gamepadImage.gameObject.SetActive(true);
        sideKeyboardImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 모두 사용
    /// </summary>
    private void EnableBoth()
    {
        // 연결된 게임패드가 있을 경우 사용 처리
        if (Gamepad.current != null)
        {
            InputSystem.EnableDevice(Gamepad.current);
        }

        // 키보드&마우스 사용 처리
        InputSystem.EnableDevice(Keyboard.current);
        InputSystem.EnableDevice(Mouse.current);
        keyboardImage.gameObject.SetActive(false);
        gamepadImage.gameObject.SetActive(true);
        sideKeyboardImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// 입력기기 선택값 저장
    /// </summary>
    private void SaveDevice()
    {
        DeviceConfig config = new DeviceConfig
        {
            selectedDevice = (int)currentDevice
        };

        string json = JsonUtility.ToJson(config, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveDevicePath), json);
    }

    /// <summary>
    /// 입력기기 선택값 로드
    /// </summary>
    private void LoadDeviceConfig()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveDevicePath);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DeviceConfig config = JsonUtility.FromJson<DeviceConfig>(json);

            currentDevice = (DeviceType)config.selectedDevice;
            inputDeviceDropdown.value = config.selectedDevice;

            OnDeviceDropdownChange(config.selectedDevice);
        }
        else
        {
            currentDevice = DeviceType.KeyboardMouse;
            inputDeviceDropdown.value = 0;
        }
    }

    /// <summary>
    /// 입력기기 변경 이벤트용 함수
    /// </summary>
    /// <param name="device"></param>
    /// <param name="change"></param>
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                inputDeviceDropdown.interactable = true;
                break;
            case InputDeviceChange.Removed:
                inputDeviceDropdown.interactable = false;
                break;
        }
    }

    /// <summary>
    /// 값 변경 이벤트 구독 해제
    /// </summary>
    private void OnDestroy()
    {
        inputDeviceDropdown.onValueChanged.RemoveListener(OnDeviceDropdownChange);
    }
}
