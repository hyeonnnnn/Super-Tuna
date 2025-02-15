using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    private float sightRange = 120f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * enemyData.speed * Time.deltaTime);
    }

    private void DetectPlayer()
    {

    }

    private void chasePlayer()
    {

    }

    private void FleeFromPlayer()
    {

    }
}
