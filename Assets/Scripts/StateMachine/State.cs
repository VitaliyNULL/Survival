using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.StateMachine
{
    public abstract class State
    {
        #region Protected Fields

        protected Animator Animator;
        protected HashAnimationsName AnimationsName = new HashAnimationsName();

        #endregion

        #region Constructor

        protected State(Animator animator)
        {
            Animator = animator;
        }

        #endregion

        #region Abstract methods

        public abstract void Start();
        public abstract void Stop();

        #endregion
    }
}