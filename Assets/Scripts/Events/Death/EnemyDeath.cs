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
        anim.SetBool("dead", true);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        transform.position = RespawnLocation;
        yield return new WaitForSeconds(RespawnDelay);
        gameObject.SetActive(true);
    }
}
