using UnityEngine;

public class EnemyDie : EnemyState
{
    public EnemyDie(Enemy enemy) : base(enemy) { }

    public override void OnStateEnter()
    {
        Debug.Log("Enemy entered Die state");
    }

    public override void OnStateUpdate()
    {
        enemy.gameObject.SetActive(false);
    }
    
    public override void OnStateExit()
    {
        Debug.Log("Enemy exited Die state");
    }
}
