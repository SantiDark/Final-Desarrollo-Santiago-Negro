using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    public float speed = 12f;
    public float lifeTime = 2f;
    public int damage = 1;

    Vector3 dir;
    float fixedZ;
    bool initialized;

    void Reset()
    {
        // Auto-config al agregar el script
        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }

    void OnEnable()
    {
        Invoke(nameof(Kill), lifeTime);
    }

    void OnDisable()
    {
        CancelInvoke();
        initialized = false;
    }

    public void Init(Vector3 direction, float zPlane)
    {
        dir = direction.normalized;
        fixedZ = zPlane;
        initialized = true;

        // Asegura 2.5D al nacer
        Vector3 p = transform.position;
        p.z = fixedZ;
        transform.position = p;
    }

    void Update()
    {
        if (!initialized) return;

        transform.position += dir * speed * Time.deltaTime;

        Vector3 p = transform.position;
        p.z = fixedZ;
        transform.position = p;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        Health h = other.GetComponentInParent<Health>();
        if (h != null)
        {
            h.TakeDamage(damage);
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
