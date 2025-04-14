using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RadiationSystem : MonoBehaviour
{ 
    private readonly float radiationDamage = 5f;
    private readonly float radiationSpreadSpeed = 3f;
    private readonly float radiationSpreadDuration = 60f;

    public static event Action<bool> OnRadiationEnter;

    private void Start()
    {
        StartCoroutine(SpreadRadiation());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnRadiationEnter?.Invoke(true);

            if (other.TryGetComponent(out HungerSystem playerHunger))
            {
                playerHunger.AddHungerDecrease((int)radiationDamage);
            }
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnTriggerDeath();
            return;
        }

        EnemySpawnerBoundary enemyBoundary = other.GetComponent<EnemySpawnerBoundary>();
        if (enemyBoundary != null)
        {
            enemyBoundary.StopSpawner();
            return;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out HungerSystem playerHunger))
        {
            OnRadiationEnter?.Invoke(false);
            playerHunger.AddHungerDecrease(0);
        }
    }
    
    private IEnumerator SpreadRadiation() 
    { 
        float elapsedTime = 0f;
        while (elapsedTime < radiationSpreadDuration) 
        { 
            transform.localScale += new Vector3(0, 0, radiationSpreadSpeed) * Time.deltaTime; 
            elapsedTime += Time.deltaTime; 
            yield return null;
        }
    }
}