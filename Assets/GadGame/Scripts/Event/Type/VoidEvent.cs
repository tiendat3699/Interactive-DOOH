using System;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
#endif

namespace GadGame.Event.Type
{
    [CreateAssetMenu(menuName = "Event/Void")]
    public class VoidEvent : ScriptableObject
    {
        private Action _event;
#if UNITY_EDITOR
        [ShowInInspector, HideInEditorMode, ReadOnly]
        private List<Delegate> _listeners = new();
#endif
        
        public void Register(Action action)
        {
            _event += action;
#if UNITY_EDITOR
            _listeners.Add(action);
#endif
        }

        public void Unregister(Action action)
        {
            _event -= action;
#if UNITY_EDITOR
            _listeners.Remove(action);
#endif
        }
        
        [Button]
        public void Raise()
        {
            _event?.Invoke();
        }
    }
}