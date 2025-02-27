using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* inspector창 정보
 처음 넣을 경우 MaxActivateSpawnPoint 설정 필요
 EnemyTypeList에 기본 컴포넌트 부착된 프리팹 넣어야함
 그 타입에 맞는 TypeProbTable에 확률 테이블을 작성해야함. 단 이 테이블이 Enum값 기준으로 되어있으므로 Enum에 맞춰서 작성
 이후 자식객체로 스폰 포인트를 넣어주아야함
 단 당연히 MaxActivateSpawnPoint보다 자식객체로 지정된 SpawnPoint가 더 많아야함.
 안그러면 코루틴 평생 돌아가서 메모리 잡아먹음.
 */

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

    [SerializeField] int[] typeProbTable = new int[Enum.GetValues(typeof(FishType)).Length];

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
            if (spawnPoint[i].position == transform.position) { continue; }
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
        //enum의 갯수과 enumTypeList의 갯수가 맞지 않으면 출력하는 에러
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
            int randLocationIndex = UnityEngine.Random.Range(0, disabledSpawnPoint.Count);

            while (CheckInCamera(disabledSpawnPoint[randLocationIndex]))
            {
                yield return null;
                randLocationIndex = UnityEngine.Random.Range(0, disabledSpawnPoint.Count);
            }

            FishType randomFishType = GetRandomFishType();

            //적 방향
            float randomRotate = UnityEngine.Random.Range(0, 2);
            if (randomRotate == 0) randomRotate = 0;
            else randomRotate = 180;
            Quaternion randomRotateDir = Quaternion.Euler(new Vector3(0, randomRotate, 0));

            //새로운 적 객체 생성
            GameObject newEnemy = Instantiate(enemyTypeList[(int)randomFishType], disabledSpawnPoint[randLocationIndex], randomRotateDir);

            //적 스타일 설정
            GameObject randomSelectedFishStyle =  Instantiate(fishPrefabsArray[(int)randomFishType].prefabs[UnityEngine.Random.Range(0, fishPrefabsArray[(int)randomFishType].prefabs.Length)],
                disabledSpawnPoint[randLocationIndex],
                randomRotateDir);

            for (int i = 1; i >= 0; i--)
            {
                randomSelectedFishStyle.transform.GetChild(0).parent = newEnemy.transform;
            }
            Destroy(randomSelectedFishStyle);
            newEnemy.GetComponent<Animator>().Rebind();
            newEnemy.GetComponent<Enemy>().deathEvent += TrySpawnEnemy;

            activatedSpawnedPoint.Add(newEnemy, disabledSpawnPoint[randLocationIndex]);
            disabledSpawnPoint.RemoveAt(randLocationIndex);
            
            if (activatedSpawnedPoint.Count >= maxActivateSpawnPoint)
            {
                isSpawning = false;
            }
        }
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
            if(randomType <= typeProbTable[value])
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

    private bool CheckInCamera(Vector3 position)
    {
        Camera playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
        Vector3 screenPosition = playerCamera.WorldToViewportPoint(position);
        bool onScreen = screenPosition.z > -0.1f && screenPosition.x > -0.1f && screenPosition.y > -0.1f && screenPosition.x < 1.1f && screenPosition.y < 1.1f;

        return onScreen;
    }
}
