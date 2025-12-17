using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 8f;

    CharacterController cc;
    Health health;

    Vector3 velocity;
    float fixedZ;
    bool isDead;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        health = GetComponent<Health>();

        fixedZ = transform.position.z;

        health.OnDeath += OnDeath;
    }

    void OnDestroy()
    {
        if (health != null)
            health.OnDeath -= OnDeath;
    }

    void Update()
    {
        // TEST: daño manual
        if (Input.GetKeyDown(KeyCode.K))
            health.TakeDamage(1);

        if (isDead) return;

        // Forzar 2.5D (bloquea el Z)
        Vector3 p = transform.position;
        p.z = fixedZ;
        transform.position = p;

        float x = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(x, 0f, 0f).normalized * moveSpeed;

        bool grounded = cc.isGrounded;
        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        if (grounded && Input.GetButtonDown("Jump"))
            velocity.y = jumpForce;

        velocity += Physics.gravity * Time.deltaTime;

        // Move = m/s, velocity = m/s -> multiplicamos por dt una sola vez
        cc.Move((move + new Vector3(0f, velocity.y, 0f)) * Time.deltaTime);
    }

    void OnDeath()
    {
        isDead = true;

        // Si vas a respawnear desde GameManager, NO destruyas acá
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerDied();

        // Si quisieras que se destruya en vez de respawn:
        // Destroy(gameObject);
    }

    public void RespawnAt(Vector3 spawnPos)
    {
        isDead = false;

        // Importante: desactivar CC antes de teletransportar
        cc.enabled = false;

        velocity = Vector3.zero;
        fixedZ = spawnPos.z;

        spawnPos.z = fixedZ;
        transform.position = spawnPos;

        // Esto requiere que exista en tu Health
        health.ResetHealth();

        cc.enabled = true;
    }
}
