using UnityEngine;

public class EnemyIdle : EnemyState
{
    private float randomDirectionTimer = 0f;
    private float randomDirectionInterval = 3f;
    private Vector3 randomDirection;

    public EnemyIdle(Enemy enemy) : base(enemy) 
    {
        randomDirection = enemy.transform.forward;
        randomDirectionInterval = Random.Range(2f, 5f);
    }

    public override void OnStateEnter()
    {
        Debug.Log("Enemy entered Idle state");
        float rotationY = (enemy.transform.rotation.eulerAngles.y >= 0 && enemy.transform.rotation.eulerAngles.y <= 180) ? 90f : -90f;
        Quaternion targetRotation = Quaternion.Euler(0, rotationY, 0);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public override void OnStateUpdate()
    {
        Move();
        UpdateRandomDirection();
        ApplyVerticalMovement();
    }

    // 기본 이동
    private void Move()
    {
        Vector3 forward = enemy.transform.forward;
        forward.z = 0;
        forward.Normalize();

        enemy.transform.position += forward * enemy.enemyData.speed * Time.deltaTime;

        if (enemy.IsPlayerDetected() && !Hunting.isPlayerDead)
        {
            if (enemy.enemyData.level > enemy.growth.CurrentLevel)
            {
                enemy.stateManager.ChangeState(enemy.stateManager.chaseState);
            }
            else
            {
                enemy.stateManager.ChangeState(enemy.stateManager.runawayState);
            }
        }
    }
    
    private void UpdateRandomDirection()
    {
        randomDirectionTimer += Time.deltaTime;
        
        if (randomDirectionTimer >= randomDirectionInterval)
        {
            float randomAngle = Random.Range(-30f, 30f);
            randomDirection = Quaternion.Euler(0, randomAngle, 0) * enemy.transform.forward;
            randomDirectionTimer = 0f;
            randomDirectionInterval = Random.Range(2f, 5f);
        }
        
        Vector3 targetDirection = Vector3.Slerp(enemy.transform.forward, randomDirection, Time.deltaTime);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 2f);
    }

    public override void OnStateExit()
    {
    }
}