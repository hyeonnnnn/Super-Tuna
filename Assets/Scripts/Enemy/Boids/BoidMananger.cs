using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager Instance;

    public GameObject prefab;

    [Header("Init")]
    public float InstantiateRadius;
    public int number;

    public List<GameObject> Boids = new List<GameObject>();

    [Header("MoveManage")]
    public float cohesionWeight, alignmentWeight, separationWeight = 1.0f;
    public float moveRadiusRange, boundaryForce;
    public float maxSpeed = 2.0f;
    public float neighborDistance, maxNeighbors = 50;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < number; ++i)
        {
            Boids.Add(Instantiate(prefab, this.transform.position + Random.insideUnitSphere * InstantiateRadius, Random.rotation));
        }
    }
}
