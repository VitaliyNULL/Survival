using Fusion;
using UnityEngine;
using VitaliyNULL.Fusion;
using UnityEngine.UI;

namespace VitaliyNULL.GameSceneUI
{
    public class StartGameButton : NetworkBehaviour
    {
        [SerializeField] private WaveManager waveManager;

        private void Start()
        {
            // GetComponent<Button>().onClick.AddListener((() => waveManager.StartGame()));
        }
    }
}