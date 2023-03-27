using Fusion;
using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Core;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.UI
{
    public class ChoosePlayerSkin : MonoBehaviour
    {
        public PlayerSkin playerSkinId;
        private Button _button;
        [Networked] public NetworkBool Waiting { get; set; }

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(GetPlayerSkin);
        }

        public void GetPlayerSkin()
        {
            Debug.Log($"{playerSkinId} was chose");
            NetworkSpawner.Instance.SpawnPlayerManager(playerSkinId);
        }
    }
}