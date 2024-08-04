using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Devenant
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public ApplicationData data { get { if (_data == null) { _data = new ApplicationData(applicationData); } return _data; } }
        private ApplicationData _data;
        [SerializeField] private ApplicationDataAsset applicationData;

        [SerializeField] private InitializableObject[] initializables;

        public async void Initialize(Action callback = null)
        {
            InitializationOptions options = new InitializationOptions();

            options.SetEnvironmentName(data.environment.ToString().ToLower());

            await UnityServices.InitializeAsync(options);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            LoadingMenu.instance.Open(() =>
            {
                InitializeInitializables(() =>
                {
                    LoadingMenu.instance.Close(() =>
                    {
                        callback?.Invoke();
                    });
                });
            });
        }

        private async void InitializeInitializables(Action callback)
        {
            foreach (InitializableObject initializable in initializables)
            {
                InitializationResponse response = await Initialize(initializable.initializable);

                if (!response.success)
                {
                    Debug.Log("Initialization failed at " + initializable.gameObject.name);

                    MessageMenu.instance.Open(response.error, () =>
                    {
                        Exit();
                    });

                    return;
                }
            }

            callback?.Invoke();
        }

        private Task<InitializationResponse> Initialize(IInitializable initializable)
        {
            TaskCompletionSource<InitializationResponse> task = new TaskCompletionSource<InitializationResponse>();

            initializable.Initialize((InitializationResponse response) =>
            {
                task.SetResult(response);
            });

            return task.Task;
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
