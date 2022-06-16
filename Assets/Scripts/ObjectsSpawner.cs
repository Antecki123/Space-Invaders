using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int objectsAmount = 30;

    private Queue<GameObject> objects = new();

    private void OnEnable() => Projectile.OnDestroyProjectile += DestroyObject;
    private void OnDisable() => Projectile.OnDestroyProjectile -= DestroyObject;

    private void Start()
    {
        for (int i = 0; i < objectsAmount; i++)
        {
            var newObject = Instantiate(prefab);
            newObject.transform.SetParent(transform);
            newObject.SetActive(false);

            objects.Enqueue(newObject);
        }
    }

    public void InstantiateObject(Vector3 position, Quaternion rotation)
    {
        var instance = objects.Dequeue();

        instance.transform.SetPositionAndRotation(position, rotation);
        instance.SetActive(true);
    }

    public void DestroyObject(GameObject returned)
    {
        objects.Enqueue(returned);
        returned.SetActive(false);
    }
}