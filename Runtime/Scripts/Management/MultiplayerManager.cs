using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;

namespace Devenant
{
    public class MultiplayerManager : Singleton<MultiplayerManager>
    {
        public static Action onPlayersUpdated;
        public static Action onDisconnected;

        public class Session
        {
            public readonly string code;
            public readonly List<Player> players = new List<Player>();

            public Session(string code)
            {
                this.code = code.ToUpper();
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

        public Session session { get { return _session; } private set { _session = value; } }
        private Session _session;

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

            if(player.data.type == PlayerType.Host)
            {
                Disconnect();
            }
        }

        public async void StartHost(int maxPlayers, Action<bool> callback = null)
        {
            if(session != null)
            {
                Disconnect();
            }

            switch(ApplicationManager.instance.application.multiplayerMode)
            {
                case ApplicationMultiplayerMode.Local:

                    session = new Session(string.Empty);

                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", 7777);

                    NetworkManager.Singleton.StartHost();

                    callback?.Invoke(true);

                    break;

                case ApplicationMultiplayerMode.Online:

                    try
                    {
                        string code = await Host(maxPlayers);

                        if(!string.IsNullOrEmpty(code))
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
                        Debug.LogWarning(e);

                        session = null;

                        callback?.Invoke(false);
                    }

                    break;
            }
        }

        public async void StartClient(string code, Action<bool> callback = null)
        {
            if(session != null)
            {
                Disconnect();
            }

            switch(ApplicationManager.instance.application.multiplayerMode)
            {
                case ApplicationMultiplayerMode.Local:

                    session = new Session(code);

                    NetworkManager.Singleton.StartClient();

                    callback?.Invoke(true);

                    break;

                case ApplicationMultiplayerMode.Online:

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
                        Debug.LogWarning(e);

                        session = null;

                        callback?.Invoke(false);
                    }

                    break;
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
