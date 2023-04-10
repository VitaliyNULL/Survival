using UnityEngine;

namespace VitaliyNULL.Factory
{
    public class SpawnPoint : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private Color _color = Color.red;
        #endregion
        #region MonoBehaviour CallBacks

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position,0.3f);
        }

        #endregion
    }
}