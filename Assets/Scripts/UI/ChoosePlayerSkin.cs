using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.UI
{
    public enum PlayerSkin
    {
        Farmer0,
        Farmer1,
        Farmer2,
        Farmer3,
    }

    public class ChoosePlayerSkin : MonoBehaviour
    {
        public PlayerSkin playerSkinId;
        private Button _button;
        [Networked] public NetworkBool Waiting { get; set; }

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(GetPlayerSkin);
        }

        public void GetPlayerSkin()
        {
            Debug.Log($"{playerSkinId} was chose");
            
        }
    }
}