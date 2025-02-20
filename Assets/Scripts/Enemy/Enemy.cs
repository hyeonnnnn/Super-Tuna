using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public const float fovAngle = 120f;

    public int PlayerLevel { get; set; } = 2;

    public Transform player;
    public Transform Player => player;
    private EnemyState currentState;

    private Camera mainCamera;
    private float outOfScreenTime = 0f;
    private float outOfScreenDelay = 3f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        mainCamera = Camera.main;

        ChangeState(new EnemyIdle(this));
    }

    private void Update()
    {
        currentState?.OnStateUpdate();
        isOutScreen();
    }

    // 상태 변경
    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = newState;
        currentState.OnStateEnter();
    }

    // 플레이어가 감지되는지 확인
    public bool IsPlayerDetected()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float verticalAngle = Vector3.Angle(transform.forward, directionToPlayer);

        bool isPlayerDetected = distanceToPlayer <= enemyData.sightRange && verticalAngle <= Enemy.fovAngle / 2;

        if (isPlayerDetected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 화면 밖에 있으면 비활성화
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

    // 충돌 확인
    private void OnTriggerEnter(Collider other)
    {
        currentState?.OnTriggerEnter(other);
    }

    // 시야각을 기즈모로 그림
    private void OnDrawGizmos()
    {
        Color _blue = new Color(0f, 0f, 1f, 0.2f);
        Color _red = new Color(1f, 0f, 0f, 0.2f);

        Handles.color = IsPlayerDetected() ? _red : _blue;
        Handles.DrawSolidArc(transform.position, transform.right, transform.forward, fovAngle / 2, enemyData.sightRange);
        Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -fovAngle / 2, enemyData.sightRange);
    }
}
