using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Devenant
{
    public enum PlayerType
    {
        Host,
        Client
    }

    public class Player : NetworkBehaviour
    {
        public static Player instance;

        public static Action<Player> onPlayerConnected;
        public static Action<Player> onPlayerDisconnected;
        public static Action<Player> onPlayerUpdated;

        public Action<Player> onUpdated;

        public PlayerNetworkData data { get { return JsonUtility.FromJson<PlayerNetworkData>(_data.Value.ToString()); } private set { _data.Value = JsonUtility.ToJson(value); } }
        private NetworkVariable<FixedString64Bytes> _data = new NetworkVariable<FixedString64Bytes>();

        private void OnEnable()
        {
            _data.OnValueChanged += (FixedString64Bytes previousValue, FixedString64Bytes newValue) => { onUpdated?.Invoke(this); onPlayerUpdated?.Invoke(this); };
        }

        private void Start()
        {
            if (IsLocalPlayer)
            {
                PlayerType type = IsHost || IsServer ? PlayerType.Host : PlayerType.Client;

                Setup(new PlayerNetworkData(type, UserManager.instance.user.nickname, UserManager.instance.user.avatar));

                instance = this;
            }

            onPlayerConnected?.Invoke(this);
        }

        private void OnDisable()
        {
            onPlayerDisconnected?.Invoke(this);
        }

        private void Setup(PlayerNetworkData data)
        {
            SetupServerRpc(JsonUtility.ToJson(data));

            Debug.Log("Player.Setup > " + JsonUtility.ToJson(data));
        }

        [ServerRpc]
        private void SetupServerRpc(string data)
        {
            this.data = JsonUtility.FromJson<PlayerNetworkData>(data);

            Debug.Log("Player.SetupServerRpc > " + data);
        }
    }
}
