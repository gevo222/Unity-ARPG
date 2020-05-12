using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodmodeKill : MonoBehaviour
{
    public void OnInteraction(GameObject actor, GameObject target)
    {
        if (target.name == "PlayerWithCyclone") return;
        var characterStats = target.GetComponent<CharacterStats>();
        if (characterStats)
        {
            StartCoroutine(Kill(target));
        }
    }

    private IEnumerator Kill(GameObject enemy)
    {
        var enemyAnim = enemy.GetComponent<Animator>();
        enemyAnim.SetBool("dead", true);
        yield return new WaitForSeconds(2.0f);
        Destroy(enemy);
    }
}
