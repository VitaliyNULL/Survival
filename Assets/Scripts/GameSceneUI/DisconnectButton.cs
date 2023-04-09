using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Fusion;

namespace VitaliyNULL.GameSceneUI
{
    public class DisconnectButton : MonoBehaviour
    {
        #region Private Methods

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() => FusionManager.Instance.OnDisconnect()));
        }

        #endregion
    }
}