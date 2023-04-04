using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using VitaliyNULL.Fusion;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerInput : MonoBehaviour, INetworkRunnerCallbacks
    {
        #region Private Fields

        private NetworkRunner _runner;
        private bool _touchedJoystick = false;
        [SerializeField] private VariableJoystick movementJoystick;
        [SerializeField] private VariableJoystick weaponJoystick;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _runner = NetworkRunner.GetRunnerForScene(SceneManager.GetActiveScene());
            _runner.AddCallbacks(this);
        }

        #endregion

        public void EventPointerDown()
        {
            Debug.Log("Can Shoot");
            _touchedJoystick = true;
        }

        public void EventPointerUp()
        {
            Debug.Log("Cannot shoot");
            _touchedJoystick = false;
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            if (_touchedJoystick)
            {
                data.isShoot = true;
            }
            else
            {
                data.isShoot = false;
            }

            Debug.Log("Data is shoot: " + data.isShoot);
            data.directionToMove = movementJoystick.Direction;
            data.directionToShoot = weaponJoystick.Direction;
            input.Set(data);
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

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
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

        public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            Debug.Log("OnHostMigration");
            _runner = NetworkRunner.GetRunnerForScene(SceneManager.GetActiveScene());
            _runner.AddCallbacks(this);
            await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);
            // Step 2.2
            // Create a new Runner.
            var newRunner = Instantiate(new GameObject("FusionManager").AddComponent<NetworkRunner>());
            FusionManager fusionManager = newRunner.gameObject.AddComponent<FusionManager>();
            fusionManager.playerController = Resources.Load<NetworkObject>("PlayerController");
            Debug.Log(fusionManager.playerController);
            // setup the new runner...
            // Start the new Runner using the "HostMigrationToken" and pass a callback ref in "HostMigrationResume".
            StartGameResult result = await newRunner.StartGame(new StartGameArgs()
            {
                // SessionName = SessionName,              // ignored, peer never disconnects from the Photon Cloud
                // GameMode = gameMode,                    // ignored, Game Mode comes with the HostMigrationToken
                HostMigrationToken = hostMigrationToken, // contains all necessary info to restart the Runner
                HostMigrationResume = HostMigrationResume, // this will be invoked to resume the simulation
                PlayerCount = 2,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
                // other args
            });

            // Check StartGameResult as usual
            if (result.Ok == false)
            {
                Debug.LogWarning(result.ShutdownReason);
            }
            else
            {
                Debug.Log("Done");
            }
        }

        void HostMigrationResume(NetworkRunner runner)
        {
            Debug.Log("HostMigrationResume");
            runner.AddCallbacks(this);
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
    }
}