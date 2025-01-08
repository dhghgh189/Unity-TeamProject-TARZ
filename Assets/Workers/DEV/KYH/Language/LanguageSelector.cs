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
    private int selectLanguage;

    private void Start()
    {
        PlayerPrefs.GetInt("CurrentLanguage");
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

        PlayerPrefs.SetInt("CurrentLanguage", index);
    }
}
