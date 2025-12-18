using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    [Header("Patrol (MegaMan)")]
    public float patrolSpeed = 2f;
    public float leftX = -2f;
    public float rightX = 2f;

    [Header("Vision & Shoot")]
    public float visionRange = 6f;
    public float shootInterval = 1.2f;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    Transform player;
    Health health;

    float shootTimer;
    int dir = 1; // 1 right, -1 left
    float fixedZ;

    void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += Die;

        fixedZ = transform.position.z;
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        // lock Z (evita “por atrás”)
        Vector3 pos = transform.position;
        pos.z = fixedZ;
        transform.position = pos;

        if (player == null)
        {
            Patrol();
            return;
        }

        // Si el player está dentro de rango horizontal: se frena y dispara
        float dx = player.position.x - transform.position.x;
        bool inRange = Mathf.Abs(dx) <= visionRange;

        if (inRange)
        {
            // “Mira” al player (solo cambia el lado de disparo)
            dir = (dx >= 0f) ? 1 : -1;

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootInterval;
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Vector3 p = transform.position;
        p.x += dir * patrolSpeed * Time.deltaTime;

        if (p.x <= leftX) { p.x = leftX; dir = 1; }
        if (p.x >= rightX) { p.x = rightX; dir = -1; }

        transform.position = p;
    }

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject b = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
        {
            Vector3 d = (dir == 1) ? Vector3.right : Vector3.left;
            bullet.Init(d, fixedZ);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
