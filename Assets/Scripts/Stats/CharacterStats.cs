using System;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Allows EnemyController to access Damage ! not recommended
[assembly: InternalsVisibleTo("EnemyController")]

[Serializable] public class HealthUpdateEvent : UnityEvent<Transform, int> {}

public class CharacterStats : MonoBehaviour, Interactable
{
    public HealthUpdateEvent HealthUpdate => healthUpdate;

    [SerializeField] private HealthUpdateEvent healthUpdate;
    [SerializeField] private Stat maxHP;
    [SerializeField] private Stat damage;
    [SerializeField] private Stat armor;

    private int _currentHealth;
    private int currentHP {
        get { return _currentHealth; }
        set {
            _currentHealth = value;
	    this.HealthUpdate?.Invoke(transform, value);
        }
    }

     // Temporary test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public virtual void Start()
    {
        if (healthUpdate == null)
        {
            healthUpdate = new HealthUpdateEvent();
        }
        currentHP = maxHP.GetStat();
    }

    public void Attack(CharacterStats target, float multiplier = 1f)
    {
        target.TakeDamage((int)(damage.GetStat() * multiplier));
    }

    public void TakeDamage(int rawDamage)
    {
        var finalDamage = Mathf.Clamp(rawDamage - armor.GetStat(), 0, 9999);
        currentHP = currentHP - rawDamage;
    }

    public void Heal(int rawHeal)
    {
        currentHP = currentHP + rawHeal;
    }
}
