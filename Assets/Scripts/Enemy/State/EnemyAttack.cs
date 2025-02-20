using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(Enemy enemy) : base(enemy) { }

    public override void OnStateEnter()
    {
        Debug.Log("Enemy entered EnemyAttack state");

        Debug.Log("슈퍼 참치가 죽었습니다.");
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        Debug.Log("Enemy exited EnemyAttack state");
    }
}
