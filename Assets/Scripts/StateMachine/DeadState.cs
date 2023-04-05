using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class DeadState: State
    {
        public DeadState(Animator animator) : base(animator)
        {
        }

        public override void Start()
        {
            Animator.CrossFade(AnimationsName.Dead,0f);
        }

        public override void Stop()
        {
            Animator.StopPlayback();
        }
    }
}