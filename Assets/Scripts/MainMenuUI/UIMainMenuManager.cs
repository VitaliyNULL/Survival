using Fusion;
using TMPro;
using UnityEngine;
using VitaliyNULL.Fusion;

namespace VitaliyNULL.MainMenuUI
{
    public class UIMainMenuManager : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text warningText;
        [SerializeField] private SessionInfoUIContainer prefabSessionInfoUIContainer;
        [SerializeField] private RectTransform lobbyContent;
        [SerializeField] private GameObject loadingUI;
        [SerializeField] private GameObject waitingForPlayerUI;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject lobbyUI;
        [SerializeField] private GameObject createRoomUI;
        [SerializeField] private GameObject warningUI;
        private GameObject _currentUIObject;
        private readonly string _gameSceneName = "GameScene";
        private string _sessionName;

        #endregion

        #region Public Fields

        public static UIMainMenuManager Instance;

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
            _currentUIObject = mainMenuUI;
        }

        #endregion
        
        #region Public Methods

        public void OpenCreateRoomUI()
        {
            ChangeCurrentUIObject(createRoomUI);
            OpenCurrentUIObject();
        }
        public void OpenMainMenuUI()
        {
            ChangeCurrentUIObject(mainMenuUI);
            OpenCurrentUIObject();
        }

        public void OpenJoinLobbyUI()
        {
            ChangeCurrentUIObject(lobbyUI);
            OpenCurrentUIObject();
        }

        public void OnChangeSessionName(string str)
        {
            _sessionName = str;
        }

        public void ChangeWarningText(string message)
        {
            warningUI.SetActive(true);
            warningText.text = message;
        }

        public void CleanWarningText()
        {
            warningText.text = "";
            warningUI.SetActive(false);
        }

        public void SpawnSessionInfoUIContainer(SessionInfo sessionInfo)
        {
            lobbyContent.sizeDelta = new Vector2(lobbyContent.sizeDelta.x, lobbyContent.sizeDelta.y + 20);
            lobbyContent.anchoredPosition =
                new Vector2(lobbyContent.anchoredPosition.x, lobbyContent.anchoredPosition.y - 10);
            SessionInfoUIContainer sessionInfoUIContainer = Instantiate(prefabSessionInfoUIContainer, lobbyContent);
            sessionInfoUIContainer.SetInfo(sessionInfo);
            sessionInfoUIContainer.OnJoinSession += SessionInfoUIContainerOnJoinSession;
        }


        public void CreateNewGameSession()
        {
            FusionManager.Instance.OnCreateRoom(_sessionName);
            Debug.Log(_sessionName);
        }

        public void OpenWaitingUI()
        {
            ChangeCurrentUIObject(waitingForPlayerUI);
            OpenCurrentUIObject();
        }

        public void OpenLoadingUI()
        {
            ChangeCurrentUIObject(loadingUI);
            OpenCurrentUIObject();
        }


        public void CleanAllSessionInfoContainers()
        {
            foreach (Transform child in lobbyContent)
            {
                Destroy(child.gameObject);
            }

            lobbyContent.sizeDelta = new Vector2(lobbyContent.sizeDelta.x, 0);
        }

        #endregion

        #region Private Methods

        private void ChangeCurrentUIObject(GameObject obj)
        {
            if (_currentUIObject != null)
            {
                _currentUIObject.SetActive(false);
            }

            _currentUIObject = obj;
        }

        private void OpenCurrentUIObject()
        {
            _currentUIObject.SetActive(true);
        }

        private void SessionInfoUIContainerOnJoinSession(SessionInfo info)
        {
            FusionManager.Instance.OnJoinRoom(info);
        }

        #endregion
    }
}