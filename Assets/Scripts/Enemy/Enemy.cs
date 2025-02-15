using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    private const float fovAngle = 120f;
    private bool isDetected;

    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    private void Update()
    {
        Move();
        CheckDetectPlayer();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * enemyData.speed * Time.deltaTime);
    }

    private void CheckDetectPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float verticalAngle = Vector3.Angle(transform.forward, directionToPlayer);

            if (distanceToPlayer <= enemyData.sightRange && verticalAngle <= fovAngle / 2)
            {
                isDetected = true;
            }
            else
            {
                isDetected = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = isDetected ? _red : _blue;
        Handles.DrawSolidArc(transform.position, transform.right, transform.forward, fovAngle / 2, enemyData.sightRange);
        Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -fovAngle / 2, enemyData.sightRange);
    }

    private void chasePlayer()
    {

    }

    private void FleeFromPlayer()
    {

    }
}
