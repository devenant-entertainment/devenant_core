using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.Events;

namespace Devenant
{
    public class MultiplayerManager : Singleton<MultiplayerManager>
    {
        public class Session
        {
            public readonly string code;
            public readonly List<Player> players = new List<Player>();

            public Session(string code)
            {
                this.code = code;
            }

            public void AddPlayer(Player player)
            {
                if(!players.Contains(player))
                {
                    players.Add(player);
                }
            }

            public void RemovePlayer(Player player)
            {
                if(players.Contains(player))
                {
                    players.Remove(player);
                }
            }
        }

        public Action onPlayersUpdated;
        public Action onDisconnected;

        public Session session;

        [SerializeField] private GameObject[] networkManagers;
        [SerializeField] private int maxPlayers;

        private void OnEnable()
        {
            Player.onPlayerConnected += PlayerConnected;
            Player.onPlayerDisconnected += PlayerDisconnected;
        }

        private void OnDisable()
        {
            Player.onPlayerConnected -= PlayerConnected;
            Player.onPlayerDisconnected -= PlayerDisconnected;
        }

        private void PlayerConnected(Player player)
        {
            session?.AddPlayer(player);

            onPlayersUpdated?.Invoke();
        }

        private void PlayerDisconnected(Player player)
        {
            session?.RemovePlayer(player);

            onPlayersUpdated?.Invoke();

            if(player.IsHost)
            {
                Disconnect();
            }
        }

        public async void StartServerHost(UnityAction<bool> callback = null)
        {
            if(session != null)
                Disconnect();

            try
            {
                string code = await Host();

                session = new Session(code);

                callback?.Invoke(true);
            }
            catch(RelayServiceException e)
            {
                Debug.LogError(e);

                session = null;

                callback?.Invoke(false);
            }
        }

        public async void StartServerClient(string code, UnityAction<bool> callback = null)
        {
            if(session != null)
                Disconnect();

            try
            {
                await Join(code);

                session = new Session(code);

                callback?.Invoke(true);
            }
            catch(RelayServiceException e)
            {
                Debug.LogError(e);

                session = null;

                callback?.Invoke(false);
            }
        }

        private async Task<string> Host()
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

            string code = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();

            foreach(GameObject setupObject in networkManagers)
            {
                NetworkObject networkObject = Instantiate(setupObject, transform).GetComponent<NetworkObject>();
                networkObject.Spawn();
            }

            return code;
        }

        private async Task<string> Join(string code)
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();

            return code;
        }

        public void Disconnect()
        {
            NetworkManager.Singleton.Shutdown();

            onDisconnected?.Invoke();
        }
    }
}
