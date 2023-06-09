using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.StateMachine
{
    public class StateMachine : NetworkBehaviour, IStateSwitcher
    {
        #region Public Fields

        public Dictionary<Type, State> dictionary = new Dictionary<Type, State>();
        public State CurrentState;

        #endregion

        #region Private Fields

        private Animator _animator;
        [SerializeField] private bool isEnemy;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            dictionary.Add(typeof(RunState), new RunState(_animator));
            dictionary.Add(typeof(DeadState), new DeadState(_animator));
            if (isEnemy)
            {
                dictionary.Add(typeof(HitState), new HitState(_animator));
            }
            else
            {
                dictionary.Add(typeof(StandState), new StandState(_animator));
            }
        }

        #endregion

        #region IStateSwitcher

        public void SwitchState<T>() where T : State
        {
            CurrentState?.Stop();
            CurrentState = dictionary[typeof(T)];
            CurrentState.Start();
        }

        #endregion
    }
}