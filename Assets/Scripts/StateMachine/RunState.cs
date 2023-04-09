using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class RunState : State
    {
        #region Constructor

        public RunState(Animator animator) : base(animator)
        {
        }

        #endregion

        #region State Methods

        public override void Start()
        {
            Animator.CrossFade(AnimationsName.Run, 0f);
        }

        public override void Stop()
        {
            Animator.StopPlayback();
        }

        #endregion
    }
}