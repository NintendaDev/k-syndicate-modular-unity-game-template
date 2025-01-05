using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Modules.StateMachines.States;

namespace Modules.StateMachines
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _registeredStates = new();
        private IExitableState _currentState;
        
        public Type PreviousState { get; private set; }

        public async UniTask SwitchState<TState>() where TState : class, IState
        {
            TState nextState = await GetAndPrepareNextState<TState>();
            await nextState.Enter();
        }

        public async UniTask SwitchState<TState, TPayload>(TPayload payload) where TState : class, IPaylodedState<TPayload>
        {
            TState nextState = await GetAndPrepareNextState<TState>();
            await nextState.Enter(payload);
        }

        public void RegisterState<TState>(TState state) where TState : IExitableState
        {
            Type stateType = typeof(TState);

            if (_registeredStates.ContainsKey(stateType) == true)
                return;

            _registeredStates.Add(stateType, state);
        }
            
        private async UniTask<TState> GetAndPrepareNextState<TState>() where TState : class, IExitableState
        {
            TState nextState = GetState<TState>();

            if (_currentState != null)
                await _currentState.Exit();

            PreviousState = _currentState?.GetType();
            _currentState = nextState;

            return nextState;
        }
    
        private TState GetState<TState>() where TState : class, IExitableState
        {
            Type stateType = typeof(TState);

            if (_registeredStates.ContainsKey(stateType) == false)
                throw new Exception($"The condition with type {stateType} is not registered");

            return _registeredStates[stateType] as TState;
        }
    }
}