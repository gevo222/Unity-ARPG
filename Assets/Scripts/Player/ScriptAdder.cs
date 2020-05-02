using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptAdder : MonoBehaviour
{
    public GameObject obj;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool Q = Input.GetKey(KeyCode.Q);
        anim.SetBool("Q", Q);

        if (!Q && (obj.GetComponent<CustomLocomotion>() == null))
        {
            obj.AddComponent<CustomLocomotion>();
        }

     
    }
}
