using TMPro;
using UnityEngine;

namespace VitaliyNULL.MainMenuUI
{
    public class InputUserName : MonoBehaviour
    {
        #region Private Fields

        private string _username = "";
        private TMP_InputField _inputField;
        private readonly string _nameKey = "USERNAME";

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onValueChanged.AddListener(OnChangeInput);
            if (PlayerPrefs.HasKey(_nameKey))
            {
                _username = PlayerPrefs.GetString(_nameKey);
                _inputField.text = _username;
            }
        }

        private void OnDisable()
        {
            UIMainMenuManager.Instance?.CleanWarningText();
        }

        #endregion

        #region Private Fields

        private void OnChangeInput(string val)
        {
            if (_inputField.text.Length <= 12)
            {
                _username = _inputField.text;
                UIMainMenuManager.Instance?.CleanWarningText();
            }
            else
            {
                _inputField.text = _username;
                UIMainMenuManager.Instance.ChangeWarningText("Username cannot be longer than 12 symbols");
            }

            PlayerPrefs.SetString(_nameKey, _username);
        }

        #endregion
    }
}