using TMPro;
using UnityEngine;

namespace VitaliyNULL.UI
{
    public class InputUserName : MonoBehaviour
    {
        private string _username = "";
        private TMP_InputField _inputField;

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        public void OnChangeInput()
        {
            if (_inputField.text.Length <= 12)
            {
                _username = _inputField.text;
                UIMainMenuManager.Instance.CleanWarningText();
            }
            else
            {
                _inputField.text = _username;
                UIMainMenuManager.Instance.ChangeWarningText("Username cannot be longer than 12 symbols");
            }
        }

        private void OnDisable()
        {
            UIMainMenuManager.Instance.CleanWarningText();
        }
    }
}