using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] private TMP_Dropdown localeDropdown;
        [Space]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [Space]
        [SerializeField] private Button manageButton;
        [SerializeField] private Button infoButton;
        [Space]
        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach(string locale in LocalizationManager.instance.locales)
                options.Add(new TMP_Dropdown.OptionData(locale));

            localeDropdown.ClearOptions();
            localeDropdown.AddOptions(options);
            localeDropdown.value = SettingsManager.instance.settings.locale;
            localeDropdown.onValueChanged.RemoveAllListeners();
            localeDropdown.onValueChanged.AddListener((int value) =>
            {
                SettingsManager.instance.SetLocale(value);
            });

            masterVolumeSlider.value = SettingsManager.instance.settings.masterVolume;
            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            masterVolumeSlider.onValueChanged.AddListener((float value) => 
            {
                SettingsManager.instance.SetMasterVolume(Mathf.RoundToInt(value));
            });

            musicVolumeSlider.value = SettingsManager.instance.settings.musicVolume;
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetMusicVolume(Mathf.RoundToInt(value));
            });

            sfxVolumeSlider.value = SettingsManager.instance.settings.sfxVolume;
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetSfxVolume(Mathf.RoundToInt(value));
            });

            manageButton.onClick.RemoveAllListeners();
            manageButton.onClick.AddListener(() =>
            {
                UnityEngine.Application.OpenURL(ApplicationManager.instance.application.GetStoreUrl());
            });

            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(() => 
            {
                UnityEngine.Application.OpenURL(ApplicationManager.instance.application.gameUrl);
            });

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            base.Open(callback);
        }

        public override void Close(Action callback = null)
        {
            SettingsManager.instance.Save();

            base.Close(callback);
        }
    }
}
