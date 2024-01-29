using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] private TMP_Dropdown localeDropdown;

        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        [SerializeField] private Button manageButton;
        [SerializeField] private Button infoButton;

        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach(string locale in LocalizationManager.instance.locales)
                options.Add(new TMP_Dropdown.OptionData(locale));

            localeDropdown.ClearOptions();
            localeDropdown.AddOptions(options);
            localeDropdown.value = SettingsManager.instance.data.locale;
            localeDropdown.onValueChanged.RemoveAllListeners();
            localeDropdown.onValueChanged.AddListener((int value) =>
            {
                SettingsManager.instance.SetLocale(value);
            });

            masterVolumeSlider.value = SettingsManager.instance.data.masterVolume;
            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            masterVolumeSlider.onValueChanged.AddListener((float value) => 
            {
                SettingsManager.instance.SetMasterVolume(Mathf.RoundToInt(value));
            });

            musicVolumeSlider.value = SettingsManager.instance.data.musicVolume;
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetMusicVolume(Mathf.RoundToInt(value));
            });

            sfxVolumeSlider.value = SettingsManager.instance.data.sfxVolume;
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.AddListener((float value) =>
            {
                SettingsManager.instance.SetSfxVolume(Mathf.RoundToInt(value));
            });

            manageButton.onClick.RemoveAllListeners();
            manageButton.onClick.AddListener(() =>
            {
                Application.OpenURL(DataManager.instance.applicationData.storeUrl.value);
            });

            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(() => 
            {
                Application.OpenURL(DataManager.instance.applicationData.infoUrl);
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
