using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class HitState : State
    {
        #region Constructor

        public HitState(Animator animator) : base(animator)
        {
        }

        #endregion

        #region State Methods

        public override void Start()
        {
            Animator.CrossFade(AnimationsName.Hit, 0f);
        }

        public override void Stop()
        {
            Animator.StopPlayback();
        }

        #endregion
    }
}