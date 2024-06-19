using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CTAEvent : ScriptableObject
{
    private List<CTAEventListener> listeners = new List<CTAEventListener>();

    public void RegisterListener(CTAEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(CTAEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }
}
