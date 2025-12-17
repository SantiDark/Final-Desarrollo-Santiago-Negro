using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    [Header("Theme / Flavor (para el examen)")]
    public string enemyThemeName = "Stalker"; // Cambialo a la temática del parcial

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float detectRange = 10f;   // si el player entra acá, lo persigue
    public float stopDistance = 1.6f; // a esta distancia deja de avanzar (para atacar)

    [Header("Attack")]
    public int damage = 1;
    public float attackRange = 1.8f;
    public float attackCooldown = 0.8f;

    CharacterController cc;
    Health health;

    Transform player;
    Health playerHealth;

    float fixedZ;
    float nextAttackTime;
    bool isDead;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        health = GetComponent<Health>();

        fixedZ = transform.position.z;

        health.OnDeath += OnDeath;
    }

    void Start()
    {
        // Busca al player por Tag. Asegurate que tu Player tenga Tag "Player".
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerHealth = p.GetComponent<Health>();
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        // Forzar 2.5D: Enemy también clavado en Z
        Vector3 pos = transform.position;
        pos.z = fixedZ;
        transform.position = pos;

        float dist = Vector3.Distance(transform.position, player.position);

        // Si está lejos, no hace nada (queda “al acecho”)
        if (dist > detectRange)
            return;

        // Moverse hacia el player solo por X
        float dirX = Mathf.Sign(player.position.x - transform.position.x);

        // Si está suficientemente cerca, atacar
        if (dist <= attackRange)
        {
            TryAttack();
            return;
        }

        // Si está entre detectRange y stopDistance, perseguir
        if (dist > stopDistance)
        {
            Vector3 move = new Vector3(dirX, 0f, 0f) * moveSpeed;
            cc.Move(move * Time.deltaTime);
        }
    }

    void TryAttack()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        if (playerHealth != null && !playerHealth.IsDead)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    void OnDeath()
    {
        isDead = true;

        // Para que deje de “interactuar” con todo al morir:
        // - Apagamos el CharacterController para que no empuje
        cc.enabled = false;

        // Si querés que desaparezca:
        Destroy(gameObject, 0.25f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
