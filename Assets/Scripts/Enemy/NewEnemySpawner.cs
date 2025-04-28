using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewEnemySpawner : MonoBehaviour
{
    //public static NewEnemySpawner Instance { get; private set; }

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public enum FishType
    {
        SmallFish,
        SeaHorse,
        Turtle,
        Dolphin,
        Ray,
        Shark,
        Mine
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
    //얘도 원래 Resources로 불러오는게 좋을 것 같은데 일단은 그냥 직접 넣는거로 함
    [SerializeField] GameObject[] enemyTypeList;
    [SerializeField] GameObject mineObject;
    private Vector2[] spawnArea = new Vector2[8];
    private float spawnDistance = 12f;
    private float spawnInternalDistance = 5f;
    private float spawnCoolTime = 0.5f;

    const string prefabsDitectory = "EnemyPrefabs";
    
    private void Start()
    {
        LoadFishPrefabs();
        InitializeSpawnArea();
        LoadEnemySpawnProbabilities();

        StartCoroutine(TrySpawnEnemy());
    }

    private void LoadFishPrefabs()
    {
        for (int i = 0; i < Enum.GetValues(typeof(FishType)).Length; i++)
        {
            string typeDitectory = prefabsDitectory + "/" + ((FishType)i).ToString();
            prefabDatas.Add((FishType)i, Resources.LoadAll<GameObject>(typeDitectory));
        }
    }
    private void InitializeSpawnArea()
    {
        for (int i = 0; i < spawnArea.Length; i++)
        {
            spawnArea[i] = new Vector2(MathF.Cos(Mathf.Deg2Rad * (45 * i)), MathF.Sin(Mathf.Deg2Rad * (45 * i))) * spawnDistance;
        }
    }

    private void LoadEnemySpawnProbabilities()
    {
        string jsonData = Resources.Load<TextAsset>("EnemySpawnTable").text;
        enemySpawnPorbTable = JsonUtility.FromJson<AllEnemySpawnProbabilities>(jsonData);
    }

    IEnumerator TrySpawnEnemy()
    {
        while(true)
        {
            if (currentEnemyCount < maxEnemyCount)
            {
                SpawnEnemy(GetRandomSpawnPosition());
            }
            yield return new WaitForSeconds(spawnCoolTime);
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
        if(spawnPosition.y > -11 || spawnPosition.y < -385f)
        {
            return;
        }
        currentEnemyCount++;
        FishType fishType = GetRandomFishType(spawnPosition.y);
        if(fishType == FishType.Mine)
        {
            GameObject newMine = Instantiate(mineObject, spawnPosition, Quaternion.identity);
            newMine.GetComponent<Mine>().ExplodeEvent += ReduceEnemyCount;
            newMine.GetComponent<Mine>().SetPlayer(playerTransform);
            return;
        }
        Quaternion randomRotate = GetRandomRotation();

        GameObject newEnemy = Instantiate(enemyTypeList[(int)fishType], spawnPosition, randomRotate);

        SetEnemyStyle(newEnemy, fishType);

        //if(fishType == FishType.SmallFish)
        //{
        //    newEnemy.GetComponent<BoidManager>().prefab = newEnemy;
        //}

        newEnemy.GetComponent<Animator>().Rebind();
        newEnemy.GetComponent<Enemy>().deathEvent += ReduceEnemyCount;
        newEnemy.GetComponent<Enemy>().SetPlayer(playerTransform);
    }

    private Quaternion GetRandomRotation()
    {
        float randomRotate = UnityEngine.Random.Range(0, 2);
        if (randomRotate == 0) randomRotate = 90;
        else randomRotate = 270;
        Quaternion randomRotateDir = Quaternion.Euler(new Vector3(0, randomRotate, 0));
        return randomRotateDir;
    }

    private void SetEnemyStyle(GameObject newEnemyObject, FishType enemyFishType)
    {
        GameObject newRandomStyle = prefabDatas[enemyFishType][UnityEngine.Random.Range(0, prefabDatas[enemyFishType].Length)];
        GameObject randomSelectedFishStyle = Instantiate(newRandomStyle, newEnemyObject.transform.position, newEnemyObject.transform.rotation);

        float resizeScale = 1f;

        switch(enemyFishType)
        {
            case FishType.SmallFish:
                resizeScale = 1.5f;
                break;
            case FishType.SeaHorse:
                resizeScale = 1.5f;
                break;
            case FishType.Turtle:
                resizeScale = 1.5f;
                break;
            case FishType.Dolphin:
                resizeScale = 1f;
                break;
            case FishType.Ray:
                resizeScale = 1.6f;
                break;
            case FishType.Shark:
                resizeScale = 1f;
                break;
        }
        randomSelectedFishStyle.transform.localScale = randomSelectedFishStyle.transform.localScale * resizeScale;
        for (int i = 1; i >= 0; i--)
        {
            randomSelectedFishStyle.transform.GetChild(0).parent = newEnemyObject.transform;

        }
        Destroy(randomSelectedFishStyle);
    }

    private FishType GetRandomFishType(float ySpawnPos)
    {
        EnemySpawnData currentSpawnData = enemySpawnPorbTable.enemyProbDatas[0];
        for (int i = enemySpawnPorbTable.enemyProbDatas.Length - 1; i >= 0 ; i--)
        {
            if(enemySpawnPorbTable.enemyProbDatas[i].depth > ySpawnPos)
            {
                currentSpawnData = enemySpawnPorbTable.enemyProbDatas[i];
                break;
            }
        }

        int randomType = UnityEngine.Random.Range(0, currentSpawnData.totalWeight);

        for (int i = 0; i < currentSpawnData.probTable.Length; i++)
        {
            if (randomType <= currentSpawnData.probTable[i])
            {
                return (FishType)i;
            }
            randomType -= currentSpawnData.probTable[i];
        }
        return FishType.Mine; // 기본값

    }

    private void ReduceEnemyCount(GameObject go)
    {
        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            go.GetComponent<Enemy>().deathEvent -= ReduceEnemyCount;
        }
        else
        {
            go.GetComponent<Mine>().ExplodeEvent -= ReduceEnemyCount;
        }
        currentEnemyCount--;
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireArc(playerTransform.position, transform.forward, transform.right, 360, spawnDistance, 2.0f);
        Handles.color =  Color.red;
        for (int i = 0; i < spawnArea.Length; i++)
        {
            Handles.DrawWireArc(playerTransform.position + new Vector3(spawnArea[i].x, spawnArea[i].y, 0), transform.forward, transform.right, 360, spawnInternalDistance, 2.0f);
        }
        Handles.color = Color.yellow;
        Handles.DrawWireArc(playerTransform.position, transform.forward, transform.right, 360, 20f, 2.0f);
    }
}