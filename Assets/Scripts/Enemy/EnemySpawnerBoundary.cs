using UnityEngine;

public class EnemySpawnerBoundary : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;

    public void StopSpawner()
    {
        enemySpawner.DisableSpawner();
    }
}
