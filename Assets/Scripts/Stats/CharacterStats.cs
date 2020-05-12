using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


[Serializable] public class HealthUpdateEvent : UnityEvent<Transform, int> {}

public class CharacterStats : MonoBehaviour, Interactable
{
    Transform player;
    [SerializeField] private HealthUpdateEvent healthUpdate;

    public int currentHP;
    public Stat maxHP;
    public Stat damage;
    public Stat armor;
    Animator anim;
    Animator playerAnim;


    public virtual void Start()
    {
        player = Player.instance.transform;
        playerAnim = player.GetComponent<Animator>();
        anim = GetComponent<Animator>();
        // Spawn with Max HP
        currentHP = maxHP.GetStat();
    }

    // Calculate and deal damage
    public void TakeDamage(int rawDamage)
    {
        // Reduce damage with armor
        int finalDamage = rawDamage;
        finalDamage -= armor.GetStat();
        finalDamage = Mathf.Clamp(finalDamage, 0, int.MaxValue);

        // Take damage
        currentHP -= finalDamage;
	healthUpdate?.Invoke(transform, currentHP);

        // Die when health reaches 0
        if (currentHP <= 0)
        {
           // Might need to make this overwriteable to give enemies deaths too
           StartCoroutine(Dead());

        }
    }

    // Player death
    public IEnumerator Dead()
    {

        // If player dies
        if (transform.tag == "Player")
        {

            // Play death animation
            anim.SetBool("dead", true);

            // TODO: Game Over message


            // Wait
            yield return new WaitForSeconds(2f);

            // Restart level Prompt
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // if anything else dies
        else
        {

            // play death animation
           anim.Play("Die");

            // destroy it
           yield return new WaitForSeconds(2f);
           Destroy(gameObject);

            // player stops attacking
            playerAnim.SetBool("RClick", false);
            Level.currentXP += 1;
            Debug.Log("xp: " + Level.currentXP);
            Debug.Log("LVL: " + Level.level);

            if (transform.tag == "Boss")
            {
                Level.currentXP += 9;
            }
        }

    }

    // Heal the character.
    public void Heal(int rawHeal)
    {
        currentHP += rawHeal;


        // Prevent overheal
        currentHP = Mathf.Clamp(currentHP, 0, maxHP.GetStat());
    }



}
