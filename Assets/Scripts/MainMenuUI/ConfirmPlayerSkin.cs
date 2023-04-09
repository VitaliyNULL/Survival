using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Core;

namespace VitaliyNULL.MainMenuUI
{
    public class ConfirmPlayerSkin : MonoBehaviour
    {
        #region Private Fields

        private PlayerSkin _skin;
        private readonly string _mySkin = "MY_SKIN";

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ConfirmPlayer);
        }

        #endregion

        #region Private Methods

        private void ConfirmPlayer()
        {
            UIMainMenuManager.Instance.OpenMainMenuUI();
            MainMenuCharacterAnim.Instance.PlayerSkinChange((PlayerSkin)PlayerPrefs.GetInt(_mySkin));
        }

        #endregion
    }
}