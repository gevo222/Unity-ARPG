using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyDeath : MonoBehaviour, Death
{
    public float RespawnDelay = 1f;
    public Vector3 RespawnLocation = Vector3.zero;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Death.die()
    {
        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        anim.Play("Die");
        yield return new WaitForSeconds(respawnDelay);
        transform.position = RespawnLocation;
    }
}
