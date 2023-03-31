using System;
using Fusion;
using UnityEngine;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerMove : NetworkBehaviour
    {
        private float _speed = 5f;
        public override void Spawned()
        {
        }

        #region NetworkBehaviour Callbacks

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.directionToMove.Normalize();

                if (transform.position.y <= -7f)
                {
                    if (data.directionToMove.y <= 0)
                    {
                        data.directionToMove = new Vector2(transform.position.x, data.directionToMove.y);
                    }
                }
                else if (transform.position.y >= 5f)
                {
                    if (data.directionToMove.y >= 0)
                    {
                        data.directionToMove = new Vector2(transform.position.x, data.directionToMove.y);
                    }
                }

                if (transform.position.x <= -13f)
                {
                    if (data.directionToMove.x <= 0)
                    {
                        data.directionToMove = new Vector2(data.directionToMove.x, transform.position.y);
                    }
                }
                else if(transform.position.x >=13f)
                {
                    if (data.directionToMove.x >= 0)
                    {
                        data.directionToMove = new Vector2(data.directionToMove.x, transform.position.y);
                    }
                }
                float flip = 0;
                if (data.directionToMove.x < 0 || data.directionToShoot.x < 0)
                {
                    flip = 180;
                }
                transform.rotation =
                    new Quaternion(transform.rotation.x, flip, transform.rotation.z, transform.rotation.w);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + data.directionToMove,
                    _speed * Runner.DeltaTime);
            }
        }

        #endregion
        
    }
}