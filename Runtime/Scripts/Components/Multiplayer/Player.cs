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

        public PlayerType type { get { return (PlayerType)_type.Value; } private set { _type.Value = (int)value; } }
        private NetworkVariable<int> _type = new NetworkVariable<int>();

        public string nickname { get { return _nickname.Value.ToString(); } private set { _nickname.Value = new FixedString32Bytes(value); } }
        private NetworkVariable<FixedString32Bytes> _nickname = new NetworkVariable<FixedString32Bytes>();

        public string avatar { get { return _avatar.Value.ToString(); } private set { _avatar.Value = new FixedString32Bytes(value); } }
        private NetworkVariable<FixedString32Bytes> _avatar = new NetworkVariable<FixedString32Bytes>();

        private void Start()
        {
            if (IsLocalPlayer)
            {
                PlayerType type = IsHost || IsServer ? PlayerType.Host : PlayerType.Client;

                SetupServerRpc((int)type, UserManager.instance.user.nickname, UserManager.instance.user.avatar);

                instance = this;
            }

            _type.OnValueChanged += (int previousValue, int newValue) => { onPlayerUpdated?.Invoke(this); };
            _nickname.OnValueChanged += (FixedString32Bytes previousValue, FixedString32Bytes newValue) => { onPlayerUpdated?.Invoke(this); };
            _avatar.OnValueChanged += (FixedString32Bytes previousValue, FixedString32Bytes newValue) => { onPlayerUpdated?.Invoke(this); };

            onPlayerConnected?.Invoke(this);
        }

        private void OnDisable()
        {
            onPlayerDisconnected?.Invoke(this);
        }

        [ServerRpc]
        private void SetupServerRpc(int type, string nickname, string avatar)
        {
            this.type = (PlayerType)type;
            this.nickname = nickname;
            this.avatar = avatar;
        }
    }
}
