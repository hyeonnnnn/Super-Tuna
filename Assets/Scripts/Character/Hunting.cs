using UnityEngine;

public class Hunting : MonoBehaviour
{
    private bool isHunting;
    private const float huntingTime = 0.1f;

    public static bool isPlayerDead = false;

    [SerializeField] private HungerSystem hungerSystem;
    [SerializeField] private Growth growth;
    [SerializeField] private Enemy enemy;

    private void Awake()
    {
        isPlayerDead = false;
        Debug.Log("Game Restarted: isPlayerDead reset to " + isPlayerDead);
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (!isPlayerDead)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (growth.CurrentLevel >= enemy.enemyData.level)
                {
                    AttemptHunt(enemy);
                }
            }
        }
    }

    private void AttemptHunt(Enemy target)
    {
        if (!isHunting)
        {
            isHunting = true;
            Debug.Log("애니메이션");
            gameObject.GetComponent<Animator>().SetTrigger("Hunting");
            ExecuteHunt(target);
        }
    }

    private void ExecuteHunt(Enemy target)
    {
        if (target != null)
        {
            growth.AddExp(target.enemyData.exp);
            hungerSystem.IncreaseHunger(target.enemyData.hungerValue);
            target.OnTriggerDeath();
            Invoke(nameof(ResetIsHunting), huntingTime);
        }
    }

    private void ResetIsHunting()
    {
        isHunting = false;
    }
}
