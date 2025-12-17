using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 2f;
    public int damage = 1;

    Vector3 direction;

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Golpea enemigos
        if (other.CompareTag("Enemy"))
        {
            Health h = other.GetComponent<Health>();
            if (h != null)
                h.TakeDamage(damage);

            Destroy(gameObject);
        }

        // Choca con paredes / piso
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);
        }
    }
}
