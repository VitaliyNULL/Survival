using TMPro;
using UnityEngine;

namespace VitaliyNULL.UI
{
    public class InputUserName : MonoBehaviour
    {
        private string _username;
        private TMP_InputField _inputField;

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        public void OnChangeInput()
        {
            if (_username.Length >= 20)
            {
                _inputField.text = _username;
                
            }
            else
            {
                _username = _inputField.text;
            }
        }
    }
}
