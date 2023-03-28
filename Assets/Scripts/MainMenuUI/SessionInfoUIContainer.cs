using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MainMenuUI
{
    public class SessionInfoUIContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text lobbyName;
        [SerializeField] private TMP_Text playerCount;
        private Button _joinRoom;
        private SessionInfo _sessionInfo;
        public event Action<SessionInfo> OnJoinSession;
        public void SetInfo(SessionInfo info)
        {
            _sessionInfo = info;
            lobbyName.text = _sessionInfo.Name;
            playerCount.text = String.Format($"{_sessionInfo.PlayerCount}/{_sessionInfo.MaxPlayers}");
            _joinRoom ??= GetComponent<Button>();
            _joinRoom.interactable = _sessionInfo.PlayerCount < _sessionInfo.MaxPlayers;
        }

        public void OnClick()
        {
            OnJoinSession?.Invoke(_sessionInfo);
        }

    }
}