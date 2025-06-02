using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class BoidManager : MonoBehaviour
{
    [Header("Boid Settings")]
    public GameObject prefab;
    public int boidCount = 0;
    public float InstantiateRadius = 0f;

    [Header("Movement Settings")]
    public float cohesionWeight = 1f;
    public float alignmentWeight = 1f;
    public float separationWeight = 1f;
    
    public float moveRadiusRange = 3f;
    public float boundaryForce = 3f;
    public float maxSpeed = 2f;
    public float neighborDistance = 1.5f;
    public int maxNeighbors = 50;

    [HideInInspector]
    public List<Boid> Boids = new List<Boid>();

    private void Start()
    {
        if (prefab != null && boidCount > 0)
        {
            SpawnBoids();
        }
    }

    private void Update()
    {
        Boids.RemoveAll(b => b == null);
        if (Boids.Count == 0)
        {
            NewEnemySpawner spawner = FindObjectOfType<NewEnemySpawner>();
            if (spawner != null)
            {
                spawner.currentEnemyCount--;
            }

            Destroy(gameObject);
        }
    }

    public void Setup(GameObject prefab, int count, float radius)
    {
        this.prefab = prefab;
        this.boidCount = count;
        this.InstantiateRadius = radius;
    }

    public void SpawnBoids()
    {
        for (int i = 0; i < boidCount; i++)
        {
            Vector3 offset = Random.insideUnitCircle * InstantiateRadius;
            Vector3 spawnPos = prefab.transform.position + new Vector3(offset.x, offset.y, 0f);
            Quaternion spawnRot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            GameObject go = Instantiate(prefab, spawnPos, spawnRot, transform);

            if (!go.GetComponent<Boid>().enabled)
            {
                Destroy(go);
                continue;
            }

            Boid boid = go.GetComponent<Boid>() ?? go.AddComponent<Boid>();
            boid.Initialize(this);
            Boids.Add(boid);
        }
    }
}