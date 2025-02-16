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

    private void Move()
    {
        enemy.transform.Translate(Vector3.forward * enemy.enemyData.speed * Time.deltaTime);

        if (enemy.IsPlayerDetected())
        {
            if(enemy.enemyData.level > enemy.PlayerLevel)
            {
                enemy.ChangeState(new EnemyChase(enemy));
            }
            else
            {
                enemy.ChangeState(new EnemyRunaway(enemy));
            }
        }
    }

    public override void OnStateExit()
    {

    }
}
