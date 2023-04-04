using Fusion;
using UnityEngine;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerMove : NetworkBehaviour
    {
        private float _speed = 5f;
        private NetworkRigidbody2D _networkRigidbody2D;

        private void Awake()
        {
            _networkRigidbody2D ??= GetComponent<NetworkRigidbody2D>();
        }

        public override void Spawned()
        {
            _networkRigidbody2D ??= GetComponent<NetworkRigidbody2D>();
        }

        #region NetworkBehaviour Callbacks

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.directionToMove.Normalize();
                Vector2 toMove = _networkRigidbody2D.Rigidbody.position;
                toMove.x += _speed * data.directionToMove.x * Runner.DeltaTime;
                toMove.y += _speed * data.directionToMove.y * Runner.DeltaTime;

                _networkRigidbody2D.Rigidbody.MovePosition(toMove);
                float flip = 0;
                if (data.directionToMove.x < 0 || data.directionToShoot.x < 0)
                {
                    flip = 180;
                }

                transform.rotation =
                    new Quaternion(transform.rotation.x, flip, transform.rotation.z, transform.rotation.w);
            }
        }

        #endregion
    }
}