using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Devenant
{
    public class LocalizationSettingsManager : Singleton<LocalizationSettingsManager>
    {
        public int locale
        {
            get
            {
                return LocalizationSettings.Instance.GetAvailableLocales().Locales.IndexOf(LocalizationSettings.Instance.GetSelectedLocale());
            }
            set
            {
                if (value != locale)
                {
                    LocalizationSettings.Instance.SetSelectedLocale(LocalizationSettings.Instance.GetAvailableLocales().Locales[value]);
                }
            }
        }

        public string[] locales
        {
            get
            {
                List<string> result = new List<string>();

                foreach(Locale locale in LocalizationSettings.Instance.GetAvailableLocales().Locales)
                {
                    result.Add(locale.LocaleName);
                }

                return result.ToArray();
            }
        }

        public void Setup(Action callback = null)
        {
            StartCoroutine(SetupCoroutine());

            IEnumerator SetupCoroutine()
            {
                AsyncOperationHandle<LocalizationSettings> initializationOperation = LocalizationSettings.InitializationOperation;

                yield return initializationOperation.WaitForCompletion();

                if(initializationOperation.IsDone)
                {
                    callback?.Invoke();
                }
            }
        }

        public string Translate(string table, string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
        }
    }
}
