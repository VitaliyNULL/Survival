using UnityEngine;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.Supply
{
    public class HealSupply: NetworkSupply
    {
        protected override void Pick(PlayerController playerController)
        {
            Debug.Log("Healed player with id : ..  by .. points");
            playerController.AddHealth();
        }
    }
}