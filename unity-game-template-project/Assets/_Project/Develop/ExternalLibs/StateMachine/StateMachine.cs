using Cysharp.Threading.Tasks;
using ExternalLibs.CoreStateMachine.States;
using System;
using System.Collections.Generic;

namespace ExternalLibs.CoreStateMachine
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _registeredStates = new();
        private IExitableState _currentState;

        public async UniTask SwitchState<TState>() where TState : class, IState
        {
            TState nextState = await GetNextStateWithSetCurrentState<TState>();
            await nextState.Enter();
        }

        public async UniTask SwitchState<TState, TPayload>(TPayload payload) where TState : class, IPaylodedState<TPayload>
        {
            TState nextState = await GetNextStateWithSetCurrentState<TState>();
            await nextState.Enter(payload);
        }

        public void RegisterState<TState>(TState state) where TState : IExitableState
        {
            Type stateType = typeof(TState);

            if (_registeredStates.ContainsKey(stateType) == true)
                return;

            _registeredStates.Add(stateType, state);
        }
            
        private async UniTask<TState> GetNextStateWithSetCurrentState<TState>() where TState : class, IExitableState
        {
            TState nextState = GetState<TState>();

            if (_currentState != null)
                await _currentState.Exit();

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