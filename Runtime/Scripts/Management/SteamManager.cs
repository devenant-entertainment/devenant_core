#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
using UnityEngine;
using Steamworks;

namespace Devenant
{
    public class SteamManager : Singleton<SteamManager>
    {
        public bool initialized { get { return _initialized; } private set { _initialized = value; } }
        private bool _initialized = false;

        private SteamAPIWarningMessageHook_t warningMessageHook;

        [AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
        protected static void DebugSteamWarningMessage(int nSeverity, System.Text.StringBuilder pchDebugText)
        {
            Debug.LogWarning(pchDebugText);
        }

        private void OnDestroy()
        {
            if(initialized)
            {
                SteamAPI.Shutdown();
            }
        }

        private void Update()
        {
            if(initialized)
            {
                SteamAPI.RunCallbacks();
            }
        }

        public void Setup(Action<bool> callback)
        {
            if(!Packsize.Test())
            {
                Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
            }

            if(!DllCheck.Test())
            {
                Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
            }

            try
            {
                if(!SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
                {
                    initialized = SteamAPI.Init();

                    if(initialized)
                    {
                        warningMessageHook = new SteamAPIWarningMessageHook_t(DebugSteamWarningMessage);

                        SteamClient.SetWarningMessageHook(warningMessageHook);

                        callback?.Invoke(true);
                    }
                    else
                    {
                        Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

                        callback?.Invoke(false);
                    }
                }
                else
                {
                    Debug.LogError("[Steamworks.NET] RestartAppIfNecessary() successed. Refer to Valve's documentation or the comment above this line for more information.", this);

                    callback?.Invoke(false);
                }
            }
            catch(System.DllNotFoundException e)
            {
                Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

                callback?.Invoke(false);
            }
        }
    }
}
#endif