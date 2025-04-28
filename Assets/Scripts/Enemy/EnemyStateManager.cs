using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyState currentState;
    public EnemyIdle idleState;
    public EnemyChase chaseState;
    public EnemyRunaway runawayState;

    private void Start()
    {
        idleState = new EnemyIdle(GetComponent<Enemy>());
        chaseState = new EnemyChase(GetComponent<Enemy>());
        runawayState = new EnemyRunaway(GetComponent<Enemy>());

        currentState = idleState;
        currentState.OnStateEnter();
    }

    private void Update()
    {
        currentState.OnStateUpdate();
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = newState;
        currentState.OnStateEnter();
    }
}