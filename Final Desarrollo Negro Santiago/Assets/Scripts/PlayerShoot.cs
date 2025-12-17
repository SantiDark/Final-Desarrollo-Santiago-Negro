using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot")]
    public Bullet bulletPrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.2f;

    float nextShootTime;
    int facing = 1; // 1 = derecha, -1 = izquierda

    void Update()
    {
        // Detectar hacia dónde mira el player según input
        float x = Input.GetAxisRaw("Horizontal");
        if (x > 0.1f) facing = 1;
        else if (x < -0.1f) facing = -1;

        if (Time.time < nextShootTime) return;

        // Disparo tipo Mega Man
        if (Input.GetKey(KeyCode.Z))
        {
            Shoot();
            nextShootTime = Time.time + shootCooldown;
        }
    }

    void Shoot()
    {
        if (!bulletPrefab || !shootPoint) return;

        // Instanciar bala
        Bullet b = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Dirección SOLO en X
        Vector3 dir = (facing == 1) ? Vector3.right : Vector3.left;

        b.Init(dir);
    }
}
