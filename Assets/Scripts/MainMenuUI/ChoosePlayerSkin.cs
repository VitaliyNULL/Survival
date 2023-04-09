using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Core;

namespace VitaliyNULL.MainMenuUI
{
    public class ChoosePlayerSkin : MonoBehaviour
    {
        #region Private Fields

        private readonly string _mySkin = "MY_SKIN";
        private Button _button;

        #endregion

        #region Public Fields

        public PlayerSkin playerSkinId;

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(GetPlayerSkin);
        }

        #endregion

        #region Public Methods

        public void GetPlayerSkin()
        {
            Debug.Log($"{playerSkinId} was chose");
            PlayerPrefs.SetInt(_mySkin, (int)playerSkinId);
        }

        #endregion
    }
}