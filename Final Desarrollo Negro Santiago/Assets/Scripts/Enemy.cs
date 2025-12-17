using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyDummy : MonoBehaviour
{
    Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += Die;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
