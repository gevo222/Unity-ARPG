using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public void OnInteraction(GameObject actor, GameObject target)
    {
        if (actor == target) return;
        var actorStats = actor.GetComponent<CharacterStats>();
        var targetStats = target.GetComponent<CharacterStats>();
        if (actorStats && targetStats)
        {
            actorStats.Attack(targetStats);
        }
    }
}
