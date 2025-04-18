using UnityEngine;
using System.Collections;

public class EnemyChase : EnemyState
{
    private const float chaseBoost = 1.7f;
    private const float detectionTime = 5f;

    public EnemyChase(Enemy enemy) : base(enemy) { }

    public override void OnStateEnter()
    {
        Debug.Log("Enemy entered Chase state");
        enemy.StartCoroutine(ChangeToIdle());
    }

    public override void OnStateUpdate()
    {
        if (enemy.Player == null) return;
        
        Vector3 directionToPlayer = (enemy.Player.position - enemy.transform.position).normalized;
        directionToPlayer.z = 0;
        
        float sinOffset = Mathf.Sin(Time.time * 3f) * 0.3f;
        Vector3 curvedDirection = directionToPlayer + enemy.transform.right * sinOffset;
        curvedDirection.Normalize();
        
        enemy.transform.position += curvedDirection * chaseBoost * enemy.enemyData.speed * Time.deltaTime;
        
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 3f);
        
        ApplyVerticalMovement();

        if(enemy.enemyData.level <= enemy.growth.CurrentLevel)
        {
            enemy.stateManager.ChangeState(enemy.stateManager.idleState);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HungerSystem playerHungerSystem = other.gameObject.GetComponent<HungerSystem>();

            if (playerHungerSystem != null)
            {
                playerHungerSystem.TriggerDeath(DyingReason.Enemy);
                enemy.stateManager.ChangeState(enemy.stateManager.idleState);
                Hunting.isPlayerDead = true;
            }
        }
    }

    System.Collections.IEnumerator ChangeToIdle()
    {
        float timer = 0f;

        while (timer < detectionTime)
        {
            yield return null;
            timer += Time.deltaTime;

            if (enemy.IsPlayerDetected())
            {
                timer = 0f;
            }
        }

        enemy.stateManager.ChangeState(enemy.stateManager.idleState);
    }

    public override void OnStateExit()
    {
        Debug.Log("Enemy exited Chase state");
    }
}