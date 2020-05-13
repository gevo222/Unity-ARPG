using System;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Allows EnemyController to access Damage ! not recommended
[assembly: InternalsVisibleTo("Level")]

[Serializable] public class HealthUpdateEvent : UnityEvent<Transform, int> {}

public class CharacterStats : MonoBehaviour, Interactable
{
    public HealthUpdateEvent HealthUpdate => healthUpdate;

    [SerializeField] private HealthUpdateEvent healthUpdate;
    [SerializeField] internal Stat maxHP;
    [SerializeField] internal Stat damage;
    [SerializeField] private Stat armor;

    private int _currentHealth;
    public int currentHP {
        get { return _currentHealth; }
        private set {
	    this.HealthUpdate?.Invoke(transform, value);
            _currentHealth = value;
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

    public void ResetHealth()
    {
        currentHP = maxHP.GetStat();
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
