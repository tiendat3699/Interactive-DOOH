using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CTAEventListener : MonoBehaviour
{
    public CTAEvent Event;

    public UnityEvent Respone;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnRegisterListener(this);
    }

    public void OnEventRaised()
    {
        Respone.Invoke();
    }
}
