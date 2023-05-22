using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Action<GameObject> OnDestroyProjectile;

    [Header("Projectile Settings")]
    [SerializeField] private float velocity = 5f;
    [SerializeField] private float damage = 1f;

    [Header("Animation References")]
    [SerializeField] private GameObject getHitAnimation;

    private Rigidbody2D rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            var explosion = Instantiate(getHitAnimation, transform.position, transform.rotation);
            explosion.transform.SetParent(collision.gameObject.transform);

            damageable.TakeDamage(damage);
            OnDestroyProjectile?.Invoke(gameObject);
        }

        if (collision.CompareTag("Boarder"))
            OnDestroyProjectile?.Invoke(gameObject);
    }
}