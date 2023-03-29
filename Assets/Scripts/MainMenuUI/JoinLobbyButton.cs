using System;
using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Fusion;

namespace VitaliyNULL.MainMenuUI
{
    public class JoinLobbyButton: MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() => FusionManager.Instance.OnJoinLobby()));
        }
    }
}