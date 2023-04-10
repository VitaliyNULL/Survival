using UnityEngine;

namespace VitaliyNULL.Supply
{
    public class BombSupply : NetworkSupply
    {
        protected override void Pick()
        {
            Debug.Log("Kill all enemies in radius");
        }
    }
}