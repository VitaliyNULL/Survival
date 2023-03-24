using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VitaliyNULL.Fusion
{
    public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        #region Private Fields

        private NetworkRunner _runner;

        #endregion

        #region Public Methods

        public void Host()
        {
            StartGame(GameMode.Host);
        }

        public void Join()
        {
            StartGame(GameMode.Client);
        }


        #endregion

        #region Private Methods

        private async void StartGame(GameMode mode)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        #endregion

        #region INetworkRunnerCallbacks

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player with id: {player.PlayerId} joined the room ");
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player with id: {player.PlayerId} left the room ");
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        #endregion
    }
}