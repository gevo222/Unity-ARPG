using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// Enemy follows the player TODO: Make it attack the player
public class EnemyController : MonoBehaviour
{

    public float lookRadius = 10f;

    Transform player;
    NavMeshAgent agent;
    CharacterStats playerStats;
    CharacterStats enemyStats;
    int enemyDamage;
    int playerDamage;
    int elapsedTime;
    Animator playerAnim;
    Animator enemyAnim;

    void Start()
    {
        player = Player.instance.transform;
        agent = GetComponent<NavMeshAgent>();
        playerStats = player.GetComponent<CharacterStats>();
        enemyStats = GetComponent<CharacterStats>();
        playerDamage = playerStats.damage.GetStat();
        enemyDamage = enemyStats.damage.GetStat();
        playerAnim = player.GetComponent<Animator>();
        enemyAnim = GetComponent<Animator>();
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

                //if (elapsedTime > 1)
                
                   // playerStats.TakeDamage(enemyDamage);
                    enemyStats.TakeDamage(playerDamage);
                    playerAnim.SetBool("RClick", true);
                    enemyAnim.SetBool("RClick", true);

                // playerstats.currentHP = playerstats.currentHP - enemystats;
                // enemystats.currentHP = enemystats.currentHP - 1;
            }
            else
            {
                playerAnim.SetBool("RClick", false);
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