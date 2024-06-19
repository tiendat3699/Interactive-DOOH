using GadGame.Event.Type;
using UnityEngine;

namespace GadGame.Event.Listener
{
    [AddComponentMenu("EventListener/"+ nameof(QuaternionEventListener))]
    public class QuaternionEventListener : GenericEventListener<Quaternion>
    {
        [SerializeField] private QuaternionEvent _event;

        protected override void RegisterEvent()
        {
            _event.Register(OnEventRaise);
        }

        protected override void UnRegisterEvent()
        {
            _event.Unregister(OnEventRaise);
        }
    }
}