using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeySettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject keyboardPanel;  // 키보드 키 설정 패널
    [SerializeField] private GameObject gamepadPanel;   // 게임패드 키 설정 패널
    [SerializeField] private Button keyboardButton;
    [SerializeField] private Button gamepadButton;
    private GameObject target;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == keyboardButton.gameObject)
        {
            if (keyboardPanel != null && !keyboardPanel.activeSelf)
            {
                OnClickKeyboardButton();
            }
        }
        else if (EventSystem.current.currentSelectedGameObject == gamepadButton.gameObject)
        {
            if (gamepadPanel != null && !gamepadPanel.activeSelf)
            {
                OnClickGamepadButton();
            }
        }
    }

    /// <summary>
    /// Keyboard & Mouse 버튼 클릭 시 키보드/마우스 조작키 알림
    /// </summary>
    public void OnClickKeyboardButton()
    {
        keyboardPanel.SetActive(true);
        gamepadPanel.SetActive(false);
    }

    /// <summary>
    /// Gamepad 버튼 클릭 시 게임패드 조작키 알림
    /// </summary>
    public void OnClickGamepadButton()
    {
        gamepadPanel.SetActive(true);
        keyboardPanel.SetActive(false);
    }
}
