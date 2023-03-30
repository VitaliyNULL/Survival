using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerInput : NetworkBehaviour, INetworkRunnerCallbacks
    {
        #region Private Fields

        private NetworkRunner _runner;
        private NetworkCharacterControllerPrototype _controller;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _controller = GetComponent<NetworkCharacterControllerPrototype>();
            _runner = NetworkRunner.GetRunnerForScene(SceneManager.GetActiveScene());

            _runner.AddCallbacks(this);
        }
        
        #endregion

        #region NetworkBehaviour Callbacks

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.direction.Normalize(); 
                _controller?.Move(5*data.direction * Runner.DeltaTime);
            }
        }

        #endregion

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();
            if (Input.GetKey(KeyCode.W))
            {
                Debug.Log("Pressed W");
                data.direction += Vector3.up;
            }

            if (Input.GetKey(KeyCode.A))
            {
                Debug.Log("Pressed A");
                data.direction += Vector3.left;
            }

            if (Input.GetKey(KeyCode.S))
            {
                Debug.Log("Pressed S");
                data.direction += Vector3.down;
            }

            if (Input.GetKey(KeyCode.D))
            {
                Debug.Log("Pressed D");
                data.direction += Vector3.right;
            }

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
    }
}