using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvadersBehaviour : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private List<Invader> invaders = new();
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private Collider2D leftBumper;
    [SerializeField] private Collider2D rightBumper;

    [Header("Invaders Wave Settings")]
    [SerializeField, Range(0, 10)] private float horizontalSpeed = 3.0f;
    [SerializeField, Range(0, 5)] private float ascendingDistance = .2f;
    [Space]
    [SerializeField] private float fireRate = .5f;

    private Transform invadersTransform;
    private int direction = 1;
    private bool verticalMove = false;

    private List<Invader> shootingInvaders = new();

    private void Awake()
    {
        invadersTransform = transform;
    }

    private void Start()
    {
        UpdateShooters();
        InvokeRepeating(nameof(TakeShot), 2f, fireRate);
    }

    private void Update() => Movement();

    public void MoveBumpers()
    {
        if (invaders.Any(i => i.isActiveAndEnabled))
        {
            var firstCollumn = invaders
                .Where(i => i.isActiveAndEnabled)
                .Min(i => i.ShipPosition.x);

            var lastCollumn = invaders
                .Where(i => i.isActiveAndEnabled)
                .Max(i => i.ShipPosition.x);

            var leftBumperPositionX = invaders.Find(i => i.ShipPosition.x == firstCollumn).transform.position.x;
            var rightBumperPositionX = invaders.Find(i => i.ShipPosition.x == lastCollumn).transform.position.x;

            if (firstCollumn == lastCollumn)
                rightBumper.enabled = false;

            leftBumper.transform.position = new Vector2(leftBumperPositionX, leftBumper.transform.position.y);
            rightBumper.transform.position = new Vector2(rightBumperPositionX, rightBumper.transform.position.y);
        }
    }

    public void UpdateShooters()
    {
        var collumnsCount = 13;
        shootingInvaders.Clear();

        for (int i = 0; i < collumnsCount; i++)
        {
            var shooter = invaders
                .FindAll(e => e.ShipPosition.x == i && e.isActiveAndEnabled)
                .OrderByDescending(e => e.ShipPosition.y)
                .FirstOrDefault();

            if (shooter != null)
                shootingInvaders.Add(shooter);
        }
    }

    private void Movement()
    {
        invadersTransform.Translate(Time.deltaTime * horizontalSpeed * direction * invadersTransform.right);

        if (verticalMove)
        {
            invadersTransform.position = new Vector2(invadersTransform.position.x, invadersTransform.position.y - ascendingDistance);
            verticalMove = false;
        }
    }

    private void TakeShot()
    {
        if (shootingInvaders.Count > 0)
            shootingInvaders[Random.Range(0, shootingInvaders.Count)].FireProjectile();

        else
            CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boarder"))
        {
            direction = -direction;
            verticalMove = true;
        }

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamageable>().TakeDamage(playerHealth.value);
        }
    }
}