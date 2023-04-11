using System.Collections.Generic;
using UnityEngine;

namespace VitaliyNULL.Factory
{
    public class SpawnPoints : MonoBehaviour
    {
        #region Public Fields
        public List<Transform> spawnPoints;

        #endregion

        #region Public Methods

        public Transform GetRandomPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
        }

        #endregion
    }
}