using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.2f;

    float cd;

    void Update()
    {
        if (cd > 0f) cd -= Time.deltaTime;

        if (Input.GetButton("Fire1") && cd <= 0f)
        {
            Shoot();
            cd = shootCooldown;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject b = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        float x = Input.GetAxisRaw("Horizontal");
        Vector3 dir = (x < 0f) ? Vector3.left : Vector3.right;

        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Init(dir, transform.position.z);
    }
}
