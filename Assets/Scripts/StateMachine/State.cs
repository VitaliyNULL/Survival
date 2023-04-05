using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.StateMachine
{
    public abstract class State
    {
        protected Animator Animator;
        protected HashAnimationsName AnimationsName = new HashAnimationsName();

        protected State(Animator animator)
        {
            Animator = animator;
        }
        public abstract void Start();
        public abstract void Stop();
    }
}