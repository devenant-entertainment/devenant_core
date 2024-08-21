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
        [SerializeField] private ApplicationAsset applicationData;

        [SerializeField] private InitializableObject[] initializables;

        public async void Initialize(Action callback = null)
        {
            InitializationOptions options = new InitializationOptions();

            options.SetEnvironmentName(data.environment.ToString().ToLower());

            await UnityServices.InitializeAsync(options);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            InitialLoadingMenu.instance.Open(() =>
            {
                InitializeInitializables(() =>
                {
                    InitialLoadingMenu.instance.Close(() =>
                    {
                        callback?.Invoke();
                    });
                });
            });
        }

        private async void InitializeInitializables(Action callback)
        {
            for (int i = 0; i < initializables.Length; i ++)
            {
                InitializationResponse response = await Initialize(initializables[i].initializable);

                InitialLoadingMenu.instance.SetValue((float)i / (float)initializables.Length);

                if (!response.success)
                {
                    Debug.Log("Initialization failed at " + initializables[i].gameObject.name);

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
