using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
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

    [SerializeField] int maxActivateSpawnPoint;
    [SerializeField] Dictionary<GameObject, Vector3> activatedSpawnedPoint = new Dictionary<GameObject, Vector3>();
    [SerializeField] List<Vector3> enabledSpawnPoint = new List<Vector3>();
    [SerializeField] List<Vector3> disabledSpawnPoint = new List<Vector3>();
    [SerializeField] public List<GameObject> enemyTypeList = new List<GameObject>();
    
    //타입 확률 테이블 넣기

    bool isSpawning = false;

    const string prefabsDitectory = "EnemyPrefabs";

    struct FishPrefabSet
    {
        public FishType fishType;
        public GameObject[] prefabs;
    }
    FishPrefabSet[] fishPrefabsArray;

    private void Start()
    {
        Transform[] spawnPoint = GetComponentsInChildren<Transform>();

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            disabledSpawnPoint.Add(spawnPoint[i].position);
        }
        isSpawning = true;

        fishPrefabsArray = new FishPrefabSet[enemyTypeList.Count];
        for (int i = 0;i < fishPrefabsArray.Length;i++)
        {
            fishPrefabsArray[i] = new FishPrefabSet();
            fishPrefabsArray[i].fishType = (FishType)i;
            string typeDitectory = prefabsDitectory + "/" + ((FishType)i).ToString();
            fishPrefabsArray[i].prefabs = Resources.LoadAll<GameObject>(typeDitectory);
        }
#if UNITY_EDITOR
        if (enemyTypeList.Count != System.Enum.GetValues(typeof(FishType)).Length)
        {
            Debug.LogError("enum의 갯수와 enemyTypeList의 갯수가 맞지 않습니다!");
        }
#endif


        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
    #if UNITY_EDITOR
        enabledSpawnPoint.Clear();
        enabledSpawnPoint.AddRange(activatedSpawnedPoint.Values);
    #endif

    }

    private void TrySpawnEnemy(GameObject enemy)
    {
        disabledSpawnPoint.Add(activatedSpawnedPoint[enemy]);
        activatedSpawnedPoint.Remove(enemy);

        if (!isSpawning && maxActivateSpawnPoint > activatedSpawnedPoint.Count)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (isSpawning)
        {
            int randLocationIndex = Random.Range(0, disabledSpawnPoint.Count);

            while (CheckInCamera(disabledSpawnPoint[randLocationIndex]))
            {
                yield return null;
                randLocationIndex = Random.Range(0, disabledSpawnPoint.Count);
            }

            FishType randomFishType = (FishType)Random.Range(0, enemyTypeList.Count);

            //적 방향
            float randomRotate = Random.Range(0, 2);
            if (randomRotate == 0) randomRotate = 0;
            else randomRotate = 180;
            Quaternion randomRotateDir = Quaternion.Euler(new Vector3(0, randomRotate, 0));

            GameObject newEnemy = Instantiate(enemyTypeList[(int)randomFishType], disabledSpawnPoint[randLocationIndex], randomRotateDir);

            GameObject randomSelectedFishStyle =  Instantiate(fishPrefabsArray[(int)randomFishType].prefabs[Random.Range(0, fishPrefabsArray[(int)randomFishType].prefabs.Length)],
                disabledSpawnPoint[randLocationIndex],
                randomRotateDir);

            for (int i = 1; i >= 0; i--)
            {
                randomSelectedFishStyle.transform.GetChild(0).parent = newEnemy.transform;
            }
            Destroy(randomSelectedFishStyle);
            newEnemy.GetComponent<Animator>().Rebind();
            activatedSpawnedPoint.Add(newEnemy, disabledSpawnPoint[randLocationIndex]);
            disabledSpawnPoint.RemoveAt(randLocationIndex);
            
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
