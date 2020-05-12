using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private DeathEvent killManager;
    [SerializeField] private DamageNumbers damageNumbers;
    [SerializeField] private int spawnCount;
    [SerializeField] private int spawnRange;
    [SerializeField] private Vector2 spawnDelayInterval;

    private Transform[] spawnPoints;

    void Start()
    {
        Assert.IsTrue(spawnDelayInterval[0] <= spawnDelayInterval[1]);

        foreach (Transform spawnPoint in transform) {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 position;
                if (RandomPoint(spawnPoint.position, spawnRange, out position))
                {
                    var delay = Random.Range(spawnDelayInterval[0], spawnDelayInterval[1]);
                    StartCoroutine(SpawnPrefab(position, delay * i, spawnPoint));
                }
            }
        }
    }

    private IEnumerator SpawnPrefab(Vector3 position, float delay, Transform parent = null)
    {
        yield return new WaitForSeconds(delay);
        var instance = Instantiate(prefab, position, Quaternion.identity);
        var death = instance.GetComponent<EnemyDeath>();
        var stats = instance.GetComponent<CharacterStats>();
        instance.transform.parent = parent;
        death.RespawnLocation = position;
        death.RespawnDelay = delay;
        stats.HealthUpdate.AddListener(killManager.OnUpdateHealth);
        stats.HealthUpdate.AddListener(damageNumbers.OnUpdateHealth);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
