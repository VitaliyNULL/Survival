using UnityEngine;
using UnityEngine.EventSystems;
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
    public class ChoosePlayerSkin: MonoBehaviour
    {
        public PlayerSkin playerSkinId;
        private Button _button;
        
        
        public void GetPlayerSkin()
        {
            Debug.Log(playerSkinId);
        }
    }
}