using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(Enemy enemy) : base(enemy) { }

    public override void OnStateEnter()
    {

    }
    
    public override void OnStateUpdate()
    {
        Move();
    }

    // ÀÌµ¿
    private void Move()
    {
        Vector3 forward = enemy.transform.forward;
        forward.y = 0;
        forward.Normalize();

        enemy.transform.position += forward * enemy.enemyData.speed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0, enemy.transform.rotation.eulerAngles.y, 0);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);

        if (enemy.IsPlayerDetected())
        {
            if (enemy.enemyData.level > enemy.PlayerLevel)
            {
                enemy.stateManager.ChangeState(enemy.stateManager.chaseState);
            }
            else
            {
                enemy.stateManager.ChangeState(enemy.stateManager.runawayState);
            }
        }
    }


    public override void OnStateExit()
    {

    }
}
