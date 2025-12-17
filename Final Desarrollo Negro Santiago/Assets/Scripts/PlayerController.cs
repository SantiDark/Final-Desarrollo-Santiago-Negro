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
    Vector3 spawnPosition;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        health = GetComponent<Health>();

        fixedZ = transform.position.z;
        spawnPosition = transform.position;

        health.OnDeath += OnDeath;
    }

    void Update()
    {
        // TEST: recibir daño con K
        if (Input.GetKeyDown(KeyCode.K))
        {
            health.TakeDamage(1);
        }

        // Forzar 2.5D
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

    void OnDeath()
    {
        enabled = false;
    }
}
