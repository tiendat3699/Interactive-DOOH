using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GadGame.State
{
    public class StateRunner<T> : MonoBehaviour where T : MonoBehaviour
    {
        private State<T> _activeState;
        
#if UNITY_EDITOR
        [ValueDropdown("AllStates")]
#endif
        [SerializeField]
        private List<string> _availableStates;
        private List<State<T>> _states;
        
#if UNITY_EDITOR
        public IEnumerable AllStates
        {
            get
            {
                return Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.BaseType is { IsGenericType: true } &&
                                t.BaseType.GetGenericTypeDefinition() == typeof(State<>)).Select(t => t.FullName);
            }
        }
#endif
        
        
        protected virtual void Awake()
        {
            _states = new List<State<T>>();

            foreach (var stateName in _availableStates)
            {
                var stateType = Assembly.GetExecutingAssembly().GetType(stateName);
                var state = Activator.CreateInstance(stateType) as State<T>;
                _states.Add(state);
                state?.Init(GetComponent<T>());
            }
        }

        private void Update()
        {
            _activeState.Update();
        }

        public void SetState(Type stateType)
        {
            if (_activeState != null)
            {
                if(_activeState.GetType() == stateType) return;
                _activeState.Exit();
            }

            var newState = _states.FirstOrDefault(s => s.GetType() == stateType);
            if(newState == null) return;
            _activeState = newState;
            _activeState.Enter();
            Debug.Log(stateType.ToString());
        }
    }
}