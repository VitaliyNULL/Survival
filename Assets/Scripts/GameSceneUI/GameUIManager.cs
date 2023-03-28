using UnityEngine;

namespace VitaliyNULL.GameSceneUI
{
    public class GameUIManager : MonoBehaviour
    {
        public static GameUIManager Instance;
        [SerializeField] private GameObject chooseAvatarUI;
        [SerializeField] private GameObject gameUI;

        private GameObject _currentUIObject;
        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
            _currentUIObject = chooseAvatarUI;
        }

        private void ChangeCurrentUI(GameObject obj)
        {
            if (_currentUIObject != null)
            {
                _currentUIObject.SetActive(false);
            }
            _currentUIObject = obj;
        }

        private void OpenCurrenUI()
        {
            _currentUIObject.SetActive(true);
        }
        public void OpenGameUI()
        {
            ChangeCurrentUI(gameUI);
            OpenCurrenUI();
        }
    }
}
