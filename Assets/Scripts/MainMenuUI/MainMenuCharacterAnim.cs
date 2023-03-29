using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.MainMenuUI
{
    public class MainMenuCharacterAnim : MonoBehaviour
    {
        public static MainMenuCharacterAnim Instance;
        private readonly string _mySkin = "MY_SKIN";

        private Animator _mainAnimator;

        private void Start()
        {
            _mainAnimator = GetComponent<Animator>();
            Instance = this;
            if (PlayerPrefs.HasKey(_mySkin))
            {
                PlayerSkinChange((PlayerSkin)PlayerPrefs.GetInt(_mySkin));
            }
            else _mainAnimator.CrossFade("farmer0",0);
        }

        public void PlayerSkinChange(PlayerSkin value)
        {
            switch (value)
            {
                case PlayerSkin.Farmer0:
                    _mainAnimator.CrossFade("farmer0",0);
                    break;
                case PlayerSkin.Farmer1:
                    _mainAnimator.CrossFade("farmer1",0);
                    break;
                case PlayerSkin.Farmer2:
                    _mainAnimator.CrossFade("farmer2",0);
                    break;
                case PlayerSkin.Farmer3:
                    _mainAnimator.CrossFade("farmer3",0);
                    break;
            }
            Debug.Log($"Current skin is {value}");
        }
    }
}