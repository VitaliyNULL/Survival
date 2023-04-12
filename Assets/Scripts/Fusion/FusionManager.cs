using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using VitaliyNULL.MainMenuUI;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.Fusion
{
    public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        #region Private Fields

        [HideInInspector] public NetworkRunner runner;
        private SessionInfo _sessionInfo;
        private readonly string _sceneName = "GameScene";
        private PlayerRef _player;
        private readonly string _nameKey = "USERNAME";
        private readonly string _lobbyName = "MainLobby";
        private readonly string _mySkin = "MY_SKIN";
        public NetworkObject playerController;
        // private NetworkObject _playerInput;

        #endregion

        #region Public Fields

        public static FusionManager Instance;
        public Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
            playerController = Resources.Load<NetworkObject>("PlayerController");
        }

        #endregion

        #region Private Methods

        private IEnumerator WaitForCreatedRoom(string sessionInfo)
        {
            UIMainMenuManager.Instance.OpenLoadingUI();
            runner ??= gameObject.AddComponent<NetworkRunner>();
            var clientTask = CreateRoom(sessionInfo, _sceneName);
            yield return new WaitUntil(predicate: () => clientTask.IsCompleted);
            Debug.Log("Final");
        }

        private IEnumerator WaitForJoinLobby()
        {
            UIMainMenuManager.Instance.OpenLoadingUI();
            runner ??= gameObject.AddComponent<NetworkRunner>();
            Debug.Log($"{runner.gameObject.name}");
            var clientTask = JoinLobby();
            yield return new WaitUntil(predicate: () => clientTask.IsCompleted);
            UIMainMenuManager.Instance.OpenJoinLobbyUI();
        }

        private IEnumerator WaitForJoinRoom(SessionInfo info)
        {
            UIMainMenuManager.Instance.OpenLoadingUI();
            runner ??= gameObject.AddComponent<NetworkRunner>();
            var clientTask = JoinRoom(info);
            yield return new WaitUntil(predicate: () => clientTask.IsCompleted);
        }

        private async Task JoinLobby()
        {
            var result = await runner.JoinSessionLobby(SessionLobby.Custom, _lobbyName);
            if (result.Ok)
            {
                Debug.Log("Joined Lobby");
            }
            else
            {
                Debug.Log($"Unable to join lobby {_lobbyName}");
            }
        }

        private async Task CreateRoom(string sessionName, string sceneName)
        {
            runner.ProvideInput = true;
            // Start or join (depends on gamemode) a session with a specific name
            await runner.StartGame(new StartGameArgs()
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
            runner.ProvideInput = true;
            // Start or join (depends on gamemode) a session with a specific name
            await runner.StartGame(new StartGameArgs()
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
            if (PlayerPrefs.GetString(_nameKey).Length > 0)
            {
                StartCoroutine(WaitForJoinLobby());
            }
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
            runner.Shutdown();
            SceneManager.LoadScene(0);
            Debug.Log("Disconnected");
        }

        #endregion

        #region INetworkRunnerCallbacks

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player with id: {player.PlayerId} joined the room ");
            if (runner.IsServer)
            {
                // Create a unique position for the player
                Vector3 spawnPosition =
                    new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
                NetworkObject playerController =
                    runner.Spawn(this.playerController, spawnPosition, Quaternion.identity, player);

                runner.Spawn(Resources.Load<NetworkObject>("WaveManager"), transform.position,
                    Quaternion.identity, player);

                // Keep track of the player avatars so we can remove it when they disconnect
                spawnedCharacters.Add(player, playerController);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player with id: {player.PlayerId} left the room ");
            spawnedCharacters.Remove(player);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }


        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            // Can check if the Runner is being shutdown because of the Host Migration
            if (shutdownReason == ShutdownReason.HostMigration)
            {
                // ...
                Debug.Log("Closed by HostMigration");
            }
            else
            {
                // Or a normal Shutdown
                Debug.Log("Closed by Hui zna sho");
            }
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

        public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            // Debug.Log("OnHostMigration");
            // await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);
            // // Step 2.2
            // // Create a new Runner.
            // var newRunner = Instantiate(new GameObject("FusionManager").AddComponent<NetworkRunner>());
            // FusionManager fusionManager = newRunner.gameObject.AddComponent<FusionManager>();
            // fusionManager.playerController = Resources.Load<NetworkObject>("PlayerController");
            // Debug.Log(fusionManager.playerController);
            // // setup the new runner...
            // // Start the new Runner using the "HostMigrationToken" and pass a callback ref in "HostMigrationResume".
            // StartGameResult result = await newRunner.StartGame(new StartGameArgs()
            // {
            //     // SessionName = SessionName,              // ignored, peer never disconnects from the Photon Cloud
            //     // GameMode = gameMode,                    // ignored, Game Mode comes with the HostMigrationToken
            //     HostMigrationToken = hostMigrationToken, // contains all necessary info to restart the Runner
            //     HostMigrationResume = HostMigrationResume, // this will be invoked to resume the simulation
            //     PlayerCount = 2,
            //     SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            //     // other args
            // });
            //
            // // Check StartGameResult as usual
            // if (result.Ok == false)
            // {
            //     Debug.LogWarning(result.ShutdownReason);
            // }
            // else
            // {
            //     Debug.Log("Done");
            // }
            // // newRunner.gameObject.AddComponent<FusionManager>();
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

        #region HostMigration

        // Step 3.
        // Resume Simulation on the new Runner
        void HostMigrationResume(NetworkRunner runner)
        {
            Debug.Log("HostMigrationResume");
            // Get a temporary reference for each NO from the old Host
            // foreach (var resumeNO in runner.GetResumeSnapshotNetworkObjects())
            // {
            //     if (resumeNO.TryGetBehaviour<NetworkPositionRotation>(out var posRot))
            //     {
            //         runner.Spawn(resumeNO, position: posRot.ReadPosition(), rotation: posRot.ReadRotation(),
            //             onBeforeSpawned: (runner, newNO) =>
            //             {
            //                 // One key aspects of the Host Migration is to have a simple way of restoring the old NetworkObjects state
            //                 // If all state of the old NetworkObject is all what is necessary, just call the NetworkObject.CopyStateFrom
            //                 newNO.CopyStateFrom(resumeNO);
            //
            //                 // and/or
            //
            //                 // If only partial State is necessary, it is possible to copy it only from specific NetworkBehaviours
            //                 if (resumeNO.TryGetBehaviour<NetworkBehaviour>(out var myCustomNetworkBehaviour))
            //                 {
            //                     newNO.GetComponent<NetworkBehaviour>().CopyStateFrom(myCustomNetworkBehaviour);
            //                 }
            //             });
            //     }
            // }
        }

        #endregion
    }
}