using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth;

    public bool IsDead { get; private set; }

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged;

    void Awake()
    {
        ResetHealth();
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        if (amount <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (IsDead) return;

        IsDead = true;
        OnDeath?.Invoke();
    }

    // 🔹 ESTE MÉTODO ES EL QUE FALTABA
    public void ResetHealth()
    {
        IsDead = false;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
