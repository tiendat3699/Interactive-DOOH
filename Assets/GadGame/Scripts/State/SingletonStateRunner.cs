﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GadGame.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GadGame.State
{
    public class SingletonStateRunner<T> : Singleton<T> where T : MonoBehaviour
    {
        protected State<T> ActiveState;

#if UNITY_EDITOR
        [ValueDropdown("AllStates")]
#endif
        [SerializeField]
        private List<string> _availableStates;

        private List<State<T>> _states;
        private float _timer;

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


        protected override void Awake()
        {
            base.Awake();
            _states = new List<State<T>>();

            foreach (var stateName in _availableStates)
            {
                var stateType = Assembly.GetExecutingAssembly().GetType(stateName);
                var state = Activator.CreateInstance(stateType) as State<T>;
                _states.Add(state);
                state?.Init(GetComponent<T>());
            }
        }

        protected virtual void Update()
        {
            if (ActiveState != null)
            {
                Debug.Log(ActiveState);
                _timer += Time.deltaTime;
                ActiveState.Update(_timer);
            }
        }

        public void SetState<TSt>() where TSt : State<T>
        {
            if (ActiveState != null)
            {
                if (ActiveState is TSt) return;
                ActiveState.Exit();
            }

            var newState = _states.FirstOrDefault(s => s is TSt);
            if (newState == null) return;
            ActiveState = newState;
            ActiveState.Enter();
            _timer = 0;
        }
    }
}