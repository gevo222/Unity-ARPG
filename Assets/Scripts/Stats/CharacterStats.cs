using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Contains all the stats for a character. */

public class CharacterStats : MonoBehaviour
{

    public Stat maxHealth;          // Maximum amount of health
    public int currentHealth { get; protected set; }    // Current amount of health
    public Stat damage;
    public Stat armor;

    Animator anim;


    public virtual void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    // Temporary test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    // Start with max HP.
    public virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Damage the character
    public void TakeDamage(int damage)
    {
        // Subtract the armor value - Make sure damage doesn't go below 0.
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        // Subtract damage from health
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        // If we hit 0. Die.
        if (currentHealth <= 0)
        {

           StartCoroutine(Dead());
         
        }
    }

    public IEnumerator Dead()
    {
        // Play death animation
        anim.SetBool("dead", true);

        // TODO: Game Over message


        // Wait
        yield return new WaitForSeconds(2f);

        // Restart level Prompt
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }
    // Heal the character.
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
    }



}