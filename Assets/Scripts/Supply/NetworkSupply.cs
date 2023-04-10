using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.Supply
{
    public abstract class NetworkSupply: NetworkBehaviour, IPickUpAble
    {
        [SerializeField] protected SupplyType supplyType;
        public void PickUp(PlayerController playerController)
        {
            Pick(playerController);
            Runner.Despawn(Object);
        }
        protected abstract void Pick(PlayerController playerController);
    }
}