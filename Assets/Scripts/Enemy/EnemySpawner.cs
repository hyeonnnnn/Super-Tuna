using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int maxActivateSpawnPoint;
    [SerializeField] Dictionary<Enemy, Vector3> activatedSpawnedPoint = new Dictionary<Enemy, Vector3>();
    [SerializeField] List<Vector3> disabledSpawnPoint = new List<Vector3>();
    [SerializeField] Dictionary<string, AnimatorController> EnemyTypeAnimatorControllerPair;

    bool isSpawning = false;

    private void Start()
    {
        Transform[] spawnPoint = GetComponentsInChildren<Transform>();

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            disabledSpawnPoint.Add(spawnPoint[i].position);
        }

        isSpawning = true;
        StartCoroutine(SpawnMine());
    }

    private void TrySpawnEnemy(Enemy enemy)
    {
        disabledSpawnPoint.Add(activatedSpawnedPoint[enemy]);
        activatedSpawnedPoint.Remove(enemy);

        if (!isSpawning && maxActivateSpawnPoint > activatedSpawnedPoint.Count)
        {
            isSpawning = true;
            StartCoroutine(SpawnMine());
        }
    }

    private IEnumerator SpawnMine()
    {
        while (isSpawning)
        {
            int randIndex = Random.Range(0, disabledSpawnPoint.Count);

            while (CheckInCamera(disabledSpawnPoint[randIndex]))
            {
                yield return null;
                randIndex = Random.Range(0, disabledSpawnPoint.Count);
            }

            //Enemy newEnemy;
            //newEnemy ~~;
            //Àû ¹æÇâ
            float randRotate = Random.Range(0, 2);
            if (randRotate == 0) randRotate = 90;
            else randRotate = 270;
            //activatedSpawnPoint.Add(newEnemy, disabledSpawnPoint[randIndex]);
            //disabledSpawnPoint.RemoveAt(randIndex);
            //
            

            if (activatedSpawnedPoint.Count >= maxActivateSpawnPoint)
            {
                isSpawning = false;
            }
        }
    }

    private bool CheckInCamera(Vector3 position)
    {
        Camera playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
        Vector3 screenPosition = playerCamera.WorldToViewportPoint(position);
        bool onScreen = screenPosition.z > -0.1f && screenPosition.x > -0.1f && screenPosition.y > -0.1f && screenPosition.x < 1.1f && screenPosition.y < 1.1f;

        return onScreen;
    }
}
