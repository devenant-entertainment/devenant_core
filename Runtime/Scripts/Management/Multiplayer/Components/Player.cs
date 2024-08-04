using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Devenant
{
    public enum PlayerType
    {
        None,
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

        public PlayerType type { get { return _type.Value; } }
        private NetworkVariable<PlayerType> _type = new NetworkVariable<PlayerType>(PlayerType.None);

        public string nickname { get { return _nickname.Value.ToString(); } }
        private NetworkVariable<FixedString64Bytes> _nickname = new NetworkVariable<FixedString64Bytes>();

        public string avatar { get { return _avatar.Value.ToString(); } }
        private NetworkVariable<FixedString64Bytes> _avatar = new NetworkVariable<FixedString64Bytes>();

        private void OnEnable()
        {
            _type.OnValueChanged += (PlayerType previousValue, PlayerType newValue) => { onUpdated?.Invoke(this); onPlayerUpdated?.Invoke(this); };
            _nickname.OnValueChanged += (FixedString64Bytes previousValue, FixedString64Bytes newValue) => { onUpdated?.Invoke(this); onPlayerUpdated?.Invoke(this); };
            _avatar.OnValueChanged += (FixedString64Bytes previousValue, FixedString64Bytes newValue) => { onUpdated?.Invoke(this); onPlayerUpdated?.Invoke(this); };
        }

        private void Start()
        {
            if(IsLocalPlayer)
            {
                Setup(IsServer ? PlayerType.Host : PlayerType.Client, UserManager.instance.data.nickname, UserManager.instance.data.avatar);

                instance = this;
            }

            onPlayerConnected?.Invoke(this);
        }

        private void OnDisable()
        {
            onPlayerDisconnected?.Invoke(this);
        }

        private void Setup(PlayerType type, string nickname, string avatar)
        {
            SetupServerRpc(type, nickname, avatar);

            Debug.Log("Player.Setup > " + string.Format("Type: {0}; Nickname: {1}; Avatar: {2};", type.ToString(), nickname, avatar));
        }

        [ServerRpc]
        private void SetupServerRpc(PlayerType type, string nickname, string avatar)
        {
            _type.Value = type;
            _nickname.Value = nickname;
            _avatar.Value = avatar;

            Debug.Log("Player.SetupServerRpc > " + string.Format("Type: {0}; Nickname: {1}; Avatar: {2};", type.ToString(), nickname, avatar));
        }
    }
}
