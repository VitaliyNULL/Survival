using Fusion;
using UnityEngine;
using VitaliyNULL.Supply;

namespace VitaliyNULL.Factory
{
    public class NetworkSupplyFactory : NetworkBehaviour
    {
        [SerializeField] private NetworkSupply healSupply;
        [SerializeField] private NetworkSupply bombSupply;
        [SerializeField] private NetworkSupply ammoSupply;
        [SerializeField] private SpawnPoints spawnPoints;
        
        
    }
}