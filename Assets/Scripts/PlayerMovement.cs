using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private NavMeshAgent agent;

    [SerializeField]
    private float interactDistance;

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
                var interactions = hit.transform.gameObject.GetComponentsInChildren<Interaction>();
                foreach (var i in interactions)
                {
                    if (Vector3.Distance(transform.position, hit.transform.position) <= interactDistance)
                    {
                        i.Event.Invoke();
                    }
                }
                agent.SetDestination(hit.point);
            }
        }
    }
}
