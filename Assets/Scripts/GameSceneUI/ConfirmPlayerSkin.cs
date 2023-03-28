using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Core;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.GameSceneUI
{
    public class ConfirmPlayerSkin: MonoBehaviour
    {
        public static ConfirmPlayerSkin Instance;
        private PlayerSkin _skin;
        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
            
            GetComponent<Button>().onClick.AddListener(ConfirmPlayer);
        }

        public void SetPlayerSkin(PlayerSkin playerSkin)
        {
            _skin = playerSkin;
        }
        private void ConfirmPlayer()
        {
            if (_skin != null)
            {
                NetworkSpawner.Instance.SpawnPlayerManager(_skin);
                GameUIManager.Instance.OpenGameUI();
            }
        }
    }
}