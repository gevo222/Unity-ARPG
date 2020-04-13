using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractEvent : UnityEvent { }

public class Interaction : MonoBehaviour
{
    private InteractEvent _Event;
    public InteractEvent Event { get; private set; }

    void Start()
    {
        Event?.AddListener(() => Debug.Log(string.Format("Interacted with object at {0}", transform.position)));
    }
}
