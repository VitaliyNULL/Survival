using Fusion;
using VitaliyNULL.Core;

namespace VitaliyNULL.Supply
{
    public abstract class NetworkSupply: NetworkBehaviour, IPickUpAble
    {
        
        public void PickUp()
        {
            Pick();
        }
        protected abstract void Pick();
    }
}