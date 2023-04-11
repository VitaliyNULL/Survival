using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.Supply
{
    public abstract class NetworkSupply: NetworkBehaviour, IPickUpAble
    {
        [SerializeField] protected SupplyType supplyType;
        [SerializeField] protected AudioClip pickAudioClip;
        public void PickUp(PlayerController playerController)
        {
            Pick(playerController);
            AudioSource.PlayClipAtPoint(pickAudioClip,transform.position,1);
            Runner.Despawn(Object);
        }
        protected abstract void Pick(PlayerController playerController);
    }
}