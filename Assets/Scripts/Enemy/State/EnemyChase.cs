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

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.Player.position, chaseBoost * enemy.enemyData.speed * Time.deltaTime);

        Vector3 direction = (enemy.Player.position - enemy.transform.position).normalized;
        direction.z = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
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

    IEnumerator ChangeToIdle()
    {
        float timer = 0f;

        while (timer < detectionTime)
        {
            yield return new WaitForSeconds(1f);
            timer += 1f;

            if (enemy.IsPlayerDetected()) // 플레이어를 다시 감지하면 Chase 유지
                yield break;
        }

        enemy.stateManager.ChangeState(enemy.stateManager.idleState);
    }

    public override void OnStateExit()
    {
        Debug.Log("Enemy exited Chase state");
    }
}
