using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public ExperienceProgress xp;

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
        var xpPercent = (1.0f * currentXP / maxXP.Initvalue);
		xp.SetPercent(Mathf.Min(1.0f, xpPercent));

        if (xpPercent >= 1.0f)
        {
            level++;
            currentXP = 0;
            playerStats.damage.Initvalue += 5;
            playerStats.maxHP.Initvalue += 20;
        }
    }
}
