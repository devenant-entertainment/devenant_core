using UnityEngine;

namespace Devenant
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        private const string dataKey = "SettingsManager.Data";

        public Settings settings { get { return _settings; } private set { _settings = value; } }
        private Settings _settings;

        public void Setup()
        {
            if(PlayerPrefs.HasKey(dataKey))
            {
                settings = JsonUtility.FromJson<Settings>(PlayerPrefs.GetString(dataKey));
            }
            else
            {
                settings = new Settings();

                settings.locale = LocalizationManager.instance.locale;

                settings.resolution = Screen.resolutions.Length - 1;
                settings.fullScreenMode = (int)FullScreenMode.FullScreenWindow;
                settings.quality = QualitySettings.GetQualityLevel();

                settings.interfaceScale = 1.0f;

                settings.masterVolume = 100;
                settings.musicVolume = 100;
                settings.sfxVolume = 100;

                Save();
            }

            SetLocale(settings.locale);

            SetResolution(settings.resolution);
            SetFullScreenMode(settings.fullScreenMode);
            SetQuality(settings.quality);

            SetInterfaceScale(settings.interfaceScale);

            SetMasterVolume(settings.masterVolume);
            SetMusicVolume(settings.musicVolume);
            SetSfxVolume(settings.sfxVolume);
        }

        public void SetLocale(int locale)
        {
            settings.locale = locale;

            LocalizationManager.instance.locale = settings.locale;
        }

        public void SetResolution(int resolution)
        {
            if(ApplicationManager.instance.application.platform != ApplicationPlatform.Steam)
                return;

            settings.resolution = resolution;

            Screen.SetResolution(Screen.resolutions[settings.resolution].width, Screen.resolutions[settings.resolution].height, (FullScreenMode)settings.fullScreenMode);
        }

        public void SetFullScreenMode(int fullScreenMode)
        {
            if(ApplicationManager.instance.application.platform != ApplicationPlatform.Steam)
                return;

            settings.fullScreenMode = fullScreenMode;

            Screen.SetResolution(Screen.resolutions[settings.resolution].width, Screen.resolutions[settings.resolution].height, (FullScreenMode)settings.fullScreenMode);
        }

        public void SetQuality(int quality)
        {
            settings.quality = Mathf.Clamp(quality,0, QualitySettings.names.Length);

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