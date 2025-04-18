using UnityEngine;
using System.Collections;

public class EnemyRunaway : EnemyState
{
    private const float runawayBoost = 1.7f;
    private bool isRunning = false;
    private float checkPlayerInterval = 0.5f;
    private float checkTimer = 0f;

    public EnemyRunaway(Enemy enemy) : base(enemy) { }

    public override void OnStateEnter()
    {
        Debug.Log("Enemy entered Runaway state");
        isRunning = true;
    }

    public override void OnStateUpdate()
    {
        if (enemy.Player == null) return;
        
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkPlayerInterval)
        {
            checkTimer = 0f;
            isRunning = enemy.IsPlayerDetected();
            
            if (!isRunning)
            {
                enemy.stateManager.ChangeState(enemy.stateManager.idleState);
                return;
            }
        }
        
        Vector3 fleeDirection = (enemy.transform.position - enemy.Player.position).normalized;
        fleeDirection.z = 0;
        
        float verticalDifference = enemy.transform.position.y - enemy.Player.position.y;
        Vector3 verticalAdjustment = Vector3.zero;
        
        if (verticalDifference < -0.01f)
        {
            verticalAdjustment = Vector3.down * 0.5f;
        }
        else if (verticalDifference > 0.01f)
        {
            verticalAdjustment = Vector3.up * 0.5f;
        }
        
        float zigzagOffset = Mathf.Sin(Time.time * 4f) * 0.4f;
        Vector3 zigzagDirection = fleeDirection + enemy.transform.right * zigzagOffset;
        zigzagDirection.Normalize();
        
        Vector3 finalDirection = zigzagDirection + verticalAdjustment;
        finalDirection.Normalize();
        
        enemy.transform.position += finalDirection * runawayBoost * enemy.enemyData.speed * Time.deltaTime;
        
        fleeDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(fleeDirection);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 4f);
        
        verticalSpeed = 1.0f;
        verticalRange = 0.8f;
        ApplyVerticalMovement();
    }

    public override void OnStateExit()
    {
        Debug.Log("Enemy exited Runaway state");
        isRunning = false;
    }
}
