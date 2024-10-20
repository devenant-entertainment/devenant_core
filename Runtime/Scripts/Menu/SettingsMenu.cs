using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [Header("Localization")]
        [SerializeField] private TMP_Dropdown localeDropdown;

        [Header("Graphics")]
        [SerializeField] private Slider qualitySlider;

        [Header("Interface")]
        [SerializeField] private Slider interfaceScaleSlider;

        [Header("Audio")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        [Header("Links")]
        [SerializeField] private Button supportUrlButton;
        [SerializeField] private Button gameUrlButton;
        [SerializeField] private Button storeUrlButton;

        [Header("Menu")]
        [SerializeField] private Button exitButton;
        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            SetupLocale();

            SetupQuality();

            SetupInterfaceScale();

            SetupMasterVolume();
            SetupMusicVolume();
            SetupSfxVolume();

            supportUrlButton.onClick.RemoveAllListeners();
            supportUrlButton.onClick.AddListener(() =>
            {
                UnityEngine.Application.OpenURL(ApplicationManager.instance.data.supportUrl);
            });

            gameUrlButton.onClick.RemoveAllListeners();
            gameUrlButton.onClick.AddListener(() =>
            {
                UnityEngine.Application.OpenURL(ApplicationManager.instance.data.gameUrl);
            });

            storeUrlButton.onClick.RemoveAllListeners();
            storeUrlButton.onClick.AddListener(() =>
            {
                UnityEngine.Application.OpenURL(ApplicationManager.instance.data.storeUrl);
            });

            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(() =>
            {
                MessageMenu.instance.Open("dialogue_exit", (bool success) =>
                {
                    if(success)
                    {
                        ApplicationManager.instance.Exit();
                    }
                });
            });

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                SettingsManager.instance.Save();

                Close();
            });

            base.Open(callback);
        }

        public override void Close(Action callback = null)
        {
            SettingsManager.instance.Save();

            base.Close(callback);
        }

        private void SetupLocale()
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach(string locale in LocalizationManager.instance.locales)
            {
                options.Add(new TMP_Dropdown.OptionData(locale));
            }

            localeDropdown.ClearOptions();
            localeDropdown.AddOptions(options);

            localeDropdown.value = SettingsManager.instance.settings.locale;
            localeDropdown.onValueChanged.RemoveAllListeners();
            localeDropdown.onValueChanged.AddListener((int value) =>
            {
                SettingsManager.instance.SetLocale(value);
            });
        }

        private void SetupQuality()
        {
            qualitySlider.wholeNumbers = true;
            qualitySlider.minValue = 0;
            qualitySlider.maxValue = QualitySettings.names.Length - 1;

            qualitySlider.value = SettingsManager.instance.settings.quality;

            qualitySlider.onValueChanged.RemoveAllListeners();
            qualitySlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetQuality(Mathf.RoundToInt(value));
            });
        }

        private void SetupInterfaceScale()
        {
            float minInterfaceScale = ApplicationManager.instance.data.minInterfaceScale;
            float maxInterfaceScale = ApplicationManager.instance.data.maxInterfaceScale;

            interfaceScaleSlider.wholeNumbers = false;
            interfaceScaleSlider.minValue = minInterfaceScale;
            interfaceScaleSlider.maxValue = maxInterfaceScale;

            interfaceScaleSlider.value = maxInterfaceScale - SettingsManager.instance.settings.interfaceScale + minInterfaceScale;

            interfaceScaleSlider.onValueChanged.RemoveAllListeners();
            interfaceScaleSlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetInterfaceScale(maxInterfaceScale - value + minInterfaceScale);
            });
        }

        private void SetupMasterVolume()
        {
            masterVolumeSlider.wholeNumbers = true;
            masterVolumeSlider.minValue = 0;
            masterVolumeSlider.maxValue = 100;

            masterVolumeSlider.value = SettingsManager.instance.settings.masterVolume;

            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            masterVolumeSlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetMasterVolume(Mathf.RoundToInt(value));
            });
        }

        private void SetupMusicVolume()
        {
            musicVolumeSlider.wholeNumbers = true;
            musicVolumeSlider.minValue = 0;
            musicVolumeSlider.maxValue = 100;

            musicVolumeSlider.value = SettingsManager.instance.settings.musicVolume;

            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetMusicVolume(Mathf.RoundToInt(value));
            });
        }

        private void SetupSfxVolume()
        {
            sfxVolumeSlider.wholeNumbers = true;
            sfxVolumeSlider.minValue = 0;
            sfxVolumeSlider.maxValue = 100;

            sfxVolumeSlider.value = SettingsManager.instance.settings.sfxVolume;

            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetSfxVolume(Mathf.RoundToInt(value));
            });
        }
    }
}
