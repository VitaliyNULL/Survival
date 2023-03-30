using System.Collections.Generic;
using Fusion;
using VitaliyNULL.Fusion;

namespace VitaliyNULL.NetworkWeapon
{
    public class WeaponController: NetworkBehaviour
    {
        private List<NetworkGun> _guns = new List<NetworkGun>();

        [Rpc]
        public void RPC_ChooseGun()
        {
            //TODO: SPAWN GUN
        }

        public void ChooseRandomGun()
        {
            //TODO: Choose random gun 
        }
    }
}