using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using VitaliyNULL.MainMenuUI;

namespace VitaliyNULL.Fusion
{
    public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        #region Private Fields

        private NetworkRunner _runner;
        private SessionInfo _sessionInfo;
        private readonly string _sceneName = "GameScene";
        private PlayerRef _player;
        private readonly string _lobbyName = "MainLobby";

        #endregion

        #region Public Fields

        public static FusionManager Instance;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }

        #endregion

        #region Private Methods

        private IEnumerator WaitForCreatedRoom(string sessionInfo)
        {
            UIMainMenuManager.Instance.OpenLoadingUI();
            _runner ??= gameObject.AddComponent<NetworkRunner>();
            var clientTask = CreateRoom(sessionInfo,_sceneName);
            yield return new WaitUntil(predicate: () => clientTask.IsCompleted);
            Debug.Log("Final");
        }


        private IEnumerator WaitForJoinLobby()
        {
            UIMainMenuManager.Instance.OpenLoadingUI();
            _runner ??= gameObject.AddComponent<NetworkRunner>();
            Debug.Log($"{_runner.gameObject.name}");
            var clientTask = JoinLobby();
            yield return new WaitUntil(predicate: () => clientTask.IsCompleted);
            UIMainMenuManager.Instance.OpenJoinLobbyUI();
        }
        private IEnumerator WaitForJoinRoom(SessionInfo info)
        {
            UIMainMenuManager.Instance.OpenLoadingUI();
            _runner ??= gameObject.AddComponent<NetworkRunner>();
            var clientTask = JoinRoom(info);
            yield return new WaitUntil(predicate: () => clientTask.IsCompleted);
        }

        private async Task JoinLobby()
        {
            var result = await _runner.JoinSessionLobby(SessionLobby.Custom, _lobbyName);
            if (result.Ok)
            {
                Debug.Log("Joined Lobby");
            }
            else
            {
                Debug.Log($"Unable to join lobby {_lobbyName}");
            }
        }

        private async Task CreateRoom(string sessionName,string sceneName)
        {
            _runner.ProvideInput = true;
            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Host,
                CustomLobbyName = _lobbyName,
                SessionName = sessionName,
                PlayerCount = 2,
                Scene = SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"),
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        private async Task JoinRoom(SessionInfo sessionInfo)
        {
            _runner.ProvideInput = true;
            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                CustomLobbyName = _lobbyName,
                SessionName = sessionInfo.Name,
                PlayerCount = 2,
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        
        


        #endregion

        #region Public Methods

        public void OnJoinLobby()
        {
            StartCoroutine(WaitForJoinLobby());
        }

        public void OnCreateRoom(string sessionName)
        {
            StartCoroutine(WaitForCreatedRoom(sessionName));
        }

        public void OnJoinRoom(SessionInfo info)
        {
            StartCoroutine(WaitForJoinRoom(info));
        }

        public void OnDisconnect()
        {
            _runner.Shutdown();
            SceneManager.LoadScene(0);
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
            Debug.Log($"Shut down {runner.LocalPlayer.PlayerId}");
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log($"Player connected to server ");
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            Debug.Log($"Player connected to server ");
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
            UIMainMenuManager.Instance.CleanAllSessionInfoContainers();
            if (sessionList.Count == 0)
            {
                Debug.Log("Joined Lobby, no sessions founds");
            }
            else
            {
                foreach (SessionInfo session in sessionList)
                {
                    UIMainMenuManager.Instance.SpawnSessionInfoUIContainer(session);
                    Debug.Log($"Founded session {session.Name}, {session.PlayerCount}/{session.MaxPlayers}");
                }
            }
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