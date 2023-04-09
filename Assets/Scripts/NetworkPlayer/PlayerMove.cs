using Fusion;
using UnityEngine;
using VitaliyNULL.StateMachine;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerMove : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private StateMachine.StateMachine stateMachine;
        private float _speed = 5f;
        private NetworkRigidbody2D _networkRigidbody2D;
        [SerializeField] private PlayerController playerController;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _networkRigidbody2D ??= GetComponent<NetworkRigidbody2D>();
            playerController ??= GetComponentInParent<PlayerController>();
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            _networkRigidbody2D ??= GetComponent<NetworkRigidbody2D>();
            playerController ??= GetComponentInParent<PlayerController>();
        }

        public override void FixedUpdateNetwork()
        {
            if (playerController.isDead) return;
            if (GetInput(out NetworkInputData data))
            {
                data.directionToMove.Normalize();
                if (data.directionToMove == Vector3.zero)
                {
                    stateMachine.SwitchState<StandState>();
                }
                else
                {
                    stateMachine.SwitchState<RunState>();
                }

                Vector2 toMove = _networkRigidbody2D.Rigidbody.position;
                toMove.x += _speed * data.directionToMove.x * Runner.DeltaTime;
                toMove.y += _speed * data.directionToMove.y * Runner.DeltaTime;
                _networkRigidbody2D.Rigidbody.MovePosition(toMove);
                float flip = 0;
                if (data.directionToMove.x < 0 && data.directionToShoot.x > 0)
                {
                    flip = 0;
                }
                else if (data.directionToMove.x < 0 || data.directionToShoot.x < 0)
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