using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[Serializable] public class InteractEvent : UnityEvent<GameObject, GameObject> {}
public interface Interactable {}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float interactDistance;

    [SerializeField] public InteractEvent Event;
    private NavMeshAgent agent;

	private GameObject target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
		if(target != null && agent.hasPath && agent.remainingDistance <= interactDistance)
		{
			//TODO: Not sure how `InteractEvent Event` is meant to work with multiple events ?
			Event?.Invoke(this.transform.gameObject, target);
			target = null;
		}
		if(EventSystem.current.IsPointerOverGameObject() || !Input.GetMouseButton(0))
			return;

		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100))
		{
			var interaction = hit.transform.gameObject.GetComponent<Interactable>();
			target = (interaction == null) ? null : hit.transform.gameObject;

			agent.SetDestination(hit.point);
		}
    }
}
