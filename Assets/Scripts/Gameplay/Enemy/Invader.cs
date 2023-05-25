using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour, IDamageable
{
    public static List<Invader> Invaders { get; private set; } = new List<Invader>();

    [Header("Invader Properties")]
    [SerializeField] private float health = 2;

    [Header("Component References")]
    [SerializeField] private Transform firePoint;
    [Space]
    [SerializeField] private FloatVariable playerScore;
    [SerializeField] private GameEvent OnEnemyDestroy;

    [Header("Animation References")]
    [SerializeField] private GameObject destroyAnimation;

    [field: SerializeField] public Vector2Int ShipPosition { get; set; }

    private Renderer invaderRenderer;
    private Collider2D invaderCollider;

    private void Awake()
    {
        invaderRenderer = GetComponent<Renderer>();
        invaderCollider = GetComponent<Collider2D>();
    }

    private void OnEnable() => Invaders.Add(this);
    private void OnDisable()
    {
        StopAllCoroutines();
        Invaders.Remove(this);
    }

    public void FireProjectile()
    {
        ProjectileSpawner.OnProjectileSpawn?.Invoke(firePoint.position, firePoint.rotation);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            StartCoroutine(KillCharacter());
        }

        GameManager.OnGameOverConditions?.Invoke();
    }

    private IEnumerator KillCharacter()
    {
        var delaySec = 2f;

        playerScore.value += 10.0f;

        enabled = false;
        invaderRenderer.enabled = false;
        invaderCollider.enabled = false;
        destroyAnimation.SetActive(true);
        OnEnemyDestroy.Invoke();

        yield return new WaitForSeconds(delaySec);

        gameObject.SetActive(false);
    }
}