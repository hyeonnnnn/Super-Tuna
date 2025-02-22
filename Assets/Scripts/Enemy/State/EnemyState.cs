using UnityEngine;

public abstract class EnemyState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
    public virtual void OnCollisionEnter(Collision other) { }
}
