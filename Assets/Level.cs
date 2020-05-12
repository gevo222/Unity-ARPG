using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static int currentXP;
    public static int level;
    public Stat maxXP;
    CharacterStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<CharacterStats>();
        level = 1;
        currentXP = 0;
        maxXP.Initvalue = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentXP >= maxXP.Initvalue)
        {
            level++;
            currentXP = 0;
            playerStats.damage.Initvalue += 5;
            playerStats.maxHP.Initvalue += 20;
        }
    }
}
