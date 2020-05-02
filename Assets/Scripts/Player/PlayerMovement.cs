using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[Serializable] public class InteractEvent : UnityEvent<GameObject, GameObject> {}
public interface Interactable {}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float interactDistance;

    [SerializeField] public InteractEvent Event;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                var interaction = hit.transform.gameObject.GetComponent<Interactable>();
                if (interaction != null &&
                        Vector3.Distance(transform.position, hit.point) <= interactDistance)
                {
                    Event?.Invoke(this.transform.gameObject, hit.transform.gameObject);
                } else {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
