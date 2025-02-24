using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public const float fovAngle = 120f;
    public int PlayerLevel { get; set; } = 2;

    public Transform player;
    public Transform Player => player;

    private Camera mainCamera;
    private float outOfScreenTime = 0f;
    private float outOfScreenDelay = 3f;

    public EnemyStateManager stateManager;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        stateManager = GetComponent<EnemyStateManager>();
        mainCamera = Camera.main;

        if (player != null)
        {
            LookAtPlayer();
        }
    }

    private void Update()
    {
        isOutScreen();
    }

    public bool IsPlayerDetected()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float verticalAngle = Vector3.Angle(transform.forward, directionToPlayer);

        return distanceToPlayer <= enemyData.sightRange && verticalAngle <= Enemy.fovAngle / 2;
    }

    private void isOutScreen()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);

        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            outOfScreenTime += Time.deltaTime;

            if (outOfScreenTime >= outOfScreenDelay)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            outOfScreenTime = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HungerSystem playerHungerSystem = other.GetComponent<HungerSystem>();

            if (playerHungerSystem != null)
            {
                playerHungerSystem.TriggerDeath(DyingReason.Enemy);
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void OnTriggerDeath()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Color _blue = new Color(0f, 0f, 1f, 0.2f);
        Color _red = new Color(1f, 0f, 0f, 0.2f);

        if (enemyData == null) return;
        Handles.color = IsPlayerDetected() ? _red : _blue;
        Handles.DrawSolidArc(transform.position, transform.right, transform.forward, fovAngle / 2, enemyData.sightRange);
        Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -fovAngle / 2, enemyData.sightRange);
    }
}
