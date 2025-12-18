using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    public event Action<int, int> OnHealthChanged; // current, max
    public event Action OnDeath;

    bool dead;

    void Awake()
    {
        ResetHealth();
    }

    public void TakeDamage(int amount)
    {
        if (dead) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            dead = true;
            OnDeath?.Invoke();
        }
    }

    public void ResetHealth()
    {
        dead = false;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
