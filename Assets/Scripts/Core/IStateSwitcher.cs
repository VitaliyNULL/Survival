using VitaliyNULL.StateMachine;

namespace VitaliyNULL.Core
{
    public interface IStateSwitcher
    {
        void SwitchState<T>() where T : State;
    }
}