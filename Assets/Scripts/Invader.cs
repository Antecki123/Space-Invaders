using UnityEngine;

public class Invader : MonoBehaviour, IDamageable
{
    public static int InvadersCount;

    [Header("Invader Properties")]
    [SerializeField] private float health = 2;

    [Header("Component References")]
    [SerializeField] private ObjectsSpawner projectileSpawner;
    [SerializeField] private Transform firePoint;
    [Space]
    [SerializeField] private FloatVariable playerScore;
    [SerializeField] private GameEvent OnEnemyDestroy;

    [Header("Animation References")]
    [SerializeField] private GameObject destroyAnimation;

    [field: SerializeField] public Vector2Int ShipPosition { get; private set; }

    private void OnEnable() => InvadersCount++;

    public void FireProjectile()
    {
        projectileSpawner.InstantiateObject(firePoint.position, firePoint.rotation);
    }

    public void GetDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            var explosion = Instantiate(destroyAnimation, transform.position, transform.rotation);
            Destroy(explosion, 2f);

            InvadersCount--;
            playerScore.value += 10.0f;

            gameObject.SetActive(false);
            OnEnemyDestroy?.Invoke();
        }
    }
}