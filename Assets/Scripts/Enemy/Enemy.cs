using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public const float fovAngle = 120f;

    public Transform player;
    public Transform Player => player;

    private Camera mainCamera;
    private float outOfScreenTime = 0f;
    private float outOfScreenDelay = 3f;

    public EnemyStateManager stateManager;
    public Growth growth;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        stateManager = GetComponent<EnemyStateManager>();
        mainCamera = Camera.main;

        if (player != null)
        {
            growth = player.GetComponent<Growth>();
            LookAtPlayer();
        }
    }

    private void Update()
    {
        //isOutScreen();
    }

    // �÷��̾ Ž�� �Ǵ��� Ȯ���ϱ�
    public bool IsPlayerDetected()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float verticalAngle = Vector3.Angle(transform.forward, directionToPlayer);

        return distanceToPlayer <= enemyData.sightRange && verticalAngle <= Enemy.fovAngle / 2;
    }

    // ȭ�� �ۿ� ������ ��Ȱ��ȭ�ϱ�
    private void isOutScreen()
    {
        Vector3[] corners = new Vector3[8];
        Collider collider = GetComponent<Collider>();

        if (collider != null)
        {
            Bounds bounds = collider.bounds;

            corners[0] = bounds.min;
            corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            corners[7] = bounds.max;

            bool isVisible = false;

            foreach (Vector3 corner in corners)
            {
                Vector3 viewPos = mainCamera.WorldToViewportPoint(corner);
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    isVisible = true;
                    break;
                }
            }

            if (!isVisible)
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
    }

    // �÷��̾� �ٶ󺸱�
    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stateManager != null && stateManager.currentState != null)
        {
            stateManager.currentState.OnTriggerEnter(other);
        }
    }

    // �ױ�
    public void OnTriggerDeath()
    {
        gameObject.SetActive(false);
    }

    // �þ߰� �׸���
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
