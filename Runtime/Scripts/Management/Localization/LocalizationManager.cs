using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Devenant
{
    [RequireComponent(typeof(InitializableObject))]
    public class LocalizationManager : Singleton<LocalizationManager>, IInitializable
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

        public void Initialize(Action<InitializationResponse> callback)
        {
            StartCoroutine(InitializeCoroutine());

            IEnumerator InitializeCoroutine()
            {
                AsyncOperationHandle<LocalizationSettings> initializationOperation = LocalizationSettings.InitializationOperation;

                yield return initializationOperation.WaitForCompletion();

                callback?.Invoke(new InitializationResponse(initializationOperation.IsDone));
            }
        }

        public string Translate(string table, string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
        }
    }
}
