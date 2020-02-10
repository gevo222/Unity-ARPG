using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractEvent : UnityEvent { }

public class Interaction : MonoBehaviour
{
    private InteractEvent _Event = new InteractEvent();
    public InteractEvent Event { get; }
}
