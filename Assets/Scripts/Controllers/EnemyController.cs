using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// Enemy follows the player TODO: Make it attack the player
public class EnemyController : MonoBehaviour
{

    public float lookRadius = 10f;

    Transform player;
    NavMeshAgent agent;

    void Start()
    {
        player = Player.instance.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // check if player is within look radius
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= lookRadius)
        {
            // Follow the player
            agent.SetDestination(player.position);

            // If already in range, always face the player
            if (distance <= agent.stoppingDistance)
            {
                FacePlayer();
            }
        }
    }

    // Face the player
    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Show aggro range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}