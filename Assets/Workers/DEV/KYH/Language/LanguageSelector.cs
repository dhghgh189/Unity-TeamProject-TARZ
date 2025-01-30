using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown;

    private const string SelectedLanguageFileName = "Language.json";

    [System.Serializable]
    private class Language
    {
        public string selectedLocaleCode;
    }

    private void Start()
    {
        LoadLanguageSetting();

        // 언어 변경 드롭다운에 값 변경 시 언어를 변경하는 이벤트 등록
        languageDropdown.onValueChanged.AddListener(OnLanguageChange);
        InitDropdown();
    }

    /// <summary>
    /// 드롭다운 초기화 함수
    /// </summary>
    private void InitDropdown()
    {
        // 드롭다운의 값 초기화
        IList<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        languageDropdown.options.Clear();

        // 드롭다운의 값에 언어 로케일 추가
        foreach (Locale locale in locales)
        {
            languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
        }

        // 드롭다운의 값을 현재 선택 중인 언어 로케일 인덱스 값으로 설정
        Locale currentLocale = LocalizationSettings.SelectedLocale;
        languageDropdown.value = locales.IndexOf(currentLocale);
    }

    /// <summary>
    /// 언어 변경 기능 함수
    /// </summary>
    /// <param name="index"></param>
    private void OnLanguageChange(int index)
    {
        // 선택한 언어 로케일 값의 언어로 변경
        IList<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        Locale selectLocales = locales[index];
        LocalizationSettings.SelectedLocale = selectLocales;

        // 언어 변경 값 저장
        SaveLanguageSetting(selectLocales.Identifier.Code);
    }

    /// <summary>
    /// 언어 변경값 저장 기능 함수
    /// </summary>
    /// <param name="localeCode"></param>
    private void SaveLanguageSetting(string localeCode)
    {
        // 언어를 선택한 언어 로케일로 변경
        Language language = new Language
        {
            selectedLocaleCode = localeCode
        };

        // 선택한 언어 로케일을 json으로 저장
        string json = JsonUtility.ToJson(language, true);
        string path = Path.Combine(Application.persistentDataPath, SelectedLanguageFileName);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// 저장된 언어 변경값 불러오기 함수
    /// </summary>
    private void LoadLanguageSetting()
    {
        string path = Path.Combine(Application.persistentDataPath, SelectedLanguageFileName);

        // 저장된 언어 변경값이 있을 경우
        if (File.Exists(path))
        {
            // json으로 저장된 언어 변경값 불러오기
            string json = File.ReadAllText(path);
            Language language = JsonUtility.FromJson<Language>(json);

            // 선택한 언어 로케일 값이 비어있지 않은 경우
            if (!string.IsNullOrEmpty(language.selectedLocaleCode))
            {
                // 사용 가능한 언어 로케일 값 불러오기
                IList<Locale> availableLocales = LocalizationSettings.AvailableLocales.Locales;

                // 현재 선택 중인 로케일을 불러온 언어 로케일 값으로 설정
                foreach (Locale locale in availableLocales)
                {
                    if (locale.Identifier.Code == language.selectedLocaleCode)
                    {
                        LocalizationSettings.SelectedLocale = locale;
                        break;
                    }
                }
            }
        }
        // 저장된 언어 변경값이 없는 경우 저장된 값이 없다는 로그 출력
        else
        {
            Debug.LogWarning("No Language Setting Save Files.");
        }
    }
}
