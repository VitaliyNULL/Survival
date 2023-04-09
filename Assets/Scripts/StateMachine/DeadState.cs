using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class DeadState: State
    {
        #region Constructor

        public DeadState(Animator animator) : base(animator)
        {
        }

        #endregion

        #region State Methods

        public override void Start()
        {
            Animator.CrossFade(AnimationsName.Dead,0f);
        }

        public override void Stop()
        {
            Animator.StopPlayback();
        }

        #endregion
    }
}