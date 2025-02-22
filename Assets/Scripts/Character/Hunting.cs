using UnityEngine;

public class Hunting : MonoBehaviour
{
    private bool isHunting;
    private const float huntingTime = 0.1f;

    [SerializeField] HungerSystem hungerSystem;
    [SerializeField] Growth growth;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌했습니다.");
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            if(growth.CurrentLevel >= enemy.enemyData.level)
            {
                AttemptHunt(enemy);
            }
        }
    }

    private void AttemptHunt(Enemy target)
    {
        if (!isHunting)
        {
            Debug.Log("사냥을 시도합니다.");
            isHunting = true;
            ExecuteHunt(target);
        }
    }

    private void ExecuteHunt(Enemy target)
    {
        if (target != null)
        {
            Debug.Log("사냥에 성공했습니다.");
            hungerSystem.IncreaseHunger(target.enemyData.exp);
            target.gameObject.SetActive(false);
            Invoke(nameof(ResetIsHunting), huntingTime);
        }
    }

    private void ResetIsHunting()
    {
        isHunting = false;
    }
}
