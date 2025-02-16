using UnityEngine;

public class EnemyDie : EnemyState
{
    public EnemyDie(Enemy enemy) : base(enemy) { }

    public override void OnStateEnter()
    {
        Debug.Log("Enemy entered Die state");
        GameObject.Destroy(enemy.gameObject);
    }

    public override void OnStateUpdate()
    {
        
    }
    
    public override void OnStateExit()
    {
        Debug.Log("Enemy exited Die state");
    }
}
