using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyDeath : MonoBehaviour, Death
{
    public float RespawnDelay = 1f;
    public Vector3 RespawnLocation = Vector3.zero;
    public Player player;
    private Animator anim;
    private EnemyController controller;
    private CharacterStats stats;
    private Renderer renderer;
    private NavMeshAgent agent;
    private Collider collider;
    public Level lvl;
    public Animator playerAnim;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = gameObject.GetComponent<EnemyController>();
        stats = gameObject.GetComponent<CharacterStats>();
        renderer = gameObject.GetComponent<Renderer>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        collider = gameObject.GetComponent<Collider>();
        player = Player.instance;
        lvl = player.GetComponent<Level>();
        playerAnim = player.GetComponent<Animator>();
    }

    void Death.die()
    {
        StartCoroutine(Dead());
    }

    private void SetComponentEnabled(bool value)
    {
        anim.enabled = value;
        controller.enabled = value;
        stats.enabled = value;
        renderer.enabled = value;
        agent.enabled = value;
        collider.enabled = value;
    }

    private IEnumerator Dead()
    {
        anim.Play("Die");
        yield return new WaitForSeconds(1.5f);
        SetComponentEnabled(false);
        yield return new WaitForSeconds(RespawnDelay);
        stats.ResetHealth();
        SetComponentEnabled(true);
        transform.position = RespawnLocation;
        Level.currentXP += 1;
        playerAnim.SetBool("RClick", false);

        
    }
}
