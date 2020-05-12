using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class LightningBolt : MonoBehaviour
{
    [SerializeField] private LightningBoltScript lightingPrefab;

    public void OnInteraction(GameObject actor, GameObject target)
    {
        if (actor == target) return;
        var actorStats = actor.GetComponent<CharacterStats>();
        var targetStats = target.GetComponent<CharacterStats>();
        if (actorStats && targetStats)
        {
            actorStats.Attack(targetStats, 1.3f);
            StartCoroutine(PlayLightningBoltAt(target.transform.position));
        }
    }

    private IEnumerator PlayLightningBoltAt(Vector3 location)
    {
        var lightning = Instantiate(lightingPrefab, location, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Destroy(lightning.gameObject);
    }
}
