using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 8f;

    CharacterController cc;
    Vector3 velocity;

    // Para mantener 2.5D: bloqueamos Z
    float fixedZ;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        fixedZ = transform.position.z;
    }

    void Update()
    {
        // Forzar que el player siempre quede en el mismo Z (2.5D)
        Vector3 p = transform.position;
        p.z = fixedZ;
        transform.position = p;

        // Movimiento lateral (X)
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(x, 0f, 0f).normalized * moveSpeed;

        bool grounded = cc.isGrounded;
        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        // Salto
        if (grounded && Input.GetButtonDown("Jump"))
            velocity += Vector3.up * jumpForce;

        // Gravedad
        velocity += Physics.gravity * Time.deltaTime;

        // Aplicar movimiento (vector math)
        cc.Move((move + velocity) * Time.deltaTime);
    }
}
