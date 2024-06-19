using GadGame.Event.Type;
using UnityEngine;
using UnityEngine.Events;

namespace GadGame.Event.Listener
{
    [AddComponentMenu("EventListener/"+ nameof(VoidEventListener))]
    public class VoidEventListener : MonoBehaviour
    {
        [SerializeField] private VoidEvent _event;
        [SerializeField] private UnityEvent _action;

        private void OnEnable()
        {
            _event.Register(OnEventRaise);
        }

        private void OnDisable()
        {
            _event.Unregister(OnEventRaise);
        }

        private void OnEventRaise()
        {
            _action?.Invoke();
        }
    }
}