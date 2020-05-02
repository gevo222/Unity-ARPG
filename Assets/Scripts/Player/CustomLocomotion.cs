using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class CustomLocomotion : MonoBehaviour {
    Animator anim;
    NavMeshAgent agent;
    ScriptableObject sc;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Start ()
    {
        anim = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent> ();
        // Don’t update position automatically
        agent.updatePosition = false;
        
    }

    void Update ()
    {

        if (Input.GetKey(KeyCode.Q))
        {
            agent.updatePosition = true;
            Destroy(this);
            
        }
        else
        {
            agent.updatePosition = false;
        }
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot (transform.right, worldDeltaPosition);
        float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2 (dx, dy);
        
            

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
        smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;
        
        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
        
        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat ("velx", velocity.x);
        anim.SetFloat ("vely", velocity.y);

        // Sprint
        bool Shift = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("Shift", Shift);
        if (Shift)
            agent.speed = 12;
        else
            agent.speed = 8;

        // Roll
        bool E = Input.GetKey(KeyCode.E);
        anim.SetBool("E", E);
        // maybe slow agent.speed for a second when rolling?
        // make invinsible for a few frames when rolling?

        // Attack
        bool RClick = Input.GetMouseButton(1);
        anim.SetBool("RClick", RClick);
    }

    void OnAnimatorMove ()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }
}
