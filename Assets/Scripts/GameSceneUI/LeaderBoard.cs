using UnityEngine;

namespace VitaliyNULL.GameSceneUI
{
    public class LeaderBoard : MonoBehaviour
    {
        
        [SerializeField] private Transform leaderBoardContent;
        [SerializeField] private LeaderBoardUIContainer leaderBoardUIContainer;
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject joystickUI;
        [SerializeField] private GameObject gameUI;
        
        public void SpawnContainer(string name, int damage, int kills)
        {
            background.SetActive(true);
            LeaderBoardUIContainer container = Instantiate(leaderBoardUIContainer, leaderBoardContent);
            container.Initialize(name,damage,kills);
        }

    }
}