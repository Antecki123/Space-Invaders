using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Player Settings")]
    [SerializeField, Min(0)] private float speed = 2f;

    [Header("Component References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ObjectsSpawner projectileSpawner;
    [SerializeField] private Transform firePoint;
    [Space]
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private GameEvent OnGetDamage;

    [Header("Animation References")]
    [SerializeField] private GameObject destroyAnimation;

    private InputControls inputActions;
    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Awake()
    {
        inputActions = new InputControls();

        inputActions.Gameplay.Movement.performed += ctx => Movement(ctx.ReadValue<float>());
        inputActions.Gameplay.Movement.canceled += ctx => Movement(0f);

        inputActions.Gameplay.Fire.performed += ctx => FireProjectile();
    }

    private void Movement(float direction)
    {
        if (playerHealth.value > 0)
        {
            rb.velocity = new Vector2(direction * speed, 0f);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 8f * -direction));
        }
        else
            transform.position = transform.position;
    }

    private void FireProjectile()
    {
        projectileSpawner.InstantiateObject(firePoint.position, firePoint.rotation);
    }

    public void TakeDamage(float damage)
    {
        playerHealth.value -= damage;
        OnGetDamage?.Invoke();

        if (playerHealth.value <= 0)
        {
            speed = 0f;

            var explosion = Instantiate(destroyAnimation, transform.position, transform.rotation);
            Destroy(explosion, 2f);

            gameObject.SetActive(false);
        }
    }
}