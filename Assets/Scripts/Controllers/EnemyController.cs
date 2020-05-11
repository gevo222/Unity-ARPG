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
    AudioSource fightMusic;


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
        fightMusic = GetComponent<AudioSource>();
    }

    void Update()
    {
        // check if player is within look radius
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= lookRadius)
        {
            // Follow the player
            agent.SetDestination(player.position);
            enemyAnim.SetBool("Run Forward", true);

            // If already in range, always face the player
            if (distance <= agent.stoppingDistance)
            {
                FacePlayer();

                //if (elapsedTime > 1)
                enemyAnim.SetBool("Run Forward", false);
                //Damage player's stats
                // playerStats.TakeDamage(enemyDamage);
                //Damage enemy's stats
                enemyStats.TakeDamage(playerDamage);
                //Play attack animation for player
                playerAnim.SetBool("RClick", true);
                //Play attack animation for enemy
                    enemyAnim.SetTrigger("Stab Attack");

                // Deal double damage with cyclone
                if (playerAnim.GetBool("Q"))
                {
                    enemyStats.TakeDamage(playerDamage);
                }
                
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