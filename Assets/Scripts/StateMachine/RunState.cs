using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class RunState : State
    {
        public RunState(Animator animator) : base(animator)
        {
        }

        public override void Start()
        {
            Animator.CrossFade(AnimationsName.Run,0f);
        }

        public override void Stop()
        {
            Animator.StopPlayback();
        }
    }
}