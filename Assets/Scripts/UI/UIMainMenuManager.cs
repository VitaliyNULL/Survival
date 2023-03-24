using TMPro;
using UnityEngine;

namespace VitaliyNULL.UI
{
    public class UIMainMenuManager: MonoBehaviour
    {
        public static UIMainMenuManager Instance;
        [SerializeField] private TMP_Text warningText;
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
        
    }
}