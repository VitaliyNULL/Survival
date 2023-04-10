using UnityEngine;

namespace VitaliyNULL.Supply
{
    public class HealSupply: NetworkSupply
    {
        protected override void Pick()
        {
            Debug.Log("Healed player with id : ..  by .. points");
        }
    }
}