using UnityEngine;

namespace Devenant
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        private const string dataKey = "SettingsManager.Data";

        [System.Serializable]
        public class Data 
        {
            public int masterVolume = 100;
            public int musicVolume = 100;
            public int sfxVolume = 100;

            public int locale = 0;
        }

        public Data data { get { return _data; } private set { _data = value; } }
        private Data _data = new Data();

        public void Load()
        {
            if(PlayerPrefs.HasKey(dataKey))
            {
                data = JsonUtility.FromJson<Data>(PlayerPrefs.GetString(dataKey));

                SetMasterVolume(data.masterVolume);
                SetMusicVolume(data.musicVolume);
                SetSfxVolume(data.sfxVolume);
                SetLocale(data.locale);
            }
        }

        public void SetMasterVolume(int volume)
        {
            data.masterVolume = Mathf.Clamp(volume, 0, 100);

            AudioManager.instance.master.volume = data.masterVolume;
        }

        public void SetMusicVolume(int volume)
        {
            data.musicVolume = Mathf.Clamp(volume, 0, 100);

            AudioManager.instance.music.volume = data.musicVolume;
        }

        public void SetSfxVolume(int volume)
        {
            data.sfxVolume = Mathf.Clamp(volume, 0, 100);

            AudioManager.instance.sfx.volume = data.sfxVolume;
        }

        public void SetLocale(int locale)
        {
            data.locale = locale;

            LocalizationManager.instance.locale = data.locale;
        }

        public void Save()
        {
            PlayerPrefs.SetString(dataKey, JsonUtility.ToJson(data));
        }
    }
}