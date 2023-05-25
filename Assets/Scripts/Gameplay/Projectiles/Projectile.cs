using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float velocity = 5f;
    [SerializeField] private float damage = 1f;

    [Header("Animation References")]
    [SerializeField] private Transform explosionPrefab;

    private Transform projectileTransform;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        projectileTransform = transform;

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            explosionPrefab.SetPositionAndRotation(projectileTransform.position, projectileTransform.rotation);
            explosionPrefab.SetParent(collision.gameObject.transform);
            explosionPrefab.gameObject.SetActive(true);

            damageable.TakeDamage(damage);
            ProjectileSpawner.OnProjectileDestroy?.Invoke(this);
        }

        if (collision.CompareTag("Boarder"))
            ProjectileSpawner.OnProjectileDestroy?.Invoke(this);
    }
}