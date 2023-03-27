using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Fusion;

namespace VitaliyNULL.UI
{
    public class DisconnectButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() => FusionManager.Instance.OnDisconnect()));
        }
    }
}