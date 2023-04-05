using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class HitState : State
    {
        public HitState(Animator animator) : base(animator)
        {
        }

        public override void Start()
        {
            Animator.CrossFade(AnimationsName.Hit, 0f);
        }

        public override void Stop()
        {
            Animator.StopPlayback();
        }
    }
}