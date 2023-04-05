using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class StandState: State
    {
        public StandState(Animator animator) : base(animator)
        {
        }

        public override void Start()
        {
            Animator.CrossFade(AnimationsName.Stand,0f);
        }

        public override void Stop()
        {
            Animator.StopPlayback();
        }
    }
}