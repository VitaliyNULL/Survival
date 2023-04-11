using System;
using TMPro;
using UnityEngine;

namespace VitaliyNULL.GameSceneUI
{
    public class GameUIManager : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text killsText;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private Transform leaderBoardContent;
        [SerializeField] private LeaderBoardUIContainer leaderBoardUIContainer;
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject joystickUI;
        [SerializeField] private GameObject gameUI;
        [SerializeField] private GameObject disconnectButton;

        #endregion

        #region Public Methods

        public void ActivateDisconnectButton()
        {
            disconnectButton.SetActive(true);
        }
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

        public void SpawnContainer(string name, int damage, int kills)
        {
            Debug.LogError("Spawning Container");
            background.SetActive(true);
            joystickUI.SetActive(false);
            gameUI.SetActive(false);
            disconnectButton.SetActive(true);
            LeaderBoardUIContainer container = Instantiate(leaderBoardUIContainer, leaderBoardContent);
            container.Initialize(name, damage, kills);
        }

        public void SpawnContainer(string name, int damage, int kills, out GameObject disconnectButton)
        {
            Debug.LogError("Spawning Container");
            background.SetActive(true);
            joystickUI.SetActive(false);
            gameUI.SetActive(false);
            this.disconnectButton.SetActive(true);
            LeaderBoardUIContainer container = Instantiate(leaderBoardUIContainer, leaderBoardContent);
            container.Initialize(name, damage, kills);
            disconnectButton = this.disconnectButton;
        }

        #endregion
    }
}