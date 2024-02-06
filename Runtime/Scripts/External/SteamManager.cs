#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using UnityEngine;
using Devenant;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

[DisallowMultipleComponent]
public class SteamManager : Singleton<SteamManager>
{
#if !DISABLESTEAMWORKS
    private static bool everInitialized = false;

    protected bool _initialized = false;
    public bool initialized { get { return _initialized; } private set {  _initialized = value; } }

    protected SteamAPIWarningMessageHook_t warningMessageHook;

    [AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
    protected static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
    {
        Debug.LogWarning(pchDebugText);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitOnPlayMode()
    {
        everInitialized = false;
    }

    private void Awake()
    {
        if(everInitialized)
        {
            throw new System.Exception("Tried to Initialize the SteamAPI twice in one session!");
        }

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
            if(SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
            {
                Devenant.Application.Exit();

                return;
            }
        }
        catch(System.DllNotFoundException e)
        {
            Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

            Devenant.Application.Exit();

            return;
        }

        initialized = SteamAPI.Init();

        if(!initialized)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

            return;
        }

        everInitialized = true;
    }

    private void OnEnable()
    {
        if(!initialized)
        {
            return;
        }

        if(warningMessageHook == null)
        {
            warningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);

            SteamClient.SetWarningMessageHook(warningMessageHook);
        }
    }

    private void OnDestroy()
    {
        if(!_initialized)
        {
            return;
        }

        SteamAPI.Shutdown();
    }

    protected virtual void Update()
    {
        if(initialized)
        {
            SteamAPI.RunCallbacks();
        }
    }
#endif
}