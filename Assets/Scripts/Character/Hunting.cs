using UnityEngine;

public class Hunting : MonoBehaviour
{
    private bool isHunting;
    private const float huntingTime = 0.1f;

    public static bool isPlayerDead = false;

    private HungerSystem hungerSystem;
    private Growth growth;

    private void Start()
    {
        hungerSystem = GetComponent<HungerSystem>();
        growth = GetComponent<Growth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isPlayerDead)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
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
            ExecuteHunt(target);
        }
    }

    private void ExecuteHunt(Enemy target)
    {
        if (target != null)
        {
            Debug.Log("사냥에 성공했습니다.");
            growth.AddExp(target.enemyData.exp);
            hungerSystem.IncreaseHunger(target.enemyData.hungerValue);
            target.gameObject.SetActive(false);
            Invoke(nameof(ResetIsHunting), huntingTime);
        }
    }

    private void ResetIsHunting()
    {
        isHunting = false;
    }
}
