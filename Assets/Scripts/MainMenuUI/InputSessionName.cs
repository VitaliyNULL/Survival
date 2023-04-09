using TMPro;
using UnityEngine;

namespace VitaliyNULL.MainMenuUI
{
    public class InputSessionName : MonoBehaviour
    {
        #region Private Fields

        private string _sessionName = "";
        private TMP_InputField _inputField;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        private void OnDisable()
        {
            _sessionName = "";
            _inputField.text = _sessionName;
        }

        #endregion

        #region Public Methods

        public void OnChangeInput()
        {
            if (_inputField.text.Length <= 20)
            {
                _sessionName = _inputField.text;
                UIMainMenuManager.Instance.CleanWarningText();
            }
            else
            {
                _inputField.text = _sessionName;
                UIMainMenuManager.Instance.ChangeWarningText("Room name cannot be longer than 20 symbols");
            }

            UIMainMenuManager.Instance.OnChangeSessionName(_sessionName);
        }

        #endregion
    }
}