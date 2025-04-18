using UnityEngine;

public abstract class EnemyState
{
    protected Enemy enemy;
    
    // 물고기의 상하 움직임을 위한 변수
    protected float verticalOffset = 0f;
    protected float verticalSpeed = 0.5f;
    protected float verticalRange = 0.5f;

    public EnemyState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
    public virtual void OnTriggerEnter(Collider other) { }
    
    // 물고기의 상하 움직임 적용 메서드
    protected void ApplyVerticalMovement()
    {
        verticalOffset += Time.deltaTime * verticalSpeed;
        float yMovement = Mathf.Sin(verticalOffset) * verticalRange;
        
        Vector3 newPosition = enemy.transform.position;
        newPosition.y += yMovement * Time.deltaTime;
        enemy.transform.position = newPosition;
    }
}

