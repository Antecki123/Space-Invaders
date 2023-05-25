using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Player Settings")]
    [SerializeField, Min(0)] private float speed = 2f;

    [Header("Component References")]
    [SerializeField] private Transform firePoint;
    [Space]
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private GameEvent OnGetDamage;

    [Header("Animation References")]
    [SerializeField] private GameObject destroyAnimation;
    private SpriteRenderer shipRenderer;

    private Transform playerTransform;
    private Rigidbody2D playerRigidbody;
    private InputControls inputActions;

    private void OnEnable() => inputActions.Enable();
    private void OnDisable()
    {
        StopAllCoroutines();
        inputActions.Disable();
    }

    private void Awake()
    {
        playerTransform = transform;
        shipRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();

        inputActions = new InputControls();

        inputActions.Gameplay.Movement.performed += ctx => Movement(ctx.ReadValue<float>());
        inputActions.Gameplay.Movement.canceled += ctx => Movement(0f);

        inputActions.Gameplay.Fire.performed += ctx => FireProjectile();
    }

    private void Start()
    {
        destroyAnimation.SetActive(false);
    }

    private void Movement(float direction)
    {
        if (playerHealth.value <= 0) return;

        playerRigidbody.velocity = new Vector2(direction * speed, 0f);
        playerTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 8f * -direction));
    }

    private void FireProjectile()
    {
        ProjectileSpawner.OnProjectileSpawn?.Invoke(firePoint.position, firePoint.rotation);
    }

    public void TakeDamage(float damage)
    {
        playerHealth.value -= damage;
        OnGetDamage.Invoke();

        if (playerHealth.value <= 0)
        {
            StartCoroutine(KillCharacter());
        }

        GameManager.OnGameOverConditions?.Invoke();
    }

    private IEnumerator KillCharacter()
    {
        var delaySec = 2f;

        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        shipRenderer.enabled = false;
        destroyAnimation.SetActive(true);

        yield return new WaitForSeconds(delaySec);
    }
}