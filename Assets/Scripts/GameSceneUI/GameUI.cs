using System;
using TMPro;
using UnityEngine;

namespace VitaliyNULL.GameSceneUI
{
    public class GameUI: MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text killsText;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text ammoText;

        #endregion


        #region Public Methods

        public void SetKillsUI(int val)
        {
            killsText.text = String.Format($"Kills {val}");
        }

        public void SetHpUI(int currentHp, int maxHp)
        {
            hpText.text = String.Format($"HP {currentHp}/{maxHp}");
        }

        public void SetAmmoUI(int currentAmmo, int allAmmo)
        {
            ammoText.text = String.Format($"Ammo {currentAmmo}/{allAmmo}");
        }

        #endregion
    }
}