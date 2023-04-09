using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MainMenuUI
{
    public class SessionInfoUIContainer : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text lobbyName;
        [SerializeField] private TMP_Text playerCount;
        private Button _joinRoom;
        private SessionInfo _sessionInfo;

        #endregion

        #region Public Fields

        public event Action<SessionInfo> OnJoinSession;

        #endregion

        #region Public Methods

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

        #endregion
    }
}