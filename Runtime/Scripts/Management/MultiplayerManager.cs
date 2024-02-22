using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.Events;
using Unity.Services.Core;
using Unity.Services.Authentication;

namespace Devenant
{
    public class MultiplayerManager : Singleton<MultiplayerManager>
    {
        public static Action onPlayersUpdated;
        public static Action onDisconnected;

        public class Session
        {
            public readonly string code;
            public readonly List<NetworkPlayer> players = new List<NetworkPlayer>();

            public Session(string code)
            {
                this.code = code;
            }

            public void AddPlayer(NetworkPlayer player)
            {
                if(!players.Contains(player))
                {
                    players.Add(player);
                }
            }

            public void RemovePlayer(NetworkPlayer player)
            {
                if(players.Contains(player))
                {
                    players.Remove(player);
                }
            }
        }

        public Session session { get { return _session; } private set { _session = value; } }
        private Session _session;

        private void OnEnable()
        {
            NetworkPlayer.onPlayerConnected += PlayerConnected;
            NetworkPlayer.onPlayerDisconnected += PlayerDisconnected;
        }

        private void OnDisable()
        {
            NetworkPlayer.onPlayerConnected -= PlayerConnected;
            NetworkPlayer.onPlayerDisconnected -= PlayerDisconnected;
        }

        private void PlayerConnected(NetworkPlayer player)
        {
            session?.AddPlayer(player);

            onPlayersUpdated?.Invoke();
        }

        private void PlayerDisconnected(NetworkPlayer player)
        {
            session?.RemovePlayer(player);

            onPlayersUpdated?.Invoke();

            if(player.IsHost)
            {
                Disconnect();
            }
        }

        public async void StartHost(int maxPlayers, UnityAction<bool> callback = null)
        {
            if(session != null)
                Disconnect();

            try
            {
                string code = await Host(maxPlayers);

                if (!string.IsNullOrEmpty(code))
                {
                    session = new Session(code);

                    callback?.Invoke(true);
                }
                else
                {
                    session = null;

                    callback?.Invoke(false);
                }
            }
            catch(RelayServiceException e)
            {
                Debug.LogError(e);

                session = null;

                callback?.Invoke(false);
            }
        }

        public async void StartClient(string code, UnityAction<bool> callback = null)
        {
            if(session != null)
                Disconnect();

            try
            {
                bool success = await Join(code);

                if(success)
                {
                    session = new Session(code);

                    callback?.Invoke(true);
                }
                else
                {
                    session = null;

                    callback?.Invoke(false);
                }
            }
            catch(RelayServiceException e)
            {
                Debug.LogError(e);

                session = null;

                callback?.Invoke(false);
            }
        }

        private async Task<string> Host(int maxPlayers)
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

            return NetworkManager.Singleton.StartHost() ? code : string.Empty;
        }

        private async Task<bool> Join(string code)
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

            return NetworkManager.Singleton.StartClient();
        }

        public void Disconnect()
        {
            NetworkManager.Singleton.Shutdown();

            onDisconnected?.Invoke();
        }
    }
}
