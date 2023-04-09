using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class StandState: State
    {
        #region Constructor

        public StandState(Animator animator) : base(animator)
        {
        }

        #endregion

        #region State Methods

        public override void Start()
        {
            Animator.CrossFade(AnimationsName.Stand,0f);
        }

        public override void Stop()
        {
            Animator.StopPlayback();
        }

        #endregion
    }
}