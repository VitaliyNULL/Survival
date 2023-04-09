using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Fusion;

namespace VitaliyNULL.MainMenuUI
{
    public class JoinLobbyButton: MonoBehaviour
    {
        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() => FusionManager.Instance.OnJoinLobby()));
        }

        #endregion
    }
}