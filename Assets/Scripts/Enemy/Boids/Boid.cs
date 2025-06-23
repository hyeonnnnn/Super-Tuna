using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private BoidManager spawner;
    private Vector3 velocity;
    private List<Boid> nearNeighbors = new List<Boid>();
    
    [SerializeField, HideInInspector]
    private int neighborCount;

    public void Initialize(BoidManager manager)
    {
        spawner = manager;
        Vector2 dir = Random.insideUnitCircle.normalized;
        velocity = new Vector3(dir.x, dir.y, 0f) * spawner.maxSpeed;
    }

    private void Awake()
    {
        if (spawner == null)
        {
            BoidManager parentMgr = GetComponentInParent<BoidManager>();
            if (parentMgr != null)
            {
                Initialize(parentMgr);
            }
        }
    }

    private void Update()
    {
        if (spawner == null)
        {
            Debug.LogError($"[{name}] spawner is not assigned.");
            return;
        }

        FindNeighbors();
        velocity += CalculateCohesion() * spawner.cohesionWeight;
        velocity += CalculateAlignment() * spawner.alignmentWeight;
        velocity += CalculateSeparation() * spawner.separationWeight;
        LimitMoveRadius();

        velocity.z = 0f;
        if (velocity.sqrMagnitude > spawner.maxSpeed * spawner.maxSpeed)
            velocity = velocity.normalized * spawner.maxSpeed;

        transform.position += velocity * Time.deltaTime;
        if (velocity.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(velocity);
    }

    private void FindNeighbors()
    {
        if (spawner?.Boids == null) return;

        nearNeighbors.Clear();
        foreach (var other in spawner.Boids)
        {
            if (other == null) continue;
            if (other == this) continue;

            Vector3 diff = other.transform.position - transform.position;
            if (diff.sqrMagnitude < spawner.neighborDistance * spawner.neighborDistance)
            {
                nearNeighbors.Add(other);
                if (nearNeighbors.Count >= spawner.maxNeighbors)
                    break;
            }
        }
        neighborCount = nearNeighbors.Count;
    }

    private Vector3 CalculateCohesion()
    {
        if (nearNeighbors.Count == 0) return Vector3.zero;
        Vector3 center = Vector3.zero;
        foreach (var n in nearNeighbors)
            center += n.transform.position;
        center /= nearNeighbors.Count;
        return (center - transform.position).normalized;
    }

    private Vector3 CalculateAlignment()
    {
        Vector3 avg = transform.forward;
        foreach (var n in nearNeighbors)
            avg += n.transform.forward;
        avg /= (nearNeighbors.Count + 1);
        return avg.normalized;
    }

    private Vector3 CalculateSeparation()
    {
        if (nearNeighbors.Count == 0) return Vector3.zero;
        Vector3 sum = Vector3.zero;
        foreach (var n in nearNeighbors)
            sum += transform.position - n.transform.position;
        sum /= nearNeighbors.Count;
        return sum.normalized;
    }

    private void LimitMoveRadius()
    {
        Vector3 offset = transform.position - spawner.transform.position;
        float dist = offset.magnitude;

        if (dist > spawner.moveRadiusRange)
        {
            Vector3 dirToCenter = -offset.normalized;
            velocity += dirToCenter
                      * (dist - spawner.moveRadiusRange)
                      * spawner.boundaryForce
                      * Time.deltaTime;
        }
    }
}