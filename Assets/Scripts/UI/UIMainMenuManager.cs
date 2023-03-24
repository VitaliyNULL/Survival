using TMPro;
using UnityEngine;

namespace VitaliyNULL.UI
{
    public class UIMainMenuManager : MonoBehaviour
    {
        public static UIMainMenuManager Instance;
        [SerializeField] private TMP_Text warningText;
        [SerializeField] private SessionInfoUIContainer prefabSessionInfoUIContainer;
        [SerializeField] private RectTransform lobbyContent;
        [SerializeField] private GameObject loadingUI;
        [SerializeField] private GameObject waitingForPlayerUI;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject lobbyUI;
        [SerializeField] private GameObject roomUI;
        private void Start()
        {
            Instance ??= this;
        }

        public void ChangeWarningText(string message)
        {
            warningText.text = message;
        }

        public void CleanWarningText()
        {
            warningText.text = "";
        }

        public void SpawnSessionInfoUIContainer()
        {
            lobbyContent.sizeDelta = new Vector2(lobbyContent.sizeDelta.x, lobbyContent.sizeDelta.y + 20);
            lobbyContent.anchoredPosition =
                new Vector2(lobbyContent.anchoredPosition.x, lobbyContent.anchoredPosition.y - 10);
            Instantiate(prefabSessionInfoUIContainer, lobbyContent);
        }

        public void CleanAllSessionInfoContainers()
        {
            foreach (Transform child in lobbyContent)
            {
                Destroy(child.gameObject);
            }

            lobbyContent.sizeDelta = new Vector2(lobbyContent.sizeDelta.x, 0);
        }
    }
}