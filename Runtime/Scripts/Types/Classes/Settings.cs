using UnityEngine;

namespace Devenant
{
    [System.Serializable]
    public class Settings
    {
        public int locale = 0;

        public int resolution = Screen.resolutions.Length - 1;
        public int fullScreenMode = (int)FullScreenMode.FullScreenWindow;

        public int masterVolume = 100;
        public int musicVolume = 100;
        public int sfxVolume = 100;
    }
}
