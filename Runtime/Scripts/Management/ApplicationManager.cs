using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Devenant
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public static Action OnApplicationSetup;

        public enum Environment
        {
            Production,
            Development
        }

        public ApplicationData applicationData { get { return _applicationData; } private set { _applicationData = value; } }
        [SerializeField] private ApplicationData _applicationData;

        [SerializeField] private Environment environment;

        private void Start()
        {
            DontDestroyOnLoad(this);

            Setup();
        }

        private async void Setup()
        {
            InitializationOptions options = new InitializationOptions();
            options.SetEnvironmentName(environment.ToString().ToLower());

            await UnityServices.InitializeAsync(options);

            LoadingMenu.instance.Open(() =>
            {
                LocalizationManager.instance.Initialize(() =>
                {
                    SettingsManager.instance.Load();

                    ConfigurationManager.instance.Initialize((bool success) =>
                    {
                        if(success)
                        {
                            PurchaseManager.instance.Setup((bool success) =>
                            {
                                LoadingMenu.instance.Close(() =>
                                {
                                    if(success)
                                    {
                                        OnApplicationSetup?.Invoke();
                                    }
                                    else
                                    {
                                        MessageMenu.instance.Open("error", () =>
                                        {
                                            Exit();
                                        });
                                    }
                                });
                            });
                        }
                        else
                        {
                            LoadingMenu.instance.Close();

                            MessageMenu.instance.Open("error", () =>
                            {
                                Exit();
                            });
                        }
                    });
                });
            });
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}