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

        languageDropdown.onValueChanged.AddListener(OnLanguageChange);
        InitDropdown();
    }

    private void InitDropdown()
    {
        IList<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        languageDropdown.options.Clear();

        foreach (Locale locale in locales)
        {
            languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
        }

        Locale currentLocale = LocalizationSettings.SelectedLocale;
        languageDropdown.value = locales.IndexOf(currentLocale);
    }

    private void OnLanguageChange(int index)
    {
        IList<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        Locale selectLocales = locales[index];
        LocalizationSettings.SelectedLocale = selectLocales;

        SaveLanguageSetting(selectLocales.Identifier.Code);
    }

    private void SaveLanguageSetting(string localeCode)
    {
        Language language = new Language
        {
            selectedLocaleCode = localeCode
        };

        string json = JsonUtility.ToJson(language, true);
        string path = Path.Combine(Application.persistentDataPath, SelectedLanguageFileName);
        File.WriteAllText(path, json);
    }

    private void LoadLanguageSetting()
    {
        string path = Path.Combine(Application.persistentDataPath, SelectedLanguageFileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Language language = JsonUtility.FromJson<Language>(json);

            if (!string.IsNullOrEmpty(language.selectedLocaleCode))
            {
                IList<Locale> availableLocales = LocalizationSettings.AvailableLocales.Locales;

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
        else
        {
            Debug.LogWarning("No Language Setting Save Files.");
        }
    }
}
