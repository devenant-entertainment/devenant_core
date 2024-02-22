using UnityEngine;

namespace Devenant
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        private const string dataKey = "SettingsManager.Data";

        public Settings settings { get { return _settings; } private set { _settings = value; } }
        private Settings _settings = new Settings();

        public void Load()
        {
            if(PlayerPrefs.HasKey(dataKey))
            {
                settings = JsonUtility.FromJson<Settings>(PlayerPrefs.GetString(dataKey));

                SetMasterVolume(settings.masterVolume);
                SetMusicVolume(settings.musicVolume);
                SetSfxVolume(settings.sfxVolume);
                SetLocale(settings.locale);
            }
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

        public void SetLocale(int locale)
        {
            settings.locale = locale;

            LocalizationManager.instance.locale = settings.locale;
        }

        public void Save()
        {
            PlayerPrefs.SetString(dataKey, JsonUtility.ToJson(settings));
        }
    }
}