using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycloneProperties : MonoBehaviour
{
    TrailRenderer trailren;
    AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        trailren = GetComponent<TrailRenderer>();
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && !trailren.emitting)
        {
            trailren.emitting = true;
            audioS.enabled = true;
        }
            
        else if (!Input.GetKey(KeyCode.Q))
        {
            trailren.emitting = false;
            audioS.enabled = false;
        }
            


    }
}
