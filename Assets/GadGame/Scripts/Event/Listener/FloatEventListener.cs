using GadGame.Event.Type;
using UnityEngine;

namespace GadGame.Event.Listener
{
    [AddComponentMenu("EventListener/"+ nameof(FloatEventListener))]
    public class FloatEventListener : GenericEventListener<float>
    {
        [SerializeField] private FloatEvent _event;

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