using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkPlayer
{
    public class NetworkSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        public static NetworkSpawner Instance;


        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }

        public void SpawnPlayerManager(PlayerSkin playerSkin)
        {
            PlayerManager pM = Instantiate(playerManager);
            pM.SetPlayerSkin(playerSkin);
            
        }
    }
}