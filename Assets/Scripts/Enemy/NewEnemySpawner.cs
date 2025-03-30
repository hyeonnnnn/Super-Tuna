using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    readonly int fishTypeCount = Enum.GetValues(typeof(FishType)).Length;

    struct FishPrefabSet
    {
        public FishType fishType;
        public GameObject[] prefabs;
    }
    Dictionary<FishType, GameObject[]> prefabDatas = new Dictionary<FishType, GameObject[]>();

    [Serializable] struct EnemySpawnData
    {
        public float depth;
        public int[] probTable;
        public int totalWeight;
    }
    struct AllEnemySpawnProbabilities
    {
        public EnemySpawnData[] enemyProbDatas;
    }

    [SerializeField] Transform playerTransform;

    [SerializeField] private int maxEnemyCount;
    private int currentEnemyCount = 0;

    AllEnemySpawnProbabilities enemySpawnPorbTable;
    [SerializeField] GameObject[] enemyTypeList;

    private Vector2[] spawnArea = new Vector2[8];
    private float spawnDistance = 15f;
    private float spawnInternalDistance = 5f;

    FishPrefabSet[] fishPrefabsArray;
    const string prefabsDitectory = "EnemyPrefabs";
    
    private void Start()
    {
        fishPrefabsArray = new FishPrefabSet[enemyTypeList.Length];
        for (int i = 0; i < fishPrefabsArray.Length; i++)
        {
            fishPrefabsArray[i] = new FishPrefabSet();
            fishPrefabsArray[i].fishType = (FishType)i;
            string typeDitectory = prefabsDitectory + "/" + ((FishType)i).ToString();
            fishPrefabsArray[i].prefabs = Resources.LoadAll<GameObject>(typeDitectory);
        }

        for (int i = 0; i < Enum.GetValues(typeof(FishType)).Length; i++)
        {
            string typeDitectory = prefabsDitectory + "/" + ((FishType)i).ToString();
            prefabDatas.Add((FishType)i, Resources.LoadAll<GameObject>(typeDitectory));
        }

        for (int i = 0; i < spawnArea.Length; i++)
        {
            spawnArea[i] = new Vector2(MathF.Cos(Mathf.Deg2Rad * (45 * i)), MathF.Sin(Mathf.Deg2Rad * (45 * i))) * spawnDistance;
        }

        string jsonData = Resources.Load<TextAsset>("EnemySpawnTable").text;
        enemySpawnPorbTable = JsonUtility.FromJson<AllEnemySpawnProbabilities>(jsonData);

        StartCoroutine(TrySpawnEnemy());
    }


    IEnumerator TrySpawnEnemy()
    {
        while(true)
        {
            if (currentEnemyCount < maxEnemyCount)
            {
                SpawnEnemy(GetRandomSpawnPosition());
                currentEnemyCount++;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition;
        randomPosition = UnityEngine.Random.insideUnitSphere * spawnInternalDistance;
        Vector2 randomDir = spawnArea[UnityEngine.Random.Range(0, 8)];
        randomPosition += new Vector3(randomDir.x, randomDir.y, 0) + playerTransform.position;
        randomPosition.z = 0;

        return randomPosition;
    }

    private void SpawnEnemy(Vector3 spawnPosition)
    {
        FishType randomFishType = GetRandomFishType(spawnPosition.y);

        //利 规氢
        float randomRotate = UnityEngine.Random.Range(0, 2);
        if (randomRotate == 0) randomRotate = 0;
        else randomRotate = 180;
        Quaternion randomRotateDir = Quaternion.Euler(new Vector3(0, randomRotate, 0));

        //货肺款 利 按眉 积己
        GameObject newEnemy = Instantiate(enemyTypeList[(int)randomFishType], spawnPosition, randomRotateDir);

        //利 胶鸥老 汲沥
        GameObject randomSelectedFishStyle = Instantiate(fishPrefabsArray[(int)randomFishType].prefabs[UnityEngine.Random.Range(0, fishPrefabsArray[(int)randomFishType].prefabs.Length)],
            spawnPosition, randomRotateDir);

        for (int i = 1; i >= 0; i--)
        {
            randomSelectedFishStyle.transform.GetChild(0).parent = newEnemy.transform;
        }
        Destroy(randomSelectedFishStyle);

        newEnemy.GetComponent<Animator>().Rebind();
        newEnemy.GetComponent<Enemy>().deathEvent += ReduceEnemyCount;
        newEnemy.GetComponent<Enemy>().SetPlayer(playerTransform);
    }

    private FishType GetRandomFishType(float ySpawnPos)
    {
        EnemySpawnData currentSpawnData = enemySpawnPorbTable.enemyProbDatas[0];
        for (int i = enemySpawnPorbTable.enemyProbDatas.Length - 1; i >= 0 ; i--)
        {
            if(enemySpawnPorbTable.enemyProbDatas[i].depth < ySpawnPos)
            {
                currentSpawnData = enemySpawnPorbTable.enemyProbDatas[i];
                break;
            }
        }

        int randomType = UnityEngine.Random.Range(0, currentSpawnData.totalWeight);
        
        int value = 0; //return Value
        for (; value < currentSpawnData.probTable.Length; value++)
        {
            if (randomType <= currentSpawnData.probTable[value])
            {
                break;
            }
            else
            {
                randomType -= currentSpawnData.probTable[value];
            }
        }
        return (FishType)value;

    }

    private void ReduceEnemyCount(GameObject go)
    {
        currentEnemyCount--;
    }
}