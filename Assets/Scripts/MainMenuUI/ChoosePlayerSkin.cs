using Fusion;
using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Core;

namespace VitaliyNULL.MainMenuUI
{
    public class ChoosePlayerSkin : MonoBehaviour
    {
        public PlayerSkin playerSkinId;
        private readonly string _mySkin = "MY_SKIN";
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(GetPlayerSkin);
        }

        public void GetPlayerSkin()
        {
            Debug.Log($"{playerSkinId} was chose");
            PlayerPrefs.SetInt(_mySkin, (int)playerSkinId);
        }
    }
}