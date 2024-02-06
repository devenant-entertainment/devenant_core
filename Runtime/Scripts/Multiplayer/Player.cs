using Unity.Netcode;

namespace Devenant
{
    public class Player : NetworkBehaviour
    {
        public static Action<Player> onPlayerConnected;
        public static Action<Player> onPlayerDisconnected;

        public string nickname { get { return _nickname; } private set { _nickname = value; } }
        private string _nickname;

        public string avatar { get { return _avatar; } private set { _avatar = value; } }
        private string _avatar;

        private void Start()
        {
            if (IsLocalPlayer)
            {
                SetupServerRpc(UserManager.instance.data.nickname, UserManager.instance.data.avatar);
            }
        }

        private void OnDisable()
        {
            onPlayerDisconnected?.Invoke(this);
        }

        [ServerRpc]
        private void SetupServerRpc(string nickname, string avatar)
        {
            if(IsHost || IsServer)
            {
                this.nickname = nickname;
                this.avatar = avatar;

                onPlayerConnected?.Invoke(this);

                SetupClientRpc(nickname, avatar);
            }
        }

        [ClientRpc]
        private void SetupClientRpc(string nickname, string avatar)
        {
            if(!IsHost && !IsServer)
            {
                this.nickname = nickname;
                this.avatar = avatar;

                onPlayerConnected?.Invoke(this);
            }
        }
    }
}
