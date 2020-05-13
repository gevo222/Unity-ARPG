using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class PlayerDeath : MonoBehaviour, Death
{
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
