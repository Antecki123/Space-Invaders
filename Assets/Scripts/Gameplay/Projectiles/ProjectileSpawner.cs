using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public static Action<Vector3, Quaternion> OnProjectileSpawn;
    public static Action<Projectile> OnProjectileDestroy;

    [Header("Component References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectilesCount = 50;

    private Queue<Projectile> projectiles = new Queue<Projectile>();

    private void OnEnable()
    {
        OnProjectileSpawn += InstantiateProjectile;
        OnProjectileDestroy += DestroyProjectile;
    }
    private void OnDisable()
    {
        OnProjectileSpawn -= InstantiateProjectile;
        OnProjectileDestroy -= DestroyProjectile;
    }

    private void Start()
    {
        CreateNewProjectile(projectilesCount);
    }

    public void InstantiateProjectile(Vector3 position, Quaternion rotation)
    {
        if (projectiles.Count <= 0) return;

        var projectile = projectiles.Dequeue();
        projectile.transform.SetPositionAndRotation(position, rotation);
        projectile.gameObject.SetActive(true);

        projectilesCount--;
    }

    public void DestroyProjectile(Projectile returned)
    {
        projectiles.Enqueue(returned);
        returned.gameObject.SetActive(false);

        projectilesCount++;
    }

    private void CreateNewProjectile(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.SetParent(transform);
            newProjectile.SetActive(false);

            projectiles.Enqueue(newProjectile.GetComponent<Projectile>());
        }
    }
}