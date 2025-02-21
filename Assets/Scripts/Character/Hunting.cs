using UnityEngine;

public class Hunting : MonoBehaviour
{
    private bool isHunting;
    private const float huntingTime = 0.1f;

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            AttemptHunt(enemy);
        }
    }

    private bool AttemptHunt(Enemy target)
    {
        if (!isHunting)
        {
            isHunting = true;
            ExecuteHunt(target);
            return true;
        }
        return false;
    }

    private void ExecuteHunt(Enemy target)
    {
        if (target != null)
        {
            Invoke(nameof(ResetIsHunting), huntingTime);
        }
    }

    private void ResetIsHunting()
    {
        isHunting = false;
    }
}
