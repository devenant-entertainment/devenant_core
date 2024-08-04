using System;
using UnityEngine;

namespace Devenant
{
    public class StatusManager : Singleton<StatusManager>, IInitializable
    {
        public StatusData data { get { return _data; } private set { _data = value; } }
        private StatusData _data;

        public void Initialize(Action<InitializationResponse> callback)
        {
            Request.Get(BackendManager.instance.data.status, ((Request.Response response) =>
            {
                if (response.success)
                {
                    data = new StatusData(JsonUtility.FromJson<StatusDataResponse>(response.data));

                    switch (data.status)
                    {
                        case ApplicationStatus.Active:

                            if (new Version(Application.version).Compare(data.version) != Version.Comparison.Greater)
                            {
                                callback?.Invoke(new InitializationResponse(true));
                            }
                            else
                            {
                                MessageMenu.instance.Open("dialogue_version", ((bool success) =>
                                {
                                    if (success)
                                    {
                                        Application.OpenURL(ApplicationManager.instance.data.storeUrl);

                                    }

                                    callback?.Invoke(new InitializationResponse(false));
                                }));
                            }

                            break;

                        case ApplicationStatus.Inactive:

                            callback?.Invoke(new InitializationResponse(false, "error_maintenance"));

                            break;
                    }
                }
                else
                {
                    callback?.Invoke(new InitializationResponse(false));
                }
            }));
        }
    }
}
