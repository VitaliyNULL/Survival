
using System.Collections.Generic;
using UnityEngine;

namespace VitaliyNULL.Factory
{
    public class SpawnPoints : MonoBehaviour
    {
        public List<Transform> spawnPoints;

        public Transform GetRandomPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count-1)];
        }
    }
}
