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

        health.OnDeath += HandleDeath;
    }

    void Update()
    {
        if (isDead) return;

        // 2.5D: lock Z
        Vector3 p = transform.position;
        p.z = fixedZ;
        transform.position = p;

        float x = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(x, 0f, 0f).normalized * moveSpeed;

        bool grounded = cc.isGrounded;
        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        if (grounded && Input.GetButtonDown("Jump"))
            velocity += Vector3.up * jumpForce;

        velocity += Physics.gravity * Time.deltaTime;

        cc.Move((move + velocity) * Time.deltaTime);
    }

    void HandleDeath()
    {
        isDead = true;

        // Opcional: desactivar movimiento instantáneamente
        velocity = Vector3.zero;

        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerDied();
    }

    public void RespawnAt(Vector3 spawnPos)
    {
        isDead = false;

        velocity = Vector3.zero;
        fixedZ = spawnPos.z;

        // Reset de CC
        cc.enabled = false;
        transform.position = spawnPos;
        cc.enabled = true;

        health.ResetHealth();
    }
}
