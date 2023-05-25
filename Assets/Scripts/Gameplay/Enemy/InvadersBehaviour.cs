using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvadersBehaviour : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Invader invaderPrefab;
    [SerializeField] private Vector2 invadersMatrixSize;
    [Space]
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

    private Vector2 positionOffset;
    private List<Invader> shootingInvaders = new List<Invader>();

    private void Awake()
    {
        invadersTransform = transform;
        positionOffset = new Vector2(-Mathf.RoundToInt(invadersMatrixSize.x / 2), 4);
    }

    private void Start()
    {
        CreateInvadersWave();
        SetBumpersPosition();

        UpdateShooters();
        InvokeRepeating(nameof(TakeShot), 2f, fireRate);
    }

    private void Update() => Movement();

    private void CreateInvadersWave()
    {
        for (int y  = 0; y < invadersMatrixSize.y; y++)
        {
            for (int x = 0; x < invadersMatrixSize.x; x++)
            {
                var invader = Instantiate(invaderPrefab, new Vector2(x, -y) + positionOffset, invadersTransform.rotation);
                invader.transform.SetParent(invadersTransform);
                invader.ShipPosition = new Vector2Int(x, y);
                invader.name = $"Invader [{x}, {y}]";
            }
        }
    }

    public void SetBumpersPosition()
    {
        if (Invader.Invaders.Any(i => i.isActiveAndEnabled))
        {
            var firstCollumn = Invader.Invaders
                .Where(i => i.isActiveAndEnabled)
                .Min(i => i.ShipPosition.x);

            var lastCollumn = Invader.Invaders
                .Where(i => i.isActiveAndEnabled)
                .Max(i => i.ShipPosition.x);

            var leftBumperPositionX = Invader.Invaders.Find(i => i.ShipPosition.x == firstCollumn).transform.position.x;
            var rightBumperPositionX = Invader.Invaders.Find(i => i.ShipPosition.x == lastCollumn).transform.position.x;

            if (firstCollumn == lastCollumn)
                rightBumper.enabled = false;

            leftBumper.transform.position = new Vector2(leftBumperPositionX, leftBumper.transform.position.y);
            rightBumper.transform.position = new Vector2(rightBumperPositionX, rightBumper.transform.position.y);
        }
    }

    public void UpdateShooters()
    {
        var collumnsCount = invadersMatrixSize.x;
        shootingInvaders.Clear();

        for (int i = 0; i < collumnsCount; i++)
        {
            var shooter = Invader.Invaders
                .FindAll(e => e.ShipPosition.x == i && e.enabled)
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

        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage(playerHealth.value);
        }
    }
}