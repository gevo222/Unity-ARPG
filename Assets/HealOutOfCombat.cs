using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOutOfCombat : MonoBehaviour
{
    Transform player;
    CharacterStats playerStats;
    float OutOfCombatTime;
    public static bool playerInCombat = false;
    public float healSpeed = 1f;
    private float healCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
        player = Player.instance.transform;
        playerStats = player.GetComponent<CharacterStats>();

    }

    // Update is called once per frame
    void Update()
    {
        healCooldown -= Time.deltaTime;
        if (playerInCombat == false && healCooldown <= 0f)
        {
            playerStats.Heal(1);
            healCooldown = 1f / healSpeed;
        }
    }
}
