using System;
using UnityEngine;

namespace Devenant
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        private const string dataKey = "SettingsManager.Data";

        public SettingsData settings { get { return _settings; } private set { _settings = value; } }
        private SettingsData _settings;

        public void Initialize(Action<InitializationResponse> callback)
        {
            if(PlayerPrefs.HasKey(dataKey))
            {
                settings = JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString(dataKey));
            }
            else
            {
                settings = new SettingsData();

                settings.locale = LocalizationManager.instance.locale;

                settings.quality = QualitySettings.GetQualityLevel();

                settings.interfaceScale = 1.0f;

                settings.masterVolume = 100;
                settings.musicVolume = 100;
                settings.sfxVolume = 100;

                Save();
            }

            SetLocale(settings.locale);

            SetQuality(settings.quality);

            SetInterfaceScale(settings.interfaceScale);

            SetMasterVolume(settings.masterVolume);
            SetMusicVolume(settings.musicVolume);
            SetSfxVolume(settings.sfxVolume);

            callback?.Invoke(new InitializationResponse(true));
        }

        public void SetLocale(int locale)
        {
            settings.locale = Mathf.Clamp(locale, 0, LocalizationManager.instance.locales.Length - 1);

            LocalizationManager.instance.locale = settings.locale;
        }

        public void SetQuality(int quality)
        {
            settings.quality = Mathf.Clamp(quality, 0, QualitySettings.names.Length - 1);

            QualitySettings.SetQualityLevel(settings.quality);
        }

        public void SetInterfaceScale(float scale)
        {
            settings.interfaceScale = scale;

            InterfaceManager.instance.SetScale(settings.interfaceScale);
        }

        public void SetMasterVolume(int volume)
        {
            settings.masterVolume = Mathf.Clamp(volume, 0, 100);

            AudioManager.instance.master.volume = settings.masterVolume;
        }

        public void SetMusicVolume(int volume)
        {
            settings.musicVolume = Mathf.Clamp(volume, 0, 100);

            AudioManager.instance.music.volume = settings.musicVolume;
        }

        public void SetSfxVolume(int volume)
        {
            settings.sfxVolume = Mathf.Clamp(volume, 0, 100);

            AudioManager.instance.sfx.volume = settings.sfxVolume;
        }

        public void Save()
        {
            PlayerPrefs.SetString(dataKey, JsonUtility.ToJson(settings));
        }
    }
}