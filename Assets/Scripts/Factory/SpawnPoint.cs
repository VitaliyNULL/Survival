using UnityEngine;

namespace VitaliyNULL.Factory
{
    public class SpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position,0.3f);
        }
    }
}