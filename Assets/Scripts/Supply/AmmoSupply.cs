using UnityEngine;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.Supply
{
    public class AmmoSupply: NetworkSupply
    {
        protected override void Pick(PlayerController playerController)
        {
            Debug.Log("Added ammo: ..  for player with id: ..");
            playerController.AddAmmo();
        }
    }
}