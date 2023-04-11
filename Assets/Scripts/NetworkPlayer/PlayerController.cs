using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.GameSceneUI;
using VitaliyNULL.NetworkWeapon;
using VitaliyNULL.StateMachine;
using VitaliyNULL.Supply;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft, IDamageable
    {
        #region Private Fields

        [SerializeField] private StateMachine.StateMachine stateMachine;
        [SerializeField] private WeaponController weaponController;
        private CinemachineVirtualCamera _camera;
        private LeaderBoard _leaderBoard;
        private readonly string _nameKey = "USERNAME";
        private string _username;
        private GameUI _gameUI;
        private Vector3 _deathPosition;
        private readonly int _maxHealth = 15;
        private int _currentHealth;
        private int _damageCount = 0;
        private int _kills = 0;

        #endregion

        #region Public Fields

        [HideInInspector] public bool isDead = false;

        #endregion

        #region Public Methods

        public static PlayerController FindPlayer(PlayerRef playerRef)
        {
            var playerController = FindObjectsOfType<PlayerController>();
            foreach (var controller in playerController)
            {
                if (controller.Object.InputAuthority.PlayerId.Equals(Mathf.Abs(playerRef.PlayerId)))
                {
                    return controller;
                }
            }

            return null;
        }

        #endregion

        #region Private Properties

        private int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                RPC_TakeHealthUpdate(_currentHealth, _maxHealth);
                if (_currentHealth == 0)
                {
                    RPC_Death();
                }
            }
        }

        private int Kills
        {
            get => _kills;
            set
            {
                _kills = value;
                if (HasInputAuthority)
                {
                    _gameUI.SetKillsUI(_kills);
                    RPC_ChangeKills(_kills);
                }

                RPC_ChangeKillsRemotePlayer();
            }
        }

        #endregion

        #region Public Properties

        public GameUI GameUI => _gameUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Supply"))
            {
                col.GetComponent<NetworkSupply>().PickUp(this);
            }
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            _currentHealth = _maxHealth;
            if (HasInputAuthority)
            {
                _camera = FindObjectOfType<CinemachineVirtualCamera>();
                _camera.Follow = transform;
                _gameUI = FindObjectOfType<GameUI>();
                _gameUI.SetHpUI(_currentHealth, _maxHealth);
                _gameUI.SetKillsUI(_kills);
                _username = PlayerPrefs.GetString(_nameKey);
                RPC_ChangeNickName(PlayerPrefs.GetString(_nameKey));
                Debug.Log("spawned local player");
                // _gameUI.SetAmmoUI(weaponController.currentGun.CurrentAmmo, weaponController.currentGun.AllAmmo);
            }
            _leaderBoard = FindObjectOfType<LeaderBoard>();
            RPC_ChangeNickNameRemotePlayer();
        }

        private void FixedUpdate()
        {
            if (isDead)
            {
                transform.position = _deathPosition;
            }
        }

        #endregion

        #region Public Methods

        public void SetDamage()
        {
            if (HasInputAuthority)
            {
                _damageCount += weaponController.currentGun.Damage;
                RPC_ChangeDamage(_damageCount);
            }

            RPC_ChangeDamageRemotePlayer();
        }

        public void SetKill()
        {
            Kills++;
        }

        public void AddHealth()
        {
            Health += 4;
        }

        public void AddAmmo()
        {
            weaponController.currentGun.AddAmmo();
        }

        #endregion

        #region IPlayerLeft

        public void PlayerLeft(PlayerRef player)
        {
            if (HasInputAuthority) return;
            Runner.Despawn(Object);
            Debug.Log("Despawn Object");
        }

        #endregion

        #region IDamageable

        public void TakeDamage(int damage, PlayerRef playerRef)
        {
            Health -= damage;
            Debug.Log($"Player health is {Health}");
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_TakeHealthUpdate(int currentHealth, int maxHealth)
        {
            if (HasInputAuthority)
            {
                _gameUI.SetHpUI(currentHealth, maxHealth);
            }
        }

        [Rpc]
        void RPC_TakeDeathRemote()
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                stateMachine.SwitchState<DeadState>();
            }
        }
        [Rpc]
        private void RPC_Death()
        {
            if (HasStateAuthority && HasInputAuthority)
            {
                RPC_TakeDeathRemote();
            }
            stateMachine.SwitchState<DeadState>();
            isDead = true;
            _deathPosition = transform.position;
            gameObject.layer = 0;
            tag = "Untagged";
            weaponController.gameObject.SetActive(false);
            Destroy(GetComponentInChildren<Collider2D>());
            Destroy(GetComponent<NetworkRigidbody2D>());
            Destroy(GetComponent<Rigidbody2D>());

            var players = Runner.ActivePlayers;
            foreach (var player in players)
            {
                if (player != Runner.LocalPlayer)
                {
                    _camera.Follow = FindPlayer(player).transform;
                    break;
                }
            }
            Debug.Log("Game Over");
            
        }

        // [Rpc]
        public void SpawnLeaderboardContainer()
        {
            Debug.LogError("SpawnLeaderboardContainer in player with id: " + Runner.LocalPlayer.PlayerId);
            _leaderBoard.SpawnContainer(_username, _damageCount, _kills);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeNickName(string username, RpcInfo info = default)
        {
            Debug.Log($"[RPC_ChangeSkin] {info.Source.PlayerId} called RPC");
            _username = username;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeNickNameRemotePlayer(RpcInfo info = default)
        {
            if (HasInputAuthority && HasStateAuthority)
            {
                Debug.Log($"[RPC_ChangeSkinRemotePlayer] {info.Source.PlayerId} called RPC");
                _username = PlayerPrefs.GetString(_nameKey);
                RPC_TakeChangeNickNameRpc(PlayerPrefs.GetString(_nameKey));
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_TakeChangeNickNameRpc(string username, RpcInfo info = default)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                _username = username;
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeDamage(int damage, RpcInfo info = default)
        {
            Debug.Log($"[RPC_ChangeSkin] {info.Source.PlayerId} called RPC");
            _damageCount = damage;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeDamageRemotePlayer(RpcInfo info = default)
        {
            if (HasInputAuthority && HasStateAuthority)
            {
                Debug.Log($"[RPC_ChangeSkinRemotePlayer] {info.Source.PlayerId} called RPC");
                RPC_TakeChangeDamageRPC(_damageCount);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_TakeChangeDamageRPC(int damage, RpcInfo info = default)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                _damageCount = damage;
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeKills(int kills, RpcInfo info = default)
        {
            Debug.Log($"[RPC_ChangeSkin] {info.Source.PlayerId} called RPC");
            _kills = kills;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeKillsRemotePlayer(RpcInfo info = default)
        {
            if (HasInputAuthority && HasStateAuthority)
            {
                Debug.Log($"[RPC_ChangeSkinRemotePlayer] {info.Source.PlayerId} called RPC");
                RPC_TakeChangeKillsRPC(_kills);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_TakeChangeKillsRPC(int kills, RpcInfo info = default)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                _kills = kills;
            }
        }

        #endregion
    }
}