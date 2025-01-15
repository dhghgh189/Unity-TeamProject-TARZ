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

    public enum DeviceType { KeyboardMouse, Gamepad, Both }

    private DeviceType currentDevice;

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

        if (Gamepad.current == null)
        {
            currentDevice = DeviceType.KeyboardMouse;
            keyboardImage.gameObject.SetActive(true);
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

    private void Update()
    {
        
    }

    private void OnDeviceDropdownChange(int index)
    {
        currentDevice = (DeviceType)index;
        SaveDevice();

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

    private void EnableKeyboardMouse()
    {
        if (Gamepad.current != null)
        {
            InputSystem.DisableDevice(Gamepad.current);
        }
        
        InputSystem.EnableDevice(Keyboard.current);
        InputSystem.EnableDevice(Mouse.current);
        keyboardImage.gameObject.SetActive(true);
        gamepadImage.gameObject.SetActive(false);
        sideKeyboardImage.gameObject.SetActive(false);
    }

    private void EnableGamepad()
    {
        if (Gamepad.current != null)
        {
            InputSystem.EnableDevice(Gamepad.current);
        }

        InputSystem.DisableDevice(Keyboard.current);
        InputSystem.DisableDevice(Mouse.current);
        keyboardImage.gameObject.SetActive(false);
        gamepadImage.gameObject.SetActive(true);
        sideKeyboardImage.gameObject.SetActive(false);
    }

    private void EnableBoth()
    {
        if (Gamepad.current != null)
        {
            InputSystem.EnableDevice(Gamepad.current);
        }

        InputSystem.EnableDevice(Keyboard.current);
        InputSystem.EnableDevice(Mouse.current);
        keyboardImage.gameObject.SetActive(false);
        gamepadImage.gameObject.SetActive(true);
        sideKeyboardImage.gameObject.SetActive(true);
    }

    private void SaveDevice()
    {
        DeviceConfig config = new DeviceConfig
        {
            selectedDevice = (int)currentDevice
        };

        string json = JsonUtility.ToJson(config, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveDevicePath), json);
    }

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

    private void OnDestroy()
    {
        inputDeviceDropdown.onValueChanged.RemoveListener(OnDeviceDropdownChange);
    }
}
