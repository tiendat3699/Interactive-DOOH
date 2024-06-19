using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GadGame.Event
{
    public abstract class GenericEventListener<T> : MonoBehaviour
    {
        [SerializeField, PropertyOrder(10)] private UnityEvent<T> _action;

        protected abstract void RegisterEvent();
        protected abstract void UnRegisterEvent();
        
        private void OnEnable()
        {
            RegisterEvent();
        }

        private void OnDisable()
        {
            UnRegisterEvent();
        }

        protected void OnEventRaise(T value)
        {
            _action?.Invoke(value);
        }
    }
}