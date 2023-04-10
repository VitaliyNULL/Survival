using UnityEngine;

namespace VitaliyNULL.Supply
{
    public class AmmoSupply: NetworkSupply
    {
        protected override void Pick()
        {
            Debug.Log("Added ammo: ..  for player with id: ..");
        }
    }
}