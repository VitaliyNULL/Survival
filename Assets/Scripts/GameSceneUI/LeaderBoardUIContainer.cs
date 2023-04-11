using TMPro;
using UnityEngine;

namespace VitaliyNULL.GameSceneUI
{
    public class LeaderBoardUIContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text killCountText;

        public void Initialize(string username, int damage, int killCount)
        {
            usernameText.text = username;
            damageText.text = damage.ToString();
            killCountText.text = killCount.ToString();
        }
    }
}