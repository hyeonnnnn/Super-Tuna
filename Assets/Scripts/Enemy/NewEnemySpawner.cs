using System;
using UnityEngine;

public class NewEnemySpawner : MonoBehaviour
{
    public enum FishType
    {
        SmallFish,
        SeaHorse,
        Turtle,
        Dolphin,
        Ray,
        Shark
    }

    [SerializeField] Transform player;
    private float minSpawnDistance = 30f;
    private float maxSpawnDistance = 50f;

    [SerializeField] private int maxSpawnCount;
    [SerializeField] int[] typeProbTable = new int[Enum.GetValues(typeof(FishType)).Length];

    [SerializeField] private float[] waterLayerDepth;

    private void Start()
    {
        for (int i = 0; i < maxSpawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            SpawnEnemy(spawnPosition);
        }
    }


    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomDirection;
        do
        {
            randomDirection = UnityEngine.Random.insideUnitSphere * maxSpawnDistance;
            randomDirection += player.position;
            randomDirection.z = 0;
        } while (Vector3.Distance(player.position, randomDirection) < minSpawnDistance);

        return randomDirection;
    }

    private void SpawnEnemy(Vector3 spawnPosition)
    {
        
    }

    private FishType GetRandomFishType()
    {
        int totalWeight = 0;
        for (int i = 0; i < typeProbTable.Length; i++)
        {
            totalWeight += typeProbTable[i];
        }
        int randomType = UnityEngine.Random.Range(0, totalWeight);
        Debug.Log(randomType);
        int value = 0; //return Value
        for (; value < typeProbTable.Length; value++)
        {
            if (randomType <= typeProbTable[value])
            {
                break;
            }
            else
            {
                randomType -= typeProbTable[value];
            }
        }
        Debug.Log((FishType)value);
        return (FishType)value;

    }


}